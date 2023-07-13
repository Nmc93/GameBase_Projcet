using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using ExcelDataReader;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

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
        #region 데이터 경로
        /// <summary> 엑셀 테이블 폴더 경로 </summary>
        private string tablePath = "Table";
        /// <summary> 테이블의 바이너리 폴드 경로 </summary>
        private string tablebytesPath = "Assets\\Resources\\TableBytes";
        /// <summary> 테이블의 CSV 폴더 경로 </summary>
        private string tableCSVPath = "TableCSV";
        /// <summary> 테이블의 CS 폴더 경로 </summary>
        private string tableCSPath = "Assets\\Scripts\\TableData";
        #endregion 데이터 경로 

        #region 검색 변수
        /// <summary> 검색 테이블용 텍스트 </summary>
        private string tableNameText = string.Empty;
        /// <summary> 검색된 테이블을 저장할 저장소 </summary>
        private string[] tableArray;
        /// <summary> 검색된 테이블의 스크롤 포지션 </summary>
        private Vector2 searchScrollPosition;
        #endregion 검색 변수

        #region 선택시 사용할 변수
        /// <summary> 선택된 테이블 이름 </summary>
        private string selectTableName = string.Empty;
        /// <summary> 선택된 엑셀 테이블 경로 </summary>
        private string selectTablePath = string.Empty;
        /// <summary> 선택된 테이블 바이너리 경로 </summary>
        private string selectTableBytesPath = string.Empty;
        /// <summary> 선택된 테이블 CSV 경로 </summary>
        private string selectTableCSVPath = string.Empty;
        /// <summary> 선택된 테이블 CS 경로 </summary>
        private string selectTableCSPath = string.Empty;

        /// <summary> 선택된 테이블의 각 열의 이름들 </summary>
        private List<string> selectColumnNameList = new List<string>();
        /// <summary> 선택된 테이블의 각 열의 타입들 </summary>
        private List<eDataType> selectColumnTypeList = new List<eDataType>();
        /// <summary> 검색된 테이블의 스크롤 포지션 </summary>
        private Vector2 selectScrollPosition;
        #endregion 선택시 사용할 변수

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
                string[] tableNames;
                List<string> tableList = new List<string>();

                //테이블 이름을 입력하지 않았을 경우
                if (string.IsNullOrEmpty(tableNameText))
                {
                    tableNames = Directory.GetFiles(tablePath, "*.xlsx");
                }
                //테이블 이름을 입력했을 경우
                else
                {
                    tableNames = Directory.GetFiles(tablePath, $"*{tableNameText}*");
                }

                for(int i = 0; i < tableNames.Length; ++i)
                {
                    string tableName = tableNames[i].Substring(6);
                    tableName = tableName.Substring(0, tableName.Length - 5);
                    if (tableName.Contains(tableNameText, StringComparison.OrdinalIgnoreCase))
                    {
                        tableList.Add(tableNames[i]);
                    }
                }

                //검색된 테이블 저장
                tableArray = tableList.ToArray();
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
            if (GUILayout.Button($"{selectTableName}.bytes(.csv) 생성/갱신"))
            {
                ConvertExcelToBytes();
            }
            #endregion 테이블 csv 생성 및 갱신

            #region 테이블 cs 생성 및 갱신
            if (GUILayout.Button($"{selectTableName}.cs 생성/갱신"))
            {
                ConvertExcelToCS();
            }
            #endregion 테이블 cs 생성 및 갱신

            #region 엑셀 테이블 폴더 열기
            if (GUILayout.Button("엑셀 테이블 폴더 열기"))
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

            #region 바이너리 폴더 열기
            if (GUILayout.Button("바이너리 폴더 열기"))
            {
                // 주소가 맞는지 확인
                if (Directory.Exists(tablebytesPath))
                {
                    try
                    {
                        //폴더 오픈
                        Process.Start(tablebytesPath);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"폴더 열기 실패 : {e.Message}");
                    }
                }
            }
            #endregion 바이너리 폴더 열기

            #region CSV 폴더 열기
            if (GUILayout.Button("CSV 폴더 열기"))
            {
                // 주소가 맞는지 확인
                if (Directory.Exists(tableCSVPath))
                {
                    try
                    {
                        //폴더 오픈
                        Process.Start(tableCSVPath);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"폴더 열기 실패 : {e.Message}");
                    }
                }
            }
            #endregion CSV 폴더 열기

            #region CS 폴더 열기
            if (GUILayout.Button("CS 폴더 열기"))
            {
                // 주소가 맞는지 확인
                if (Directory.Exists(tableCSPath))
                {
                    try
                    {
                        //폴더 오픈
                        Process.Start(tableCSPath);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"폴더 열기 실패 : {e.Message}");
                    }
                }
            }
            #endregion CS 폴더 열기

            GUILayout.EndArea();
            #endregion 검색한 테이블 컨버트(오른쪽 박스)

            #region 테이블 정보
            //스크롤 크기 설정
            Rect infocrollRect = new Rect(0, 290, position.width, position.height - 290);
            GUILayout.BeginArea(infocrollRect, GUI.skin.window);

            //검색된 테이블이 있을 경우에만
            if (!string.IsNullOrEmpty(selectTablePath))
            {
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
            //바이너리 경로  세팅
            selectTableBytesPath = string.Format("{0}\\{1}Data.bytes", tablebytesPath, selectTableName);
            //CSV 테이블의 경로 세팅
            selectTableCSVPath = string.Format("{0}\\{1}.csv", tableCSVPath, selectTableName);
            //CS 데이터 클래스의 경로 세팅
            selectTableCSPath = string.Format("{0}\\{1}Data.cs", tableCSPath, selectTableName);

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
                                    selectColumnTypeList.Add(ExcelUtility.ConvertStringToeDataType(values[1].ToString()));
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
        #endregion 선택된 엑셀의 데이터 이름과 타입을 반환

        #region 엑셀파일을 CSV,바이너리로 변환
        /// <summary> 엑셀파일을 CSV로 변환 </summary>
        public void ConvertExcelToBytes()
        {
            if (string.IsNullOrEmpty(selectTablePath) || 
                string.IsNullOrEmpty(selectTableCSVPath) || 
                string.IsNullOrEmpty(selectTableBytesPath))
            {
                EditorUtility.DisplayDialog("CSV 생성/갱신", "실패 : 지정된 주소가 없습니다.", "확인");
                return;
            }

            TableData tData = null;
            
            //행 저장용
            List<string> csvList = new List<string>();
            string typeName = $"{selectTableName}Data";
            //csv 데이터 정리 리스트

            //타입 세팅
            Assembly ab = Assembly.LoadFrom("Library/ScriptAssemblies/Assembly-CSharp.dll");
            Type type = ab.GetType(typeName);
            //타입을 찾지 못했을 경우
            if(type == null)
            {
                foreach(var item in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = item.GetType(typeName);
                    if(type != null)
                        break;
                }
            }

            //경로에 있는 엑셀 파일을 읽기 모드로 오픈
            Dictionary<object, TableBase> dicTable = new Dictionary<object, TableBase>();
            using (FileStream file = File.Open(selectTablePath, FileMode.Open, FileAccess.Read))
            {
                //해당 파일을 엑셀 데이터로 변환할 수 있는 데이터 생성
                using (var reader = ExcelReaderFactory.CreateReader(file))
                {
                    #region 타이틀과 타입이 저장된 첫번째 행 저장(타입만 저장함)
                    //첫번째 줄 - 타입 열 세팅
                    reader.Read();

                    //테이블의 열의 타입들
                    string typeStr = string.Empty;

                    //타입 세팅
                    for (int i = 0; i < reader.FieldCount; ++i)
                    {
                        var item = reader[i];
                        if (item != null)
                        {
                            //해당 열이 설명타입일 경우 저장 캔슬
                            if (selectColumnTypeList[i] == eDataType.None)
                                continue;

                            //텍스트 세팅
                            typeStr = i == 0 ?
                                ExcelUtility.ConverteDataTypeToString(selectColumnTypeList[i]) : 
                                $"{typeStr},{ExcelUtility.ConverteDataTypeToString(selectColumnTypeList[i])}";
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("바이너리 생성/갱신", $"실패 : {i}번째 열에 설명이 없습니다.", "확인");
                            return;
                        }
                    }

                    //종료 후 CSV 줄 바꿈을 위해 "" 씌움
                    csvList.Add(typeStr);
                    #endregion 타이틀과 타입이 저장된 첫번째 행 저장(타입만 저장함)

                    #region 정보가 들어있는 두번째 행부터 저장
                    
                    //더 저장할 데이터가 없을 때 까지 진행
                    while (reader.Read())
                    {
                        //테이블 정보의 값 목록
                        List<object> valueList = new List<object>();
                        //테이블 정보 저장
                        string rowText = string.Empty;

                        for (int i = 0; i < reader.FieldCount; ++i)
                        {
                            //해당 데이터의 타입이 None(메모용)일 경우 저장 캔슬
                            if (selectColumnTypeList[i] == eDataType.None)
                                continue;

                            //데이터에 정보가 있는 경우
                            var item = reader[i];
                            if (item != null)
                            {
                                //타입에 맞게 값을 형변환해서 저장
                                valueList.Add(ExcelUtility.GetValue(selectColumnTypeList[i], item.ToString()));
                            }
                            //데이터에 정보가 없는 경우
                            else
                            {
                                //타입에 따라 디폴트 세팅 적용
                                switch(selectColumnTypeList[i])
                                {
                                    case eDataType.Bool:
                                        valueList.Add(false);
                                        break;
                                    case eDataType.Int:
                                    case eDataType.Long:
                                        valueList.Add(0);
                                        break;
                                    case eDataType.String:
                                        valueList.Add(string.Empty);
                                        break;
                                }
                            }

                            //CSV 텍스트 세팅
                            rowText = rowText == string.Empty ? item.ToString() : $"{rowText},{item}";
                        }

                        //종료 후 CSV 줄 바꿈을 위해 "" 씌움
                        csvList.Add(rowText);

                        TableBase tableObj = (TableBase)Activator.CreateInstance(type, valueList.ToArray());
                        dicTable.Add(tableObj.GetKey, tableObj);
                    }
                    #endregion 정보가 들어있는 두번째 행부터 저장

                    tData = new TableData(dicTable);
                }
            }

            #region 데이터 저장
            if (tData != null)
            {
                //바이너리 세팅
                using (FileStream binaryFile = new FileStream(selectTableBytesPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(binaryFile, tData);
                }

                //CSV 세팅
                using (StreamWriter csvWriter = new StreamWriter(selectTableCSVPath, false, System.Text.Encoding.UTF8))
                {
                    csvWriter.Flush();
                    for (int i = 0; i < csvList.Count; ++i)
                    {
                        csvWriter.WriteLine(csvList[i]);
                    }

                    EditorUtility.DisplayDialog("바이너리,CSV 생성/갱신", "완료", "확인");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("바이너리 생성/갱신", $"실패 : 테이블 데이터 생성에 실패했습니다.", "확인");
                return;
            }
            #endregion 데이터 저장
        }
        #endregion 엑셀파일을 CSV,바이너리로 변환

        #region 엑셀파일을 CS로 변환
        private void ConvertExcelToCS()
        {
            //CS파일 생성에 필요한 데이터 세팅
            scGenerator.SetExcelData(selectTableName, selectColumnNameList, selectColumnTypeList);

            //CS 스크립트
            string csText = scGenerator.ConvertExcelToCSText();

            //데이터를 저장
            using (FileStream file = File.Open(selectTableCSPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.Flush();
                    writer.Write(csText);
                }

                EditorUtility.DisplayDialog("CS 생성/갱신", "완료", "확인");
            }
        }
        #endregion 엑셀파일을 CS로 변환
    }

    #region 엑셀에디터 유틸리티
    public static class ExcelUtility
    {
        /// <summary> string에 맞는 eDataType을 반환 </summary>
        public static string ConverteDataTypeToString(eDataType type)
        {
            return type switch
            {
                eDataType.String => "string",
                eDataType.Int => "int",
                eDataType.Long => "long",
                eDataType.Bool => "bool",
                _ => string.Empty,
            };
        }

        /// <summary> string에 맞는 eDataType을 반환 </summary>
        public static eDataType ConvertStringToeDataType(string typeName)
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

        /// <summary> 지정된 타입으로 값을 형변환해서 반환 </summary>
        /// <param name="type"> 값의 타입 </param>
        /// <param name="value"> 테이블값 </param>
        /// <returns> 값이 제대로 지정되지 않으면 string으로 변환 </returns>
        public static object GetValue(eDataType type, string value)
        {
            switch (type)
            {
                case eDataType.Int:
                    return int.Parse(value);
                case eDataType.Long:
                    return long.Parse(value);
                case eDataType.String :
                    return value;
                case eDataType.Bool :
                    return bool.Parse(value);
                default:
                    return value;
            }
        }
    }
    #endregion 엑셀에디터 유틸리티
}