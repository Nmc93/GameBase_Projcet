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
        /// <summary> ���� ���̺� ���� ��� </summary>
        private string tablePath = "Table";
        /// <summary> ���̺��� CSV ���� ��� </summary>
        private string tableCSVPath = "Assets\\Resources\\TableCSV";
        /// <summary> ���̺��� CS ���� ��� </summary>
        private string tableCSPath = "Assets\\Scripts\\TableData";

        /// <summary> �˻� ���̺�� �ؽ�Ʈ </summary>
        private string tableNameText = string.Empty;

        /// <summary> �˻��� ���̺��� ������ ����� </summary>
        private string[] tableArray;
        /// <summary> �˻��� ���̺��� ��ũ�� ������ </summary>
        private Vector2 searchScrollPosition;

        /// <summary> ���õ� ���̺� �̸� </summary>
        private string selectTableName = string.Empty;
        /// <summary> ���õ� ���� ���̺� ��� </summary>
        private string selectTablePath = string.Empty;
        /// <summary> ���õ� ���� ���̺� ��� </summary>
        private string selectTableCSVPath = string.Empty;
        /// <summary> ���õ� ���� ���̺� ��� </summary>
        private string selectTableCSPath = string.Empty;

        /// <summary> ���õ� ���̺��� �� ���� �̸��� </summary>
        private List<string> selectColumnNameList = new List<string>();
        /// <summary> ���õ� ���̺��� �� ���� Ÿ�Ե� </summary>
        private List<eDataType> selectColumnTypeList = new List<eDataType>();

        /// <summary> �˻��� ���̺��� ��ũ�� ������ </summary>
        private Vector2 selectScrollPosition;

        /// <summary> CS������ </summary>
        private ScriptGenerator scGenerator = new ScriptGenerator();

        [MenuItem("GameTool/ExcelEditor")]
        public static void ExcelConverter()
        {
            //������ ��� Ŭ���� ȹ��
            EditorWindow wnd = GetWindow<ExcelEdit>();
            //������ �̸� ����
            wnd.titleContent = new GUIContent("ExcelEdit");
        }

        private void OnGUI()
        {
            #region ���̺� �˻�â
            GUILayout.BeginArea(new Rect(0, 0, position.width, 90), GUI.skin.window);
            GUILayout.BeginHorizontal();

            //�ؽ�Ʈ �Է�â(�Է�â �̸�, ���� �ؽ�Ʈ)
            tableNameText = EditorGUILayout.TextField("���̺� �̸� : ", tableNameText);

            //��ư(��ư �̸�)
            if (GUILayout.Button("���̺� �˻�", GUILayout.Width(100f)))
            {
                //���̺� �̸��� �Է����� �ʾ��� ���
                if (string.IsNullOrEmpty(tableNameText))
                {
                    tableArray = Directory.GetFiles(tablePath, "*.xlsx");
                }
                //���̺� �̸��� �Է����� ���
                else
                {
                    tableArray = Directory.GetFiles(tablePath, $"*{tableNameText}*");
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            //���� ��ũ�� Ÿ��Ʋ
            GUILayout.Label($"������ ���� : {selectTableName}", EditorStyles.label);

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            #endregion ���̺� �˻�â

            #region �˻��� ���� ��ũ��(���� �ڽ�)
            //��ũ�� ũ�� ����
            Rect LeftscrollRect = new Rect(0, 90, position.width / 2, 200);
            GUILayout.BeginArea(LeftscrollRect, GUI.skin.window);

            //�˻��� ���̺��� ���� ��쿡��
            if (tableArray != null && tableArray.Length > 0)
            {
                //��ũ��
                searchScrollPosition = EditorGUILayout.BeginScrollView(searchScrollPosition);

                for (int i = 0; i < tableArray.Length; ++i)
                {
                    if (GUILayout.Button(tableArray[i]))
                    {
                        //���̺� ������ ����
                        SetSelectTableDatas(tableArray[i]);

                        //����
                        Repaint();
                    }
                }

                GUILayout.EndScrollView();
            }

            GUILayout.EndArea();
            #endregion �˻��� ���� ��ũ��(���� �ڽ�)

            #region �˻��� ���̺� ����Ʈ(������ �ڽ�)
            GUILayout.BeginArea(new Rect(position.width / 2, 90, position.width / 2, 200), GUI.skin.window);

            #region ���̺� csv ���� �� ����
            if (GUILayout.Button($"{selectTableName}.csv ����/����"))
            {
                ConvertExcelToCSV();
            }
            #endregion ���̺� csv ���� �� ����

            #region ���̺� cs ���� �� ����
            if (GUILayout.Button($"{selectTableName}.cs ����/����"))
            {
                ConvertExcelToCS();
            }
            #endregion ���̺� cs ���� �� ����

            #region ���� ���̺� ���� ����
            if (GUILayout.Button("���� ����"))
            {
                // �ּҰ� �´��� Ȯ��
                if (Directory.Exists(tablePath))
                {
                    try
                    {
                        //���� ����
                        Process.Start(tablePath);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"���� ���� ���� : {e.Message}");
                    }
                }
            }
            #endregion ���� ���̺� ���� ����

            GUILayout.EndArea();
            #endregion �˻��� ���̺� ����Ʈ(������ �ڽ�)

            #region ���̺� ����
            //��ũ�� ũ�� ����
            Rect infocrollRect = new Rect(0, 290, position.width, position.height - 290);
            GUILayout.BeginArea(infocrollRect, GUI.skin.window);

            //�˻��� ���̺��� ���� ��쿡��
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
                            GUILayout.Label($"�̸� : {selectColumnNameList[i].PadRight(size > 0 ? size : 0)} Ÿ�� : {selectColumnTypeList[i]}");
                            GUILayout.EndHorizontal();
                            GUILayout.Space(2);
                        }
                    }

                    GUILayout.EndScrollView();
                }
            }

            GUILayout.EndArea();
            #endregion ���̺� ����
        }

        #region ������ ���̺��� ������ ȹ��
        /// <summary> ������ ���̺��� ������ ȹ�� </summary>
        public void SetSelectTableDatas(string excelPath)
        {
            //���̺��� �̸� ����
            selectTableName = GetTableName(excelPath);

            //���� ���̺��� ��� ����
            selectTablePath = excelPath;
            //CSV ���̺��� ��� ����
            selectTableCSVPath = string.Format("{0}\\{1}.csv", tableCSVPath, selectTableName);
            //CS ������ Ŭ������ ��� ����
            selectTableCSPath = string.Format("{0}\\{1}.cs", tableCSPath, selectTableName);

            //���̺��� �̸��� Ÿ�� ����
            SetExcelData(selectTablePath);
        }
        #endregion ������ ���̺��� ������ ȹ�� 

        #region ������ ��ο� �ִ� �������� �̸��� ��ȯ

        /// <summary> ������ ������ �̸��� ��ȯ </summary>
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

        #endregion ������ ��ο� �ִ� �������� �̸��� ��ȯ

        #region ���õ� ������ ������ �̸��� Ÿ���� ��ȯ
        /// <summary> ���õ� ������ ������ �̸��� Ÿ���� ��ȯ </summary>
        public void SetExcelData(string path)
        {
            selectColumnNameList.Clear();
            selectColumnTypeList.Clear();

            //��ο� �ִ� ������ �б� ���� ����
            using (FileStream file = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                //�ش� ������ ���� �����ͷ� ��ȯ�� �� �ִ� ������ ����
                using (var reader = ExcelReaderFactory.CreateReader(file))
                {
                    //�����ͼ����� ��ȯ
                    DataSet data = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = item => new ExcelDataTableConfiguration()
                        {
                            //ù��° ���� �� �̸����� ������� ����
                            UseHeaderRow = false
                        }
                    });

                    if (data.Tables.Count > 0)
                    {
                        //���� ���̺��� ù��° �������� Ȯ��
                        DataTable TableData = data.Tables[0];
                        if (TableData.Rows.Count > 0)
                        {
                            //���̺��� ù��° �࿡ ����
                            DataRow row = TableData.Rows[0];
                            //ù��° ���� ��� ���� ���� ������ ȹ��
                            object[] firstData = row.ItemArray;

                            //ù��° ���� ���� ���� String���� ��ȯ�ؼ� List�� ����
                            List<string> list = new List<string>();
                            foreach (var item in firstData)
                            {
                                string[] values = item.ToString().Split("-");

                                //���̺� �� ��
                                if (values.Length == 2)
                                {
                                    selectColumnNameList.Add(values[0].ToString());
                                    selectColumnTypeList.Add(ConvertStringToeDataType(values[1].ToString()));
                                }
                                //����� ��
                                else if (values.Length == 1)
                                {
                                    selectColumnNameList.Add(values[0].ToString());
                                    selectColumnTypeList.Add(eDataType.None);
                                }
                                else
                                {
                                    UnityEngine.Debug.LogError($" �߸��� ���̺� �� �����Դϴ�. : {values.Length}");
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary> string�� �´� eDataType�� ��ȯ </summary>
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

        #endregion ���õ� ������ ������ �̸��� Ÿ���� ��ȯ

        #region ���������� CSV�� ��ȯ
        /// <summary> ���������� CSV�� ��ȯ </summary>
        public void ConvertExcelToCSV()
        {
            if (string.IsNullOrEmpty(selectTablePath) ||
                string.IsNullOrEmpty(selectTableCSVPath))
            {
                UnityEngine.Debug.LogError("������ �ּҰ� �����ϴ�.");
                return;
            }

            //csv ������ ���� ����Ʈ
            List<string> scvList = new List<string>();

            //��ο� �ִ� ���� ������ �б� ���� ����
            using (FileStream file = File.Open(selectTablePath, FileMode.Open, FileAccess.Read))
            {
                //�ش� ������ ���� �����ͷ� ��ȯ�� �� �ִ� ������ ����
                using (var reader = ExcelReaderFactory.CreateReader(file))
                {
                    //���� Ÿ��Ʋ�� Ÿ���� ����� ù��° ���� ��ŵ
                    reader.Read();

                    //������ ����ִ� �ι�° ����� ����
                    while (reader.Read())
                    {
                        //�� �����
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

            //�����͸� ����
            using (FileStream file = File.Open(selectTableCSVPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    for (int i = 0; i < scvList.Count; ++i)
                    {
                        writer.WriteLine(scvList[i]);
                    }
                }

                EditorUtility.DisplayDialog("CSV ����/����", "�Ϸ�", "Ȯ��");
            }
        }
        #endregion ���������� CSV�� ��ȯ

        #region ���������� CS�� ��ȯ

        private void ConvertExcelToCS()
        {
            //CS���� ������ �ʿ��� ������ ����
            scGenerator.SetExcelData(selectTableName, selectColumnNameList, selectColumnTypeList);

            //CS ��ũ��Ʈ
            string csText = scGenerator.ConvertExcelToCS();

            //�����͸� ����
            using (FileStream file = File.Open(selectTableCSPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.Write(csText);
                }

                EditorUtility.DisplayDialog("CS ����/����", "�Ϸ�", "Ȯ��");
            }
        }

        #endregion ���������� CS�� ��ȯ

    }
}