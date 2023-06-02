using GEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMgr : MgrBase
{
    public static MgrBase instance = null;

    [Header("[UI 캔버스]"), Tooltip("기본적인 UI를 그리는 캔버스")]
    [SerializeField] private Canvas sceneCanvas;
    [Header("[Page 캔버스]"), Tooltip("실제 게임을 유지하면서 화면 전체를 가리는 정보창을 그리는 캔버스")]
    [SerializeField] private Canvas pageCanvas;
    [Header("[Popup 캔버스]"), Tooltip("화면 전체를 가리지 않는 정보 팝업과 같은 창들을 그리는 캔버스")]
    [SerializeField] private Canvas popupCanvas;

    public override void Init()
    {
        //1.인스턴스 생성
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //UICanvas 생성
        GameObject sceneObj = new GameObject();
        sceneObj.transform.SetParent(transform);
        sceneObj.name = "SceneCanvas";
        sceneCanvas = sceneObj.AddComponent<Canvas>();
        sceneCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //PageCanvas 생성
        GameObject pageObj = new GameObject();
        pageObj.transform.SetParent(transform);
        pageObj.name = "PageCanvas";
        pageCanvas = pageObj.AddComponent<Canvas>();
        pageCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //PopupCanvas 생성
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
