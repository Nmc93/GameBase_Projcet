using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    private static RectTransform uiPool;

    /// <summary> UI 저장소 </summary>
    private static Dictionary<eUI, UIData> dicUI = new Dictionary<eUI, UIData>();
    /// <summary> 현재 열려있는 UI 리스트 </summary>
    private static List<eUI> openList = new List<eUI>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //캔버스 세팅
        CanvasSetting();

        //UI 풀, 데이터 세팅
        UIDataSetting();
    }

    #region Setting

    /// <summary> 캔버스를 생성하고 세팅 </summary>
    private void CanvasSetting()
    {
        //이벤트 시스템 세팅
        gameObject.AddComponent<EventSystem>();
        gameObject.AddComponent<StandaloneInputModule>();

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
        UIMgr.uiPool = uiPool;

        #endregion 풀 오브젝트

    }

    /// <summary> UI의 오브젝트 풀과 UI 데이터를 세팅 </summary>
    private void UIDataSetting()
    {
        //로딩 화면 UI
        dicUI.Add(eUI.UILoading, new UIData("UI/UILoading"));

        //로비 씬 메인 화면 UI
        dicUI.Add(eUI.UILobby, new UIData("UI/UILobby"));
    }

    #endregion Setting

    #region Open

    /// <summary> UI 오픈 </summary>
    /// <typeparam name="T">UIBase를 상속받은 UI의 메인 컴포넌트 타입</typeparam>
    /// <returns> UI 오픈에 성공하면 true </returns>
    public static bool OpenUI<T>() where T : UIBase
    {
        //eUI와 UI의 대표 컴포넌트의 이름은 동일해야함
        return OpenUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name));
    }

    /// <summary> UI 오픈 </summary>
    /// <param name="ui"> UI에 지정된 enum </param>
    /// <returns> UI 오픈에 성공하면 true </returns>
    public static bool OpenUI(eUI ui)
    {
        // 0. 이미 열려있는 경우 종료함
        if(openList.Contains(ui))
        {
            Debug.LogError($"{ui}는 이미 열려있는 UI 입니다.");
            return false;
        }

        //해당 UI의 데이터를 확인
        if(dicUI.TryGetValue(ui, out UIData data))
        {
            UIBase uiBase = data.uiClass;
            
            //1. 한번도 사용하지 않은 UI의 경우 로드해서 저장한 뒤 오픈을 시작함
            if (uiBase == null)
            {
                // 1-1-1. UI 로드에 성공했을 경우 데이터 저장
                if(AssetsMgr.LoadResourcesUIPrefab(data.path,out GameObject obj))
                {
                    data.uiClass = Instantiate(obj, uiPool).GetComponent<UIBase>();
                    uiBase = data.uiClass;
                }
                // 1-1-2. UI를 찾을 수 없을 경우 진행 종료
                else
                {
                    Debug.LogError($"UIOpenFailed : [{ui}]는 등록되지 않은 UI입니다.");
                    return false;
                }
            }

            //2. Page 타입의 UI를 오픈할 경우 Scene 캔버스를 비활성화 하고 다른 Page와 Popup타입의 UI를 종료함
            if (uiBase.canvasType == eCanvas.Page)
            {
                //2-1. 씬 캔버스 비활성화
                scene.SetActivate(false);

                //2-2. 현재 열려있는 UI를 체크
                for (int i = openList.Count - 1; i >= 0; -- i)
                {
                    //2-3. 같은 UI가 아닌 Scene 타입 UI를 빼고 전부 종료
                    UIBase temp = dicUI[openList[i]].uiClass;
                    if (temp.uiType != data.uiClass.uiType && temp.canvasType != eCanvas.Scene)
                    {
                        instance.CloseUI(temp.uiType, false);
                    }
                }
            }

            //3. UI를 캔버스에 올리고 UI를 활성화
            uiBase.transform.SetParent(GetCanvas(uiBase.canvasType).transform);
            openList.Add(ui);
            uiBase.Open();

            Debug.Log($"UIOpen : [{ui}], CanvasType : [{data.uiClass.canvasType}]");
            return true;
        }

        //실행 실패
        Debug.LogError($"UIOpenFailed : [{ui}]는 등록되지 않은 UI입니다.");
        return false;
    }

    #endregion Open

    #region Close

    /// <summary> 씬 변경으로 인해 현재 열려있는 모든 UI 종료 </summary>
    public static void SceneChangeAllUIClose()
    {
        // 1. 현재 열려있는 모든 UI를 체크
        for (int i = openList.Count - 1; i >= 0; --i)
        {
            // 2. 종료 대상 데이터 획득
            eUI type = openList[i];
            UIBase uIBase = dicUI[type].uiClass;

            // 3. 씬 변경시 종료하기로 한 UI인지 체크
            if (uIBase.IsSceneChangeClose)
            {
                // 4. UI 종료
                uIBase.DataClear();
                openList.Remove(type);
            }
        }
    }

    /// <summary> UI 종료 </summary>
    /// <typeparam name="T"> UIBase를 상속받은 UI의 메인 컴포넌트 타입 </typeparam>
    /// <returns> 종료에 성공하면 true </returns>
    public bool CloseUI<T>()
    {
        //eUI와 UI의 대표 컴포넌트의 이름은 동일해야함
        return CloseUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name));
    }

    /// <summary> UI 종료 </summary>
    /// <param name="ui"> 종료할 UI </param>
    /// <param name="isChainClose"> UI가 종료될때 타입에 따라 추가로 다른 UI를 종료하는 이벤트를 실행할지 여부 </param>
    /// <returns> 종료에 성공하면 true </returns>
    public bool CloseUI(eUI ui, bool isChainClose = true)
    {
        // 1. 오픈 리스트 체크
        if(!openList.Contains(ui))
        {
            return false;
        }

        // 2. 종료 대상 UI 정보 획득
        if(!dicUI.TryGetValue(ui,out UIData uiData))
        {
            Debug.LogError($"UICloseFailed : [{ui}]는 지정되지 않은 타입의 UI입니다.");
            return false;
        }

        // 3. 추가적 종료를 하고 해당 UI가 페이지 타입일 경우
        if (isChainClose && uiData.uiClass.canvasType == eCanvas.Page)
        {
            // 3-1.씬 캔버스를 활성화
            scene.SetActivate(true);

            // 3-2. 현재 열려있는 UI를 체크
            for (int i = openList.Count - 1; i >= 0; --i)
            {
                // 3-3. 같은 UI가 아니고 페이지일 경우 종료
                UIBase temp = dicUI[openList[i]].uiClass;
                if (temp.uiType != uiData.uiClass.uiType && temp.canvasType != eCanvas.Scene)
                {
                    instance.CloseUI(temp.uiType, false);
                }
            }
        }

        //대상 UI 종료 프로세스 시작
        uiData.uiClass.DataClear();
        openList.Remove(ui);
        return true;
    }

    //종료 후 Pool로 돌아감
    public void ReturnToUIPool(UIBase uiBase)
    {
        //UI 풀로 이동
        Debug.Log($"UIClose : [{uiBase.uiType}], CanvasType : [{uiBase.canvasType}]");
        uiBase.transform.SetParent(uiPool);
    }

    #endregion Close

    #region Get

    #region UI의 메인 컴포넌트 반환 (GetUI)

    /// <summary> UI 클래스를 받는 함수 </summary>
    /// <typeparam name="T"> 대상 UI에 있는 UIBase를 상속받은 메인 클래스 </typeparam>
    /// <returns> 검색 실패시 null 반환 </returns>
    public T GetUI<T>() where T : UIBase
    {
        return GetUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name)) as T;
    }

    /// <summary> UI 클래스를 받는 함수 </summary>
    /// <param name="ui"> 대상 UI에 할당된 eUI </param>
    /// <returns> 검색 실패시 null 반환 </returns>
    public UIBase GetUI(eUI ui)
    {
        //UI가 등록되어 있을 경우
        if (dicUI.TryGetValue(ui, out UIData data))
        {
            //UI의 로드했을 경우
            if (data.uiClass != null)
            {
                return data.uiClass;
            }
            //UI가 단 한번도 활성화 된적이 없을 경우
            else
            {
                Debug.LogError($"[{ui}]타입의 UI의 메인 컴포넌트를 찾을 수 없습니다.");
            }
        }
        //UI가 등록 누락이 되어 있을 경우
        else
        {
            Debug.LogError($"[{ui}]타입의 UI의 등록이 누락되어 있습니다. UIMgr.UIDataSetting() 를 참조하십시오");
        }

        return null;
    }

    /// <summary> UI 클래스를 받는 함수 </summary>
    /// <typeparam name="T">UIBase 를 상속받은 UI 클래스</typeparam>
    /// <param name="uiBase"> 검색 결과 반환 </param>
    /// <returns> 검색 성공시 true </returns>
    public bool GetUI<T>(out T uiBase) where T : UIBase
    {
        uiBase = GetUI((eUI)Enum.Parse(typeof(eUI), typeof(T).Name)) as T;
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
    public static Canvas GetCanvas(eCanvas uIType)
    {
        switch (uIType)
        {
            case eCanvas.Scene:
                return scene.canvas;
            case eCanvas.Page:
                return page.canvas;
            case eCanvas.Popup:
                return popup.canvas;
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
    public bool SetActivate(bool isActive)
    {
        canvas.enabled = isActive;
        rayCast.enabled = isActive;

        return isActive;
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