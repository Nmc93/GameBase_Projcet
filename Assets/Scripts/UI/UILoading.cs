using GEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{
    public override eCanvas canvasType => eCanvas.Page;

    public override eUI uiType => eUI.UILoading;

    #region 인스펙터

    [Header("[로딩 스크롤 바]"), Tooltip("로딩 스크롤 바")]
    [SerializeField] private Scrollbar scrollbar;

    #endregion 인스펙터

    /// <summary> 로딩 진행 스크롤 </summary>
    public float ScrollProgress
    {
        set => scrollbar.size = value;
    }
}