using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

public class UIMgr : MgrBase
{
    public static UIMgr instance;

    /// <summary> ������ UI ĵ���� </summary>
    private Canvas pageCanvas;
    /// <summary> �˾� UI ĵ���� </summary>
    private Canvas popupCanvas;

    /// <summary> ��Ȱ��ȭ�� UI�� �����ϴ� Ǯ </summary>
    private Transform uiPool;

    /// <summary> UI ����� </summary>
    private Dictionary<eUIName, UIBase> dicUI = new Dictionary<eUIName, UIBase>(); 

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

        //������ ĵ���� ����
        Canvas pageCanvas = new GameObject().AddComponent<Canvas>();
        pageCanvas.transform.SetParent(canvasParent.transform);
        pageCanvas.name = "PageCanvas";
        this.pageCanvas = pageCanvas;

        //������ ĵ���� ����
        Canvas popupCanvas = new GameObject().AddComponent<Canvas>();
        popupCanvas.transform.SetParent(canvasParent.transform);
        popupCanvas.name = "PopupCanvas";
        this.popupCanvas = popupCanvas;
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
        dicUI.Add(eUIName.UILoading, null);
    }

}
