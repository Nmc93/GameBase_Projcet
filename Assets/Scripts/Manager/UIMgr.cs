using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;
using UnityEngine.UI;

public class UIMgr : MgrBase
{
    public static UIMgr instance;

    /// <summary> 페이지 UI 캔버스 </summary>
    private Canvas pageCanvas;
    /// <summary> 팝업 UI 캔버스 </summary>
    private Canvas popupCanvas;

    /// <summary> 비활성화된 UI를 저장하는 풀 </summary>
    private Transform uiPool;

    /// <summary> UI 저장소 </summary>
    private Dictionary<eUIName, UIBase> dicUI = new Dictionary<eUIName, UIBase>(); 

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //캔버스 세팅
        CanvasSetting();

        //UI 풀, 데이터 세팅
        UIDataSetting();
    }

    /// <summary> 캔버스를 생성하고 세팅 </summary>
    private void CanvasSetting()
    {
        //캔버스 세팅
        GameObject canvasParent = new GameObject();
        canvasParent.transform.SetParent(transform);
        canvasParent.name = "UICanvas";

        //페이지 캔버스 세팅
        Canvas pageCanvas = new GameObject().AddComponent<Canvas>();
        pageCanvas.transform.SetParent(canvasParent.transform);
        pageCanvas.name = "PageCanvas";
        pageCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        pageCanvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | 
            AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
        //페이지 캔버스 스케일러 세팅
        CanvasScaler pageScale = pageCanvas.gameObject.AddComponent<CanvasScaler>();
        pageScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        pageScale.referenceResolution = new Vector2(Screen.width,Screen.height);
        pageScale.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        //페이지 캔버스 레이캐스터 세팅
        pageCanvas.gameObject.AddComponent<GraphicRaycaster>();
        this.pageCanvas = pageCanvas;

        //팝업 캔버스 세팅
        Canvas popupCanvas = new GameObject().AddComponent<Canvas>();
        popupCanvas.transform.SetParent(canvasParent.transform);
        popupCanvas.name = "PopupCanvas";
        popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        popupCanvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | 
            AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
        //팝업 캔버스 스케일러 세팅
        CanvasScaler popupScale = popupCanvas.gameObject.AddComponent<CanvasScaler>();
        popupScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        popupScale.referenceResolution = new Vector2(Screen.width, Screen.height);
        popupScale.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        //팝업 캔버스 레이캐스터 세팅
        popupCanvas.gameObject.AddComponent<GraphicRaycaster>();
        this.popupCanvas = popupCanvas;
    }

    /// <summary> UI의 오브젝트 풀과 UI 데이터를 세팅 </summary>
    private void UIDataSetting()
    {
        // 풀 오브젝트 생성
        GameObject uiPool = new GameObject();
        uiPool.transform.SetParent(transform);
        uiPool.name = "UIPool";
        this.uiPool = uiPool.transform;

        //로딩 화면 UI
        dicUI.Add(eUIName.UILoading, null);
    }

}
