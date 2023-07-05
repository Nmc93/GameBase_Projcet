using GEnum;
using System.Collections;
using System.Collections.Generic;
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
    }

    #region 인스펙터

    [Header("------------------ UI -----------------")]
    [Header("[로딩 스크롤 바]"), Tooltip("로딩 스크롤 바")]
    [SerializeField] private Scrollbar scrollbar;

    #endregion 인스펙터

    /// <summary> 로딩 진행 스크롤 </summary>
    public float ScrollProgress
    {
        set => scrollbar.size = value;
    }

    /// <summary> 로딩 상태 변경 </summary>
    /// <param name="state"></param>
    public void ChangeState(eLoadingState state)
    {
        switch(state)
        {
            case eLoadingState.CloseCurScene:
                break;
            case eLoadingState.SceneChange:
                break;
        }
    }
}