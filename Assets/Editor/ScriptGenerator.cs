using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExcelEdit;

namespace ExcelEdit
{
    public class ScriptGenerator
    {
        #region Const Template

        /// <summary> 클래스 이름 템플릿<br/> $CSName$ : 테이블이름</summary>
        const string csNameTemplate = "$CSName$Data";
        /// <summary> 클래스 몸통 템플릿 <br/> $CSName$ : 이름 <br/> $TblName$ : 테이블이름<br/> $GetKey$ : 키값 <br/> $Const$ : 생성자 <br/> $Values$ : 변수와 프로퍼티</summary>
        const string csBodyTemplate = "using System;\nusing UnityEngine;\n \n[Serializable]\npublic class $CSName$ : TableBase\n{\n\t$TblName$\n\t$GetKey$\n\n$Const$\n$Values$\n}";

        /// <summary>테이블 이름 프로퍼티 <br/> $Name$ : 이름 </summary>
        const string csTblNameTemplate = "public override string TableName { get => \"$Name$\"; }";
        /// <summary> 키값 <br/> $Key$ : 키값 </summary>
        const string csGetKeyTemplat = "public override object GetKey { get => $Key$; }";

        /// <summary> 변수와 프로퍼티 <br/> $Type$ : 타입<br/> $LowName$ : 소문자 변수이름<br/> $Name$ : 변수 이름) </summary>
        const string csParamTemplate = "\n\t[SerializeField]\n\tprivate $Type$ $LowName$;\n\tpublic $Type$ $Name$ { get => $LowName$; }";

        /// <summary> 생성자 <br/> $CSName$ : 클래스 이름<br/> $Params$ : 매개변수<br/> $Value$ : 생성자 </summary>
        const string csConstBody = "\tpublic $CSName$ ($Params$) \n\t{\n$Value$\n\t}";
        /// <summary> 생성자 매개변수<br/> $Type$ : 변수 타입<br/> $LowName$ : 변수 이름 </summary>
        const string csConstParam = "$Type$ $LowName$";
        /// <summary> 생성자 데이터 세팅<br/> $Name$ : name </summary>
        const string csConstvalue = "this.$LowName$ = $LowName$;";

        #endregion Const Template

        #region 테이블 생성 변수
        /// <summary> 테이블 이름 </summary>
        private string tableName;
        /// <summary> 테이블 열 이름 </summary>
        private List<string> columnNameList = new List<string>();
        /// <summary> 테이블 열 타입 </summary>
        private List<eDataType> columnTypeList = new List<eDataType>();
        #endregion  테이블 생성 변수

        #region 엑셀파일을 CS로 변환

        /// <summary> 데이터 세팅 </summary>
        public void SetExcelData(string tableName, List<string> columnNameList, List<eDataType> columnTypeList)
        {
            this.tableName = tableName;
            this.columnNameList = columnNameList;
            this.columnTypeList = columnTypeList;
        }

        /// <summary> 엑셀을 CS 파일로 변경 </summary>
        public string ConvertExcelToCSText()
        {
            //1. 클래스 이름
            string tableCSName = csNameTemplate.Replace("$CSName$", tableName);

            //2. 변수와 프로퍼티
            string cValue = string.Empty;
            #region 변수와 프로퍼티 세팅

            for (int i = 0; i < columnNameList.Count; ++i)
            {
                if (string.IsNullOrEmpty(cValue))
                {
                    if (columnTypeList[i] != eDataType.None)
                    {
                        //ex : private int a;
                        //     public int A { get => a; }
                        cValue = string.Format("\t{0}", csParamTemplate
                        .Replace("$Type$", ExcelUtility.ConverteDataTypeToString(columnTypeList[i]))
                        .Replace("$LowName$", columnNameList[i].ToLower())
                        .Replace("$Name$", columnNameList[i]));
                    }
                }
                else
                {
                    if (columnTypeList[i] != eDataType.None)
                    {
                        //ex : private int a;
                        //     public int A { get => a; }
                        //     private int b;
                        //     public int B { get => b; }
                        cValue = string.Format("{0}\n\t{1}", cValue, csParamTemplate
                        .Replace("$Type$", ExcelUtility.ConverteDataTypeToString(columnTypeList[i]))
                        .Replace("$LowName$", columnNameList[i].ToLower())
                        .Replace("$Name$", columnNameList[i]));
                    }
                }
            }

            #endregion 변수와 프로퍼티 세팅

            //3. 생성자 생성 - 생성자의 매개변수와 데이터 세팅
            string constString1 = string.Empty;
            string constString2 = string.Empty;
            string cConstBody = string.Empty;
            #region 생성자 세팅
            for (int i = 0; i < columnNameList.Count; ++i)
            {
                if (string.IsNullOrEmpty(constString1))
                {
                    if (columnTypeList[i] != eDataType.None)
                    {
                        //ex : int a
                        constString1 = csConstParam
                            .Replace("$Type$", ExcelUtility.ConverteDataTypeToString(columnTypeList[i]))
                            .Replace("$LowName$", columnNameList[i].ToLower());
                        //ex : this.a = a;
                        constString2 = string.Format("\t\t{0}", csConstvalue
                            .Replace("$LowName$", columnNameList[i].ToLower()));
                    }
                }
                else
                {
                    if (columnTypeList[i] != eDataType.None)
                    {
                        //ex : int a,int b
                        constString1 = string.Format("{0}, {1}", constString1, csConstParam
                            .Replace("$Type$", ExcelUtility.ConverteDataTypeToString(columnTypeList[i]))
                            .Replace("$LowName$", columnNameList[i].ToLower()));
                        //ex : this.a = a;
                        //     this.b = b;
                        constString2 = string.Format("{0}\n\t\t{1}", constString2, csConstvalue
                            .Replace("$LowName$", columnNameList[i].ToLower()));
                    }
                }
            }

            //3-1 생성자 생성
            cConstBody = csConstBody
                .Replace("$CSName$", tableCSName)
                .Replace("$Params$", constString1)
                .Replace("$Value$", constString2);

            #endregion 생성자 생성

            //테이블 이름 프로퍼티 생성
            string tblName = csTblNameTemplate.Replace("$Name$", tableName);
            //키값 프로퍼티 생성
            string getKey = csGetKeyTemplat.Replace("$Key$", columnNameList[0].ToLower());

            //4. 클래스 몸통 생성
            string cBody = csBodyTemplate
                .Replace("$CSName$", tableCSName)
                .Replace("$TblName$", tblName)
                .Replace("$GetKey$", getKey)
                .Replace("$Const$", cConstBody)
                .Replace("$Values$", cValue);
            
            return cBody;
        }
        #endregion 엑셀파일을 CS로 변환ㅡ
    }
}