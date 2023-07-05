using GEnum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{
    public override eCanvas canvasType => eCanvas.Page;

    public override eUI uiType => eUI.UILoading;

    /// <summary> 로딩 상태 </summary>
    public enum eLoadingState
    {
        /// <summary> UI 종료 시작 </summary>
        CloseCurScene,
        /// <summary> 씬 변경 시작 </summary>
        SceneChange,
        /// <summary> 씬 변경 대기 </summary>
        WaitChangeScene
    }

    #region 인스펙터

    [Header("------------------ UI -----------------")]
    [Header("[로딩 텍스트]"), Tooltip("[로딩 텍스트]")]
    [SerializeField] private TextMeshProUGUI txt;
    [Header("[로딩 스크롤 바]"), Tooltip("로딩 스크롤 바")]
    [SerializeField] private Scrollbar scrollbar;

    #endregion 인스펙터

    /// <summary> 로딩 진행 스크롤 </summary>
    public float ScrollProgress
    {
        set => scrollbar.size = value;
    }

    /// <summary> 텍스트 이펙트 </summary>
    private Coroutine textMove;
    /// <summary> UI의 현재 상태 </summary>
    private eLoadingState curState = eLoadingState.CloseCurScene;
    /// <summary> 텍스트에 사용할 문장 </summary>
    private string moveString;

    public override void DataSetting()
    {
        base.DataSetting();

        //데이터 최초 세팅
        ChangeState(eLoadingState.CloseCurScene);

        //코루틴 시작
        textMove = StartCoroutine(TextMove());
    }

    public override void Close()
    {
        base.Close();

        //코루틴 종료
        StopCoroutine(textMove);
    }

    /// <summary> 로딩 상태 변경 </summary>
    /// <param name="state"></param>
    public void ChangeState(eLoadingState state)
    {
        curState = state;

        switch (curState)
        {
            case eLoadingState.CloseCurScene:
                moveString = TableMgr.Get<StringTableData>("Loading1").Text;
                scrollbar.gameObject.SetActive(false);
                break;
            case eLoadingState.SceneChange:
                moveString = TableMgr.Get<StringTableData>("Loading2").Text;
                scrollbar.gameObject.SetActive(true);
                break;
            case eLoadingState.WaitChangeScene:
                moveString = TableMgr.Get<StringTableData>("LoadingCom").Text;
                break;
        }
    }

    /// <summary> 텍스트 이동 코루틴 </summary>
    IEnumerator TextMove()
    {
        string middleStr = string.Empty;
        while (true)
        {
            //텍스트 이펙트
            if(middleStr.Length > 6)
                middleStr = $"{middleStr}.";
            else
                middleStr = string.Empty;

            txt.text = string.Format("{0}{1}", moveString, middleStr);
            yield return new WaitForSeconds(0.5f);

            //스크롤바를 전부 채웠거나 현재 상태가 씬 변경 대기 타입일 경우
            if(scrollbar.size >= 1 || curState == eLoadingState.WaitChangeScene)
            {
                txt.text = string.Format("{0}{1}", moveString, ".");
                break;
            }
        }
    }
}