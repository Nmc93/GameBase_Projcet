using GEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMgr : MgrBase
{
    public static MgrBase instance = null;

    [Header("[UI ĵ����]"), Tooltip("�⺻���� UI�� �׸��� ĵ����")]
    [SerializeField] private Canvas sceneCanvas;
    [Header("[Page ĵ����]"), Tooltip("���� ������ �����ϸ鼭 ȭ�� ��ü�� ������ ����â�� �׸��� ĵ����")]
    [SerializeField] private Canvas pageCanvas;
    [Header("[Popup ĵ����]"), Tooltip("ȭ�� ��ü�� ������ �ʴ� ���� �˾��� ���� â���� �׸��� ĵ����")]
    [SerializeField] private Canvas popupCanvas;

    public override void Init()
    {
        //1.�ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //UICanvas ����
        GameObject sceneObj = new GameObject();
        sceneObj.transform.SetParent(transform);
        sceneObj.name = "SceneCanvas";
        sceneCanvas = sceneObj.AddComponent<Canvas>();
        sceneCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //PageCanvas ����
        GameObject pageObj = new GameObject();
        pageObj.transform.SetParent(transform);
        pageObj.name = "PageCanvas";
        pageCanvas = pageObj.AddComponent<Canvas>();
        pageCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //PopupCanvas ����
        GameObject popupObj = new GameObject();
        popupObj.transform.SetParent(transform);
        popupObj.name = "PopupCanvas";
        popupCanvas = popupObj.AddComponent<Canvas>();
        popupCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    public override void Refresh()
    {
        base.Refresh();
    }
}
