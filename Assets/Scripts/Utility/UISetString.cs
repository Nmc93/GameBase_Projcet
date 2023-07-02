using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 오브젝트 활성화 시 해당 텍스트

public class UISetString : MonoBehaviour
{
    [Header("[텍스트 컴포넌트]"), Tooltip("텍스트 컴포넌트")]
    [SerializeField] TextMeshProUGUI text;

    [Header("[해당 텍스트에 사용할 키값]"), Tooltip("해당 텍스트에 사용할 키값")]
    [SerializeField] string key;

    /// <summary> 출력할 문자열 </summary>
    private string textString;

    private void OnEnable()
    {
        //텍스트에 사용할 문자열이 준비되어있지 않을 경우
        if (string.IsNullOrEmpty(textString))
        {
            //테이블에서 받아옴
            textString = TableMgr.Get(key, out StringTableData data) ? data.Text : string.Empty;
        }

        //텍스트 변경
        text.text = textString;
    }
}
