using GEnum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILoading : UIBase
{
    public override eCanvas canvasType => eCanvas.Page;

    public override eUI uiType => eUI.UILoading;


    #region 인스펙터

    [Header("------------------ UI -----------------")]
    [Header("[로딩 텍스트]"), Tooltip("[로딩 텍스트]")]
    [SerializeField] private TextMeshProUGUI txt;
    [Header("[로딩 스크롤 바]"), Tooltip("로딩 스크롤 바")]
    [SerializeField] private Scrollbar scrollbar;

    #endregion 인스펙터

    /// <summary> 텍스트 이펙트 </summary>
    private Coroutine textMove;
    /// <summary> UI의 현재 상태 </summary>
    private eLoadingState curState = eLoadingState.None;
    /// <summary> 텍스트에 사용할 문장 </summary>
    private string moveString;

    public override void DataSetting()
    {
        //데이터 초기화
        curState = eLoadingState.None;
        moveString = string.Empty;
        scrollbar.size = 0;
        //텍스트 이펙트가 활성화 상태라면 비활성화
        if (DOTween.IsTweening(txt))
        {
            txt.DOKill();
        }

        //코루틴 시작
        textMove = StartCoroutine(TextMove());

        //이벤트 등록(프로그레스바, 상태 변경, 클릭 이벤트)
        AddEvent();

        base.DataSetting();
    }

    public override void DataClear()
    {
        //이벤트 해제(프로그레스바, 상태 변경, 클릭 이벤트)
        RemoveEvent();

        //코루틴 종료
        if (textMove != null)
        {
            StopCoroutine(textMove);
            textMove = null;
        }

        base.DataClear();
    }

    #region 이벤트 등록, 해제

    /// <summary> 이벤트 등록 함수 </summary>
    private void AddEvent()
    {
        SceneMgr.instance.onGetchanProgress += SetProgress;
        SceneMgr.instance.onGetChanState += ChangeState;
        InputMgr.AddKeyEvent(eInputType.MoveNextScene, OnClickNextScene);
    }
    /// <summary> 이벤트 해제 함수 </summary>
    private void RemoveEvent()
    {
        //이벤트 해제(프로그레스바, 상태 변경, 클릭 이벤트)
        SceneMgr.instance.onGetchanProgress -= SetProgress;
        SceneMgr.instance.onGetChanState -= ChangeState;
        InputMgr.RemoveKeyEvent(eInputType.MoveNextScene, OnClickNextScene);
    }

    #endregion 이벤트 등록, 해제 

    #region 이벤트

    /// <summary> 로딩 상태 변경 </summary>
    /// <param name="state"></param>
    public void ChangeState(eLoadingState state)
    {
        curState = state;

        switch (curState)
        {
            // 현재 씬을 종료
            case eLoadingState.CloseCurScene:
                {
                    moveString = TableMgr.Get<StringTableData>("Loading1").Text;
                    scrollbar.gameObject.SetActive(false);
                }
                break;
            // 씬 변경
            case eLoadingState.SceneChange:
                {
                    moveString = TableMgr.Get<StringTableData>("Loading2").Text;
                    scrollbar.gameObject.SetActive(true);
                }
                break;
            // 변경작업을 완료하고 대기
            case eLoadingState.WaitChangeScene:
                {
                    //코루틴 종료
                    if (textMove != null)
                    {
                        StopCoroutine(textMove);
                        textMove = null;
                    }

                    //텍스트 세팅 및 깜빡이는 이펙트 적용
                    moveString = TableMgr.Get<StringTableData>("LoadingCom").Text;
                    txt.text = moveString;
                    txt.DOFade(0.0f, 1).SetLoops(-1,LoopType.Yoyo);
                }
                break;
            //UI 종료 페이즈
            case eLoadingState.None:
                {
                    //해당 UI 종료
                    UIMgr.instance.CloseUI(uiType);
                }
                break;
        }
    }

    /// <summary> 진행 바 조절 </summary>
    public void SetProgress(float value)
    {
        scrollbar.size = value;
    }

    /// <summary> 다음 씬으로 이동 </summary>
    public void OnClickNextScene()
    {
        SceneMgr.instance.MoveNextScene();
    }

    #endregion 이벤트

    #region 텍스트 이동 코루틴
    /// <summary> 텍스트 이동 코루틴 </summary>
    IEnumerator TextMove()
    {
        string middleStr = string.Empty;
        while (scrollbar.size < 1)
        {
            if (string.IsNullOrEmpty(moveString))
            {
                //텍스트 이펙트
                if (middleStr.Length > 6)
                    middleStr = $"{middleStr}.";
                else
                    middleStr = string.Empty;

                txt.text = string.Format("{0}{1}", moveString, middleStr);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion 텍스트 이동 코루틴
}