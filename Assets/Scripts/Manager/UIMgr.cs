using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using GEnum;

public class UIMgr : MgrBase
{
    public static UIMgr instance;

    /// <summary> �� UI ĵ���� </summary>
    private static CanvasData scene;
    /// <summary> ������ UI ĵ���� </summary>
    private static CanvasData page;
    /// <summary> �˾� UI ĵ���� </summary>
    private static CanvasData popup;

    /// <summary> ��Ȱ��ȭ�� UI�� �����ϴ� Ǯ </summary>
    private RectTransform uiPool;

    /// <summary> UI ����� </summary>
    private static Dictionary<eUI, UIData> dicUI = new Dictionary<eUI, UIData>();

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
        sceneCanvas.name = "sceneCanvas";
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

        #region Ǯ ������Ʈ

        // Ǯ ������Ʈ ����
        RectTransform uiPool = new GameObject().AddComponent<RectTransform>();
        uiPool.transform.SetParent(canvasParent.transform);
        uiPool.sizeDelta = new Vector2(Screen.width, Screen.height);
        uiPool.position = new Vector2(Screen.width / 2, Screen.height / 2);
        uiPool.gameObject.SetActive(false);
        uiPool.name = "UIPool";
        this.uiPool = uiPool;

        #endregion Ǯ ������Ʈ

    }

    /// <summary> UI�� ������Ʈ Ǯ�� UI �����͸� ���� </summary>
    private void UIDataSetting()
    {
        //�ε� ȭ�� UI
        dicUI.Add(eUI.UILoading, new UIData("UI/UILoading"));
    }

    #region Open

    /// <summary> UI ���� </summary>
    /// <typeparam name="T">UIBase�� ��ӹ��� UI�� ���� ������Ʈ Ÿ��</typeparam>
    /// <returns> UI ���¿� �����ϸ� true </returns>
    public bool OpenUI<T>() where T : UIBase
    {
        //eUI�� UI�� ��ǥ ������Ʈ�� �̸��� �����ؾ���
        return OpenUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name));
    }

    /// <summary> UI ���� </summary>
    /// <param name="ui"> UI�� ������ enum </param>
    /// <returns> UI ���¿� �����ϸ� true </returns>
    public bool OpenUI(eUI ui)
    {
        //�ش� UI�� �����͸� Ȯ��
        if(dicUI.TryGetValue(ui, out UIData data))
        {
            UIBase uiBase = data.uiClass;
            
            //�ش� UI�� ó�� ����ϴ� ���
            if (uiBase == null)
            {
                //UI �ε忡 �������� ���
                if(AssetsMgr.LoadResourcesUIPrefab(data.path,out GameObject obj))
                {
                    //����
                    data.uiClass = Instantiate(obj, uiPool).GetComponent<UIBase>();
                    uiBase = data.uiClass;
                }
                //UI�� ã�� �� ���� ���
                else
                {
                    //����
                    Debug.LogError($"UIOpenFailed : [{ui}]�� ��ϵ��� ���� UI�Դϴ�.");
                    return false;
                }
            }

            //UI�� ĵ���� Ÿ���� �������� ���
            if(data.uiClass.canvasType == eCanvas.Popup)
            {
                //�� ĵ���� ��Ȱ��ȭ
                scene.SetActivate(false);

                //������ ��� ����
                foreach (var item in dicUI.Values)
                {
                    //�����ְ� ĵ���� Ÿ���� �������� ���� ������ �ִ� �������� �ƴ� ���
                    if (item.uiClass.IsOpen && 
                        item.uiClass.canvasType == eCanvas.Page &&
                        item.uiClass.uiType != data.uiClass.uiType)
                    {
                        item.uiClass.Close();
                    }
                }
            }

            //UI�� ĵ������ �ø��� UI�� Ȱ��ȭ
            Debug.Log($"UIOpen : [{ui}]");
            uiBase.transform.SetParent(GetCanvas(uiBase.canvasType));
            uiBase.Open();
            return true;
        }

        //���� ����
        Debug.LogError($"UIOpenFailed : [{ui}]�� ��ϵ��� ���� UI�Դϴ�.");
        return false;
    }

    #endregion Open

    #region Close

    /// <summary> UI ���� </summary>
    /// <typeparam name="T"> UIBase�� ��ӹ��� UI�� ���� ������Ʈ Ÿ�� </typeparam>
    /// <returns> ���ῡ �����ϸ� true </returns>
    public bool CloseUI<T>()
    {
        //eUI�� UI�� ��ǥ ������Ʈ�� �̸��� �����ؾ���
        return CloseUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name));
    }

    public bool CloseUI(eUI ui)
    {
        //�����ϴ� UI���� üũ
        if(dicUI.TryGetValue(ui,out UIData uiData))
        {
            //���ᰡ ������ �͸� ����(ȣ��Ǽ� �������� ���)
            if (uiData.uiClass != null && uiData.uiClass.IsOpen)
            {
                //���� ����Ǵ� UI�� ĵ���� Ÿ���� �������� ���
                if (uiData.uiClass.canvasType == eCanvas.Page)
                {
                    foreach (var item in dicUI.Values)
                    {
                        //�����ִ� �˾� ���� ����
                        if (item.uiClass.IsOpen && item.uiClass.canvasType == eCanvas.Popup)
                        {
                            item.uiClass.Close();
                        }
                    }
                }

                //��� UI ���� ���μ��� ����
                uiData.uiClass.Close();
                return true;
            }
            //ȣ����� ���ų� �������� �ƴ� ���
            else
            {
                Debug.LogError($"UICloseFailed : [{ui}]�� Ȱ��ȭ ���°� �ƴմϴ�.");
            }
        }
        //�����Ǿ����� ���� UI�� ���
        else
        {
            Debug.LogError($"UICloseFailed : [{ui}]�� �������� ���� Ÿ���� UI�Դϴ�.");
        }

        return false;
    }

    //���� �� Pool�� ���ư�
    public void ReturnToUIPool(UIBase uiBase)
    {
        //UI Ǯ�� �̵�
        Debug.Log($"UIClose : [{uiBase.uiType}]");
        uiBase.transform.SetParent(uiPool);
    }

    #endregion Close

    #region Get

    #region UI�� ���� ������Ʈ ��ȯ (GetUI)

    /// <summary> UI Ŭ������ �޴� �Լ� </summary>
    /// <typeparam name="T"> ��� UI�� �ִ� UIBase�� ��ӹ��� ���� Ŭ���� </typeparam>
    /// <returns> �˻� ���н� null ��ȯ </returns>
    public UIBase GetUI<T>() where T : UIBase
    {
        return GetUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name));
    }

    /// <summary> UI Ŭ������ �޴� �Լ� </summary>
    /// <param name="ui"> ��� UI�� �Ҵ�� eUI </param>
    /// <returns> �˻� ���н� null ��ȯ </returns>
    public UIBase GetUI(eUI ui)
    {
        if (!dicUI.TryGetValue(ui, out UIData data))
        {
            Debug.LogError($"[{ui}]Ÿ���� UI�� ã�� �� �����ϴ�.");
            return data.uiClass;
        }

        return null;
    }

    /// <summary> UI Ŭ������ �޴� �Լ� </summary>
    /// <typeparam name="T">UIBase �� ��ӹ��� UI Ŭ����</typeparam>
    /// <param name="uiBase"> �˻� ��� ��ȯ </param>
    /// <returns> �˻� ������ true </returns>
    public bool GetUI<T>(out UIBase uiBase) where T : UIBase
    {
        uiBase = GetUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name));
        return uiBase != null;
    }

    /// <summary> UI Ŭ������ �޴� �Լ� </summary>
    /// <param name="ui"> ��� UI�� �Ҵ�� eUI </param>
    /// <param name="uiBase"> �˻� ��� ��ȯ </param>
    /// <returns> �˻� ������ true </returns>
    public bool GetUI(eUI ui, out UIBase uiBase)
    {
        if (!dicUI.TryGetValue(ui, out UIData data))
        {
            Debug.LogError($"[{ui}]Ÿ���� UI�� ã�� �� �����ϴ�.");
        }

        uiBase = data.uiClass;
        return uiBase != null;
    }

    #endregion UI�� ���� ������Ʈ ��ȯ (GetUI)

    /// <summary> Ÿ�Կ� �´� ĵ������ Transform�� ��ȯ </summary>
    private Transform GetCanvas(eCanvas uIType)
    {
        switch (uIType)
        {
            case eCanvas.Scene:
                return scene.canvas.transform;
            case eCanvas.Page:
                return page.canvas.transform;
            case eCanvas.Popup:
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

#region UI ����
/// <summary> UI�� ���� </summary>
public class UIData
{
    public UIData(string path)
    {
        this.path = path;
    }

    /// <summary> ������ �н� </summary>
    public string path;
    /// <summary> UIŬ���� </summary>
    public UIBase uiClass;
}
#endregion UI ����