using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;
using UnityEngine.UI;

public class UIMgr : MgrBase
{
    public static UIMgr instance;

    /// <summary> �� UI ĵ���� </summary>
    private static CanvasData scene;
    /// <summary> ������ UI ĵ���� </summary>
    private static CanvasData page;
    /// <summary> �˾� UI ĵ���� </summary>
    private static CanvasData popup;

    /// <summary> UI ����� </summary>
    private static Dictionary<eUIName, UIData> dicUI = new Dictionary<eUIName, UIData>();

    /// <summary> ��Ȱ��ȭ�� UI�� �����ϴ� Ǯ </summary>
    private Transform uiPool;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //ĵ���� ����
        CanvasSetting();

        //UI Ǯ, ������ ����
        UIDataSetting();
    }

    /// <summary> ĵ������ �����ϰ� ���� </summary>
    private void CanvasSetting()
    {
        //ĵ���� ����
        GameObject canvasParent = new GameObject();
        canvasParent.transform.SetParent(transform);
        canvasParent.name = "UICanvas";

        #region �� ĵ����
        //�� ĵ���� ����
        Canvas sceneCanvas = new GameObject().AddComponent<Canvas>();
        sceneCanvas.transform.SetParent(canvasParent.transform);
        sceneCanvas.name = "PageCanvas";
        sceneCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        sceneCanvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 |
            AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
        //�� ĵ���� �����Ϸ� ����
        CanvasScaler sceneScale = sceneCanvas.gameObject.AddComponent<CanvasScaler>();
        sceneScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        sceneScale.referenceResolution = new Vector2(Screen.width, Screen.height);
        sceneScale.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        //ĵ���� ������ ����
        scene = new CanvasData(sceneCanvas, sceneScale, sceneCanvas.gameObject.AddComponent<GraphicRaycaster>());

        #endregion �� ĵ����

        #region ������ ĵ����
        //������ ĵ���� ����
        Canvas pageCanvas = new GameObject().AddComponent<Canvas>();
        pageCanvas.transform.SetParent(canvasParent.transform);
        pageCanvas.name = "PageCanvas";
        pageCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        pageCanvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | 
            AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
        //������ ĵ���� �����Ϸ� ����
        CanvasScaler pageScale = pageCanvas.gameObject.AddComponent<CanvasScaler>();
        pageScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        pageScale.referenceResolution = new Vector2(Screen.width,Screen.height);
        pageScale.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        //ĵ���� ������ ����
        page = new CanvasData(pageCanvas, pageScale, pageCanvas.gameObject.AddComponent<GraphicRaycaster>());
        #endregion ������ ĵ����

        #region �˾� ĵ����
        //�˾� ĵ���� ����
        Canvas popupCanvas = new GameObject().AddComponent<Canvas>();
        popupCanvas.transform.SetParent(canvasParent.transform);
        popupCanvas.name = "PopupCanvas";
        popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        popupCanvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | 
            AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
        //�˾� ĵ���� �����Ϸ� ����
        CanvasScaler popupScale = popupCanvas.gameObject.AddComponent<CanvasScaler>();
        popupScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        popupScale.referenceResolution = new Vector2(Screen.width, Screen.height);
        popupScale.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        //�˾� ĵ���� ����ĳ���� ����
        popup = new CanvasData(popupCanvas, popupScale, popupCanvas.gameObject.AddComponent<GraphicRaycaster>());
        #endregion �˾� ĵ����
    }

    /// <summary> UI�� ������Ʈ Ǯ�� UI �����͸� ���� </summary>
    private void UIDataSetting()
    {
        // Ǯ ������Ʈ ����
        GameObject uiPool = new GameObject();
        uiPool.transform.SetParent(transform);
        uiPool.name = "UIPool";
        this.uiPool = uiPool.transform;

        //�ε� ȭ�� UI
        dicUI.Add(eUIName.UILoading, new UIData("", typeof(UILoading)));
    }

    #region Open

    /// <summary> UI ���� </summary>
    public static bool OpenUI<T>() where T : UIBase
    {
        return OpenUI((eUIName)Enum.Parse(typeof(eUIName), typeof(T).Name));
    }

    /// <summary> UI ���� </summary>
    public static bool OpenUI(eUIName ui)
    {
        if(dicUI.TryGetValue(ui, out UIData data))
        {
            UIBase uiBase = data.uiClass;
            //�ε�� UI�� ���
            if (uiBase != null)
            {
                uiBase.transform.SetParent(GetCanvas(uiBase.uiType));
                uiBase.Open();
            }
            //���ݱ��� �ε������ ���� UI�� ���
            else
            {

            }
        }
        else
        {
            Debug.LogError($"{ui} : ��ϵ��� ���� UI�Դϴ�.");
        }

        return false;
    }

    #endregion Open

    #region Close

    public void CloseUI()
    {
    }

    #endregion Close

    #region Get

    /// <summary> UI Ŭ������ �޴� �Լ� </summary>
    public static UIBase GetUI<T>() where T : UIBase
    {
        return GetUI((eUIName)Enum.Parse(typeof(eUIName), typeof(T).Name));
    }

    /// <summary> UI Ŭ������ �޴� �Լ� </summary>
    public static UIBase GetUI(eUIName ui)
    {
        if (dicUI.TryGetValue(ui, out UIData data))
        {
            return data.uiClass;
        }

        return null;
    }

    /// <summary> Ÿ�Կ� �´� ĵ������ Transform�� ��ȯ </summary>
    private static Transform GetCanvas(eUIType uIType)
    {
        switch (uIType)
        {
            case eUIType.Scene:
                return scene.canvas.transform;
            case eUIType.Page:
                return page.canvas.transform;
            case eUIType.Popup:
                return popup.canvas.transform;
        }

        return null;
    }

    #endregion Get
}

#region ĵ���� ����
/// <summary> ĵ���� ������ </summary>
public class CanvasData
{
    public CanvasData(Canvas canvas, CanvasScaler scale, GraphicRaycaster rayCast)
    {
        this.canvas = canvas;
        this.scale = scale;
        this.rayCast = rayCast;
    }

    /// <summary> ĵ���� </summary>
    public Canvas canvas;
    /// <summary> �����Ϸ� </summary>
    public CanvasScaler scale;
    /// <summary> �׷��� ����ĳ��Ʈ </summary>
    public GraphicRaycaster rayCast;

    /// <summary> ĵ���� ������Ʈ Ȱ��ȭ ���� </summary>
    public void SetActivate(bool isActive)
    {
        canvas.enabled = isActive;
        rayCast.enabled = isActive;
    }
}
#endregion ĵ���� ����

public class UIData
{
    public UIData(string path, Type classType)
    {
        this.path = path;
        this.classType = classType;
    }

    /// <summary> ������ �н� </summary>
    public string path;
    /// <summary> Ŭ���� Ÿ�� </summary>
    public Type classType;
    /// <summary> UIŬ���� </summary>
    public UIBase uiClass;
}