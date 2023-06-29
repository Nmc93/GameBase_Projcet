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

    /// <summary> 씬 UI 캔버스 </summary>
    private static CanvasData scene;
    /// <summary> 페이지 UI 캔버스 </summary>
    private static CanvasData page;
    /// <summary> 팝업 UI 캔버스 </summary>
    private static CanvasData popup;

    /// <summary> 비활성화된 UI를 저장하는 풀 </summary>
    private RectTransform uiPool;

    /// <summary> UI 저장소 </summary>
    private static Dictionary<eUI, UIData> dicUI = new Dictionary<eUI, UIData>();

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
        sceneCanvas.name = "sceneCanvas";
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

        #region 풀 오브젝트

        // 풀 오브젝트 생성
        RectTransform uiPool = new GameObject().AddComponent<RectTransform>();
        uiPool.transform.SetParent(canvasParent.transform);
        uiPool.sizeDelta = new Vector2(Screen.width, Screen.height);
        uiPool.position = new Vector2(Screen.width / 2, Screen.height / 2);
        uiPool.gameObject.SetActive(false);
        uiPool.name = "UIPool";
        this.uiPool = uiPool;

        #endregion 풀 오브젝트

    }

    /// <summary> UI의 오브젝트 풀과 UI 데이터를 세팅 </summary>
    private void UIDataSetting()
    {
        //로딩 화면 UI
        dicUI.Add(eUI.UILoading, new UIData("UI/UILoading"));
    }

    #region Open

    /// <summary> UI 오픈 </summary>
    /// <typeparam name="T">UIBase를 상속받은 UI의 메인 컴포넌트 타입</typeparam>
    /// <returns> UI 오픈에 성공하면 true </returns>
    public bool OpenUI<T>() where T : UIBase
    {
        //eUI와 UI의 대표 컴포넌트의 이름은 동일해야함
        return OpenUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name));
    }

    /// <summary> UI 오픈 </summary>
    /// <param name="ui"> UI에 지정된 enum </param>
    /// <returns> UI 오픈에 성공하면 true </returns>
    public bool OpenUI(eUI ui)
    {
        //해당 UI의 데이터를 확인
        if(dicUI.TryGetValue(ui, out UIData data))
        {
            UIBase uiBase = data.uiClass;
            
            //해당 UI를 처음 사용하는 경우
            if (uiBase == null)
            {
                //UI 로드에 성공했을 경우
                if(AssetsMgr.LoadResourcesUIPrefab(data.path,out GameObject obj))
                {
                    //저장
                    data.uiClass = Instantiate(obj, uiPool).GetComponent<UIBase>();
                    uiBase = data.uiClass;
                }
                //UI를 찾을 수 없을 경우
                else
                {
                    //종료
                    Debug.LogError($"UIOpenFailed : [{ui}]는 등록되지 않은 UI입니다.");
                    return false;
                }
            }

            //UI의 캔버스 타입이 페이지일 경우
            if(data.uiClass.canvasType == eCanvas.Popup)
            {
                //씬 캔버스 비활성화
                scene.SetActivate(false);

                //페이지 모두 종료
                foreach (var item in dicUI.Values)
                {
                    //열려있고 캔버스 타입이 페이지고 현재 열리고 있는 페이지가 아닐 경우
                    if (item.uiClass.IsOpen && 
                        item.uiClass.canvasType == eCanvas.Page &&
                        item.uiClass.uiType != data.uiClass.uiType)
                    {
                        item.uiClass.Close();
                    }
                }
            }

            //UI를 캔버스에 올리고 UI를 활성화
            Debug.Log($"UIOpen : [{ui}]");
            uiBase.transform.SetParent(GetCanvas(uiBase.canvasType));
            uiBase.Open();
            return true;
        }

        //실행 실패
        Debug.LogError($"UIOpenFailed : [{ui}]는 등록되지 않은 UI입니다.");
        return false;
    }

    #endregion Open

    #region Close

    /// <summary> UI 종료 </summary>
    /// <typeparam name="T"> UIBase를 상속받은 UI의 메인 컴포넌트 타입 </typeparam>
    /// <returns> 종료에 성공하면 true </returns>
    public bool CloseUI<T>()
    {
        //eUI와 UI의 대표 컴포넌트의 이름은 동일해야함
        return CloseUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name));
    }

    public bool CloseUI(eUI ui)
    {
        //존재하는 UI인지 체크
        if(dicUI.TryGetValue(ui,out UIData uiData))
        {
            //종료가 가능한 것만 종료(호출되서 오픈중일 경우)
            if (uiData.uiClass != null && uiData.uiClass.IsOpen)
            {
                //현재 종료되는 UI의 캔버스 타입이 페이지일 경우
                if (uiData.uiClass.canvasType == eCanvas.Page)
                {
                    foreach (var item in dicUI.Values)
                    {
                        //열려있는 팝업 전부 종료
                        if (item.uiClass.IsOpen && item.uiClass.canvasType == eCanvas.Popup)
                        {
                            item.uiClass.Close();
                        }
                    }
                }

                //대상 UI 종료 프로세스 시작
                uiData.uiClass.Close();
                return true;
            }
            //호출된적 없거나 오픈중이 아닐 경우
            else
            {
                Debug.LogError($"UICloseFailed : [{ui}]는 활성화 상태가 아닙니다.");
            }
        }
        //지정되어있지 않은 UI일 경우
        else
        {
            Debug.LogError($"UICloseFailed : [{ui}]는 지정되지 않은 타입의 UI입니다.");
        }

        return false;
    }

    //종료 후 Pool로 돌아감
    public void ReturnToUIPool(UIBase uiBase)
    {
        //UI 풀로 이동
        Debug.Log($"UIClose : [{uiBase.uiType}]");
        uiBase.transform.SetParent(uiPool);
    }

    #endregion Close

    #region Get

    #region UI의 메인 컴포넌트 반환 (GetUI)

    /// <summary> UI 클래스를 받는 함수 </summary>
    /// <typeparam name="T"> 대상 UI에 있는 UIBase를 상속받은 메인 클래스 </typeparam>
    /// <returns> 검색 실패시 null 반환 </returns>
    public UIBase GetUI<T>() where T : UIBase
    {
        return GetUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name));
    }

    /// <summary> UI 클래스를 받는 함수 </summary>
    /// <param name="ui"> 대상 UI에 할당된 eUI </param>
    /// <returns> 검색 실패시 null 반환 </returns>
    public UIBase GetUI(eUI ui)
    {
        if (!dicUI.TryGetValue(ui, out UIData data))
        {
            Debug.LogError($"[{ui}]타입의 UI를 찾을 수 없습니다.");
            return data.uiClass;
        }

        return null;
    }

    /// <summary> UI 클래스를 받는 함수 </summary>
    /// <typeparam name="T">UIBase 를 상속받은 UI 클래스</typeparam>
    /// <param name="uiBase"> 검색 결과 반환 </param>
    /// <returns> 검색 성공시 true </returns>
    public bool GetUI<T>(out UIBase uiBase) where T : UIBase
    {
        uiBase = GetUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name));
        return uiBase != null;
    }

    /// <summary> UI 클래스를 받는 함수 </summary>
    /// <param name="ui"> 대상 UI에 할당된 eUI </param>
    /// <param name="uiBase"> 검색 결과 반환 </param>
    /// <returns> 검색 성공시 true </returns>
    public bool GetUI(eUI ui, out UIBase uiBase)
    {
        if (!dicUI.TryGetValue(ui, out UIData data))
        {
            Debug.LogError($"[{ui}]타입의 UI를 찾을 수 없습니다.");
        }

        uiBase = data.uiClass;
        return uiBase != null;
    }

    #endregion UI의 메인 컴포넌트 반환 (GetUI)

    /// <summary> 타입에 맞는 캔버스의 Transform을 반환 </summary>
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

#region UI 정보
/// <summary> UI의 정보 </summary>
public class UIData
{
    public UIData(string path)
    {
        this.path = path;
    }

    /// <summary> 프리팹 패스 </summary>
    public string path;
    /// <summary> UI클래스 </summary>
    public UIBase uiClass;
}
#endregion UI 정보