using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using ExcelDataReader;
using System.Data;
using UnityEditorInternal.VersionControl;
using System.Diagnostics;
using DG.Tweening.Plugins.Core.PathCore;
using static UnityEngine.Rendering.DebugUI;
using Codice.Client.Common.GameUI;

namespace ExcelEdit
{
    public enum eDataType
    {
        None = 0,
        String,
        Int,
        Long,
        Bool,
    }

    public class ExcelEdit : EditorWindow
    {
        /// <summary> 엑셀 테이블 폴더 경로 </summary>
        private string tablePath = "Table";
        /// <summary> 테이블의 CSV 폴더 경로 </summary>
        private string tableCSVPath = "Assets\\Resources\\TableCSV";
        /// <summary> 테이블의 CS 폴더 경로 </summary>
        private string tableCSPath = "Assets\\Scripts\\TableData";

        /// <summary> 검색 테이블용 텍스트 </summary>
        private string tableNameText = string.Empty;

        /// <summary> 검색된 테이블을 저장할 저장소 </summary>
        private string[] tableArray;
        /// <summary> 검색된 테이블의 스크롤 포지션 </summary>
        private Vector2 searchScrollPosition;

        /// <summary> 선택된 테이블 이름 </summary>
        private string selectTableName = string.Empty;
        /// <summary> 선택된 엑셀 테이블 경로 </summary>
        private string selectTablePath = string.Empty;
        /// <summary> 선택된 엑셀 테이블 경로 </summary>
        private string selectTableCSVPath = string.Empty;
        /// <summary> 선택된 엑셀 테이블 경로 </summary>
        private string selectTableCSPath = string.Empty;

        /// <summary> 선택된 테이블의 각 열의 이름들 </summary>
        private List<string> selectColumnNameList = new List<string>();
        /// <summary> 선택된 테이블의 각 열의 타입들 </summary>
        private List<eDataType> selectColumnTypeList = new List<eDataType>();

        /// <summary> 검색된 테이블의 스크롤 포지션 </summary>
        private Vector2 selectScrollPosition;

        /// <summary> CS생성기 </summary>
        private ScriptGenerator scGenerator = new ScriptGenerator();

        [MenuItem("GameTool/ExcelEditor")]
        public static void ExcelConverter()
        {
            //에디터 대상 클래스 획득
            EditorWindow wnd = GetWindow<ExcelEdit>();
            //에디터 이름 지정
            wnd.titleContent = new GUIContent("ExcelEdit");
        }

        private void OnGUI()
        {
            #region 테이블 검색창
            GUILayout.BeginArea(new Rect(0, 0, position.width, 90), GUI.skin.window);
            GUILayout.BeginHorizontal();

            //텍스트 입력창(입력창 이름, 시작 텍스트)
            tableNameText = EditorGUILayout.TextField("테이블 이름 : ", tableNameText);

            //버튼(버튼 이름)
            if (GUILayout.Button("테이블 검색", GUILayout.Width(100f)))
            {
                //테이블 이름을 입력하지 않았을 경우
                if (string.IsNullOrEmpty(tableNameText))
                {
                    tableArray = Directory.GetFiles(tablePath, "*.xlsx");
                }
                //테이블 이름을 입력했을 경우
                else
                {
                    tableArray = Directory.GetFiles(tablePath, $"*{tableNameText}*");
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            //엑셀 스크롤 타이틀
            GUILayout.Label($"선택한 엑셀 : {selectTableName}", EditorStyles.label);

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            #endregion 테이블 검색창

            #region 검색된 엑셀 스크롤(왼쪽 박스)
            //스크롤 크기 설정
            Rect LeftscrollRect = new Rect(0, 90, position.width / 2, 200);
            GUILayout.BeginArea(LeftscrollRect, GUI.skin.window);

            //검색된 테이블이 있을 경우에만
            if (tableArray != null && tableArray.Length > 0)
            {
                //스크롤
                searchScrollPosition = EditorGUILayout.BeginScrollView(searchScrollPosition);

                for (int i = 0; i < tableArray.Length; ++i)
                {
                    if (GUILayout.Button(tableArray[i]))
                    {
                        //테이블 데이터 세팅
                        SetSelectTableDatas(tableArray[i]);

                        //갱신
                        Repaint();
                    }
                }

                GUILayout.EndScrollView();
            }

            GUILayout.EndArea();
            #endregion 검색된 엑셀 스크롤(왼쪽 박스)

            #region 검색한 테이블 컨버트(오른쪽 박스)
            GUILayout.BeginArea(new Rect(position.width / 2, 90, position.width / 2, 200), GUI.skin.window);

            #region 테이블 csv 생성 및 갱신
            if (GUILayout.Button($"{selectTableName}.csv 생성/갱신"))
            {
                ConvertExcelToCSV();
            }
            #endregion 테이블 csv 생성 및 갱신

            #region 테이블 cs 생성 및 갱신
            if (GUILayout.Button($"{selectTableName}.cs 생성/갱신"))
            {
                ConvertExcelToCS();
            }
            #endregion 테이블 cs 생성 및 갱신

            #region 엑셀 테이블 폴더 열기
            if (GUILayout.Button("폴더 열기"))
            {
                // 주소가 맞는지 확인
                if (Directory.Exists(tablePath))
                {
                    try
                    {
                        //폴더 오픈
                        Process.Start(tablePath);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"폴더 열기 실패 : {e.Message}");
                    }
                }
            }
            #endregion 엑셀 테이블 폴더 열기

            GUILayout.EndArea();
            #endregion 검색한 테이블 컨버트(오른쪽 박스)

            #region 테이블 정보
            //스크롤 크기 설정
            Rect infocrollRect = new Rect(0, 290, position.width, position.height - 290);
            GUILayout.BeginArea(infocrollRect, GUI.skin.window);

            //검색된 테이블이 있을 경우에만
            if (!string.IsNullOrEmpty(selectTablePath))
            {
                //selectColumnNameList[i]
                //selectColumnTypeList[i]

                if (selectColumnNameList != null && selectColumnNameList.Count > 0)
                {
                    selectScrollPosition = EditorGUILayout.BeginScrollView(selectScrollPosition);

                    for (int i = 0; i < selectColumnNameList.Count; ++i)
                    {
                        if (selectColumnTypeList[i] != eDataType.None)
                        {
                            int size = 90 - selectColumnNameList[i].Length;
                            GUILayout.BeginHorizontal();
                            GUILayout.Label($"이름 : {selectColumnNameList[i].PadRight(size > 0 ? size : 0)} 타입 : {selectColumnTypeList[i]}");
                            GUILayout.EndHorizontal();
                            GUILayout.Space(2);
                        }
                    }

                    GUILayout.EndScrollView();
                }
            }

            GUILayout.EndArea();
            #endregion 테이블 정보
        }

        #region 선택한 테이블의 정보를 획득
        /// <summary> 선택한 테이블의 정보를 획득 </summary>
        public void SetSelectTableDatas(string excelPath)
        {
            //테이블의 이름 세팅
            selectTableName = GetTableName(excelPath);

            //엑셀 테이블의 경로 세팅
            selectTablePath = excelPath;
            //CSV 테이블의 경로 세팅
            selectTableCSVPath = string.Format("{0}\\{1}.csv", tableCSVPath, selectTableName);
            //CS 데이터 클래스의 경로 세팅
            selectTableCSPath = string.Format("{0}\\{1}.cs", tableCSPath, selectTableName);

            //테이블의 이름과 타입 세팅
            SetExcelData(selectTablePath);
        }
        #endregion 선택한 테이블의 정보를 획득 

        #region 선택한 경로에 있는 엑셀파일 이름을 반환

        /// <summary> 선택한 엑셀의 이름을 반환 </summary>
        private string GetTableName(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string[] firstText = path.Split(".");
                if (firstText.Length > 0)
                {
                    string[] lastText = firstText[0].Split("\\");
                    if (lastText.Length > 0)
                    {
                        return lastText[lastText.Length - 1];
                    }
                }

            }
            return string.Empty;
        }

        #endregion 선택한 경로에 있는 엑셀파일 이름을 반환

        #region 선택된 엑셀의 데이터 이름과 타입을 반환
        /// <summary> 선택된 엑셀의 데이터 이름과 타입을 반환 </summary>
        public void SetExcelData(string path)
        {
            selectColumnNameList.Clear();
            selectColumnTypeList.Clear();

            //경로에 있는 파일을 읽기 모드로 오픈
            using (FileStream file = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                //해당 파일을 엑셀 데이터로 변환할 수 있는 데이터 생성
                using (var reader = ExcelReaderFactory.CreateReader(file))
                {
                    //데이터셋으로 변환
                    DataSet data = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = item => new ExcelDataTableConfiguration()
                        {
                            //첫번째 행을 열 이름으로 사용하지 않음
                            UseHeaderRow = false
                        }
                    });

                    if (data.Tables.Count > 0)
                    {
                        //읽은 테이블의 첫번째 페이지를 확인
                        DataTable TableData = data.Tables[0];
                        if (TableData.Rows.Count > 0)
                        {
                            //테이블의 첫번째 행에 접근
                            DataRow row = TableData.Rows[0];
                            //첫번째 행의 모든 열에 대한 정보를 획득
                            object[] firstData = row.ItemArray;

                            //첫번째 행의 열을 전부 String으로 변환해서 List에 넣음
                            List<string> list = new List<string>();
                            foreach (var item in firstData)
                            {
                                string[] values = item.ToString().Split("-");

                                //테이블 값 열
                                if (values.Length == 2)
                                {
                                    selectColumnNameList.Add(values[0].ToString());
                                    selectColumnTypeList.Add(ConvertStringToeDataType(values[1].ToString()));
                                }
                                //설명용 열
                                else if (values.Length == 1)
                                {
                                    selectColumnNameList.Add(values[0].ToString());
                                    selectColumnTypeList.Add(eDataType.None);
                                }
                                else
                                {
                                    UnityEngine.Debug.LogError($" 잘못된 테이블 열 형식입니다. : {values.Length}");
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary> string에 맞는 eDataType을 반환 </summary>
        private eDataType ConvertStringToeDataType(string typeName)
        {
            return typeName switch
            {
                "string" => eDataType.String,
                "int" => eDataType.Int,
                "long" => eDataType.Long,
                "bool" => eDataType.Bool,
                _ => eDataType.None,
            };
        }

        #endregion 선택된 엑셀의 데이터 이름과 타입을 반환

        #region 엑셀파일을 CSV로 변환
        /// <summary> 엑셀파일을 CSV로 변환 </summary>
        public void ConvertExcelToCSV()
        {
            if (string.IsNullOrEmpty(selectTablePath) ||
                string.IsNullOrEmpty(selectTableCSVPath))
            {
                UnityEngine.Debug.LogError("지정된 주소가 없습니다.");
                return;
            }

            //csv 데이터 정리 리스트
            List<string> scvList = new List<string>();

            //경로에 있는 엑셀 파일을 읽기 모드로 오픈
            using (FileStream file = File.Open(selectTablePath, FileMode.Open, FileAccess.Read))
            {
                //해당 파일을 엑셀 데이터로 변환할 수 있는 데이터 생성
                using (var reader = ExcelReaderFactory.CreateReader(file))
                {
                    //열의 타이틀과 타입이 저장된 첫번째 행은 스킵
                    reader.Read();

                    //정보가 들어있는 두번째 행부터 저장
                    while (reader.Read())
                    {
                        //행 저장용
                        string rowText = string.Empty;
                        for (int i = 0; i < reader.FieldCount; ++i)
                        {
                            //
                            var item = reader[i];
                            if (item != null)
                            {
                                rowText = rowText == string.Empty ?
                                    item.ToString() : string.Format("{0},{1}", rowText, item.ToString());
                            }
                        }
                        scvList.Add(rowText);
                    }
                }
            }

            //데이터를 저장
            using (FileStream file = File.Open(selectTableCSVPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    for (int i = 0; i < scvList.Count; ++i)
                    {
                        writer.WriteLine(scvList[i]);
                    }
                }

                EditorUtility.DisplayDialog("CSV 생성/갱신", "완료", "확인");
            }
        }
        #endregion 엑셀파일을 CSV로 변환

        #region 엑셀파일을 CS로 변환

        private void ConvertExcelToCS()
        {
            //CS파일 생성에 필요한 데이터 세팅
            scGenerator.SetExcelData(selectTableName, selectColumnNameList, selectColumnTypeList);

            //CS 스크립트
            string csText = scGenerator.ConvertExcelToCS();

            //데이터를 저장
            using (FileStream file = File.Open(selectTableCSPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.Write(csText);
                }

                EditorUtility.DisplayDialog("CS 생성/갱신", "완료", "확인");
            }
        }

        #endregion 엑셀파일을 CS로 변환

    }
}