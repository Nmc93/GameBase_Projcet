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

public class ExcelEdit : EditorWindow
{
    /// <summary> ���� ���̺� ���� ��� </summary>
    private string tablePath = "Table";
    /// <summary> ���̺��� CSV ���� ��� </summary>
    private string tableCSVPath = string.Empty;
    /// <summary> ���̺��� CS ���� ��� </summary>
    private string tableCSPath = string.Empty;

    /// <summary> �˻� ���̺�� �ؽ�Ʈ </summary>
    private string tableNameText = string.Empty;

    /// <summary> �˻��� ���̺��� ������ ����� </summary>
    private string[] tableArray;
    /// <summary> �˻��� ���̺��� ��ũ�� ������ </summary>
    private Vector2 searchScrollPosition;

    /// <summary> ���õ� ���̺� ��� </summary>
    string selectTablePath = string.Empty;
    /// <summary> ���õ� ���̺� �̸� </summary>
    string selectTableName = string.Empty;

    /// <summary> �˻��� ���̺��� ��ũ�� ������ </summary>
    private Vector2 selectScrollPosition;

    [MenuItem("GameTool/ExcelEditor")]
    public static void ExcelConverter()
    {
        //������ ��� Ŭ���� ȹ��
        EditorWindow wnd = GetWindow<ExcelEdit>();
        //������ �̸� ����
        wnd.titleContent = new GUIContent("���� ������");
    }

    private void OnGUI()
    {
        #region ���̺� �˻�â
        GUILayout.BeginArea(new Rect(0,0, position.width, 90), GUI.skin.window);
        GUILayout.BeginHorizontal();

        //�ؽ�Ʈ �Է�â(�Է�â �̸�, ���� �ؽ�Ʈ)
        tableNameText = EditorGUILayout.TextField("���̺� �̸� : ", tableNameText);

        //��ư(��ư �̸�)
        if (GUILayout.Button("���̺� �˻�", GUILayout.Width(100f)))
        {
            //���̺� �̸��� �Է����� �ʾ��� ���
            if(string.IsNullOrEmpty(tableNameText))
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
        Rect LeftscrollRect = new Rect(0,90,position.width / 2,200);
        GUILayout.BeginArea(LeftscrollRect, GUI.skin.window);

        //�˻��� ���̺��� ���� ��쿡��
        if (tableArray != null && tableArray.Length > 0)
        {
            //��ũ��
            searchScrollPosition = EditorGUILayout.BeginScrollView(searchScrollPosition);

            for(int i = 0; i < tableArray.Length; ++i)
            {
                if(GUILayout.Button(tableArray[i]))
                {
                    selectTablePath = tableArray[i];
                    selectTableName = GetTableName(selectTablePath);
                    Repaint();
                }
            }

            GUILayout.EndScrollView();
        }

        GUILayout.EndArea();
        #endregion �˻��� ���� ��ũ��(���� �ڽ�)

        #region �˻��� ���̺� ����Ʈ
        GUILayout.BeginArea(new Rect(position.width / 2, 90, position.width / 2, 200), GUI.skin.window);

        #region ���̺� csv ���� �� ����
        if (GUILayout.Button($"{selectTableName}.csv ����/����"))
        {
            ConvertExcelToCSV(selectTablePath);
        }
        #endregion ���̺� csv ���� �� ����

        #region ���̺� cs ���� �� ����
        if (GUILayout.Button($"{selectTableName}.cs ����/����"))
        {

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
        #endregion �˻��� ���̺� ����Ʈ

        #region ���̺� ����
        //��ũ�� ũ�� ����
        Rect infocrollRect = new Rect(0, 290, position.width, position.height - 290);
        GUILayout.BeginArea(infocrollRect, GUI.skin.window);

        //�˻��� ���̺��� ���� ��쿡��
        if (!string.IsNullOrEmpty(selectTablePath))
        {
            List<string> tableList = GetExcelData(selectTablePath);
            if (tableList != null && tableList.Count > 0)
            {
                selectScrollPosition = EditorGUILayout.BeginScrollView(selectScrollPosition);

                for(int i = 0; i < tableList.Count; ++i)
                {
                    string[] tablecell = tableList[i].Split('-');

                    if (tablecell.Length > 1)
                    {
                        int size = 90 - tablecell[0].Length;
                        GUILayout.BeginHorizontal();
                        GUILayout.Label($"�̸� : {tablecell[0].PadRight(size > 0 ? size : 0)} Ÿ�� : {tablecell[1]}");
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

    #region ������ ��ο� �ִ� �������� �̸��� ��ȯ

    /// <summary> ������ ������ �̸��� ��ȯ </summary>
    private string GetTableName(string path)
    {
        if(!string.IsNullOrEmpty(path))
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
    public List<string> GetExcelData(string path)
    {
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
                            list.Add(item.ToString());
                        }

                        return list;
                    }
                }
            }
        }

        return null;
    }
    #endregion ���õ� ������ ������ �̸��� Ÿ���� ��ȯ

    #region ���������� CSV�� ��ȯ
    /// <summary> ���������� CSV�� ��ȯ </summary>
    public void ConvertExcelToCSV(string path)
    {
        //��ο� �ִ� ������ �б� ���� ����
        using (FileStream file = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            //�ش� ������ ���� �����ͷ� ��ȯ�� �� �ִ� ������ ����
            using (var reader = ExcelReaderFactory.CreateReader(file))
            {
                reader.Read();
                List<List<string>> scvList = new List<List<string>>();

                while (reader.Read())
                {
                    List<string> rowList = new List<string>();
                    for (int ii = 0; ii < reader.FieldCount; ii++)
                    {
                        var item = reader[ii];
                        if (item != null)
                        {
                            rowList.Add(item.ToString());
                        }
                    }
                    scvList.Add(rowList);
                }
            }
        }
    }
    #endregion ���������� CSV�� ��ȯ
}
