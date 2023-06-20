using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;
using UnityEngine.UI;

public class UIMgr : MgrBase
{
    public static UIMgr instance;

    /// <summary> 씬 UI 캔버스 </summary>
    private CanvasData scene;
    /// <summary> 페이지 UI 캔버스 </summary>
    private CanvasData page;
    /// <summary> 팝업 UI 캔버스 </summary>
    private CanvasData popup;

    /// <summary> UI 저장소 </summary>
    private static Dictionary<eUIName, UIBase> dicUI = new Dictionary<eUIName, UIBase>();

    /// <summary> 비활성화된 UI를 저장하는 풀 </summary>
    private Transform uiPool;

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

        #region 씬 캔버스
        //씬 캔버스 세팅
        Canvas sceneCanvas = new GameObject().AddComponent<Canvas>();
        sceneCanvas.transform.SetParent(canvasParent.transform);
        sceneCanvas.name = "PageCanvas";
        sceneCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        sceneCanvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 |
            AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
        //씬 캔버스 스케일러 세팅
        CanvasScaler sceneScale = sceneCanvas.gameObject.AddComponent<CanvasScaler>();
        sceneScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        sceneScale.referenceResolution = new Vector2(Screen.width, Screen.height);
        sceneScale.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        //캔버스 데이터 세팅
        scene = new CanvasData(sceneCanvas, sceneScale, sceneCanvas.gameObject.AddComponent<GraphicRaycaster>());

        #endregion 씬 캔버스

        #region 페이지 캔버스
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
        //캔버스 데이터 세팅
        page = new CanvasData(pageCanvas, pageScale, pageCanvas.gameObject.AddComponent<GraphicRaycaster>());
        #endregion 페이지 캔버스

        #region 팝업 캔버스
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
        popup = new CanvasData(popupCanvas, popupScale, popupCanvas.gameObject.AddComponent<GraphicRaycaster>());
        #endregion 팝업 캔버스
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

    /// <summary> UI 오픈 </summary>
    public static bool OpenUI<T>()
    {
        Type uiType = typeof(T);
        eUIName name = (eUIName)Enum.Parse(typeof(eUIName),uiType.Name);
        return false;
    }

    public void CloseUI()
    {
    }

}

#region 캔버스 정보
/// <summary> 캔버스 데이터 </summary>
public class CanvasData
{
    public CanvasData(Canvas canvas, CanvasScaler scale, GraphicRaycaster rayCast)
    {
        this.canvas = canvas;
        this.scale = scale;
        this.rayCast = rayCast;
    }

    /// <summary> 캔버스 </summary>
    public Canvas canvas;
    /// <summary> 스케일러 </summary>
    public CanvasScaler scale;
    /// <summary> 그래픽 레이캐스트 </summary>
    public GraphicRaycaster rayCast;

    /// <summary> 캔버스 컴포넌트 활성화 변경 </summary>
    public void SetActivate(bool isActive)
    {
        canvas.enabled = isActive;
        rayCast.enabled = isActive;
    }
}
#endregion 캔버스 정보

public class UIData
{
    public UIData(string path, string name)
    {
        this.path = path;
        this.name = name;
    }

    /// <summary> 프리팹 패스 </summary>
    public string path;
    /// <summary> 클래스 이름 </summary>
    public string name;
    /// <summary> UI클래스 </summary>
    public UIBase uiClass;
}