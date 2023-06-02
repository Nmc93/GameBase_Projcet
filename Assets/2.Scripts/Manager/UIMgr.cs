using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

public class UIMgr : MgrBase
{
    public static UIMgr instance = null;

    /// <summary>UI를 저장하는 딕셔너리</summary>
    private Dictionary<eUIPrefab, GameObject> uiBaseDic = new Dictionary<eUIPrefab, GameObject>();

    private Dictionary<eUIPrefab, UIPrefabData> uiDic = new Dictionary<eUIPrefab, UIPrefabData>();

    [SerializeField]
    private List<eUIPrefab> activePrefabList = new List<eUIPrefab>();

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

        //UI 저장소 체크
        if (uiDic == null)
        {
            uiDic = new Dictionary<eUIPrefab, UIPrefabData>();
        }

        #region Scene UI
        #endregion Scene UI

        #region Page UI
        #endregion Page UI

        #region Popup UI

        //옵션 팝업
        uiDic.Add(eUIPrefab.OptionPopup, new UIPrefabData(eUIType.Popup, "Prefab/UI/OptionPopup"));

        #endregion Popup UI
    }

    #region 프리팹 활성화 비활성화

    /// <summary> UI를 활성화함, 풀에 있다면 풀에서 가져오고 아니면 생성함 </summary>
    /// <param name="uIPrefab"> UI이름 </param>
    /// <param name="canvas"> UI를 출력해야할 캔버스 </param>
    /// <param name="uiPosition"> UI의 위치 </param>
    public void OpenUI(eUIPrefab uIPrefab, GameObject canvas, Vector3 uiPosition)
    {
        GameObject uiObjcet = null;

        //풀에 프리팹이 있을 경우
        if (PoolMgr.instance.GetUIPool(uIPrefab, out uiObjcet))
        {
            uiObjcet.transform.SetParent(canvas.transform);
            uiObjcet.transform.position = uiPosition;
            uiObjcet.transform.localScale = new Vector3(1, 1, 1);

            //테스트
            activePrefabList.Add(uIPrefab);
            uiBaseDic.Add(uIPrefab, uiObjcet);

            //매니저 관리에 들어왔으니 활성화
            uiBaseDic[uIPrefab].SetActive(true);
        }
        //풀에 프리팹이 없을 경우
        else
        {
            //프리팹 생성
            uiObjcet = CreateUI(uIPrefab);
            uiObjcet.transform.SetParent(canvas.transform);
            uiObjcet.transform.position = uiPosition;
            uiObjcet.transform.localScale = new Vector3(1, 1, 1);

            //테스트
            activePrefabList.Add(uIPrefab);
            uiBaseDic.Add(uIPrefab, uiObjcet);

            //매니저 관리에 들어왔으니 활성화
            uiBaseDic[uIPrefab].SetActive(true);
        }
    }

    /// <summary> 매니저에서 관리중인 프리팹을 풀에 넣고 종료함 </summary>
    public void CloseUI(eUIPrefab uIPrefab)
    {
        GameObject uiObjcet = null;

        //프리팹이 현재 활성화 상태인지 확인
        if(uiBaseDic.TryGetValue(uIPrefab,out uiObjcet))
        {
            //UI매니저에서 관리중이던 프리팹을 해제
            uiBaseDic.Remove(uIPrefab);

            //테스트용
            for(int i = 0; i < activePrefabList.Count; ++i)
            {
                if(activePrefabList[i] == uIPrefab)
                {
                    activePrefabList.RemoveAt(i);
                    break;
                }
            }

            //풀에 프리팹을 넣음
            PoolMgr.instance.SetUIPool(uIPrefab, uiObjcet);
        }
    }
    #endregion 프리팹 활성화 비활성화

    #region 프리팹 생성
    /// <summary> 프리팹 생성 </summary>
    /// <returns> 프리팹이 없다면 Null 반환 </returns>
    public GameObject CreateUI(eUIPrefab uIPrefab)
    {
        GameObject uiObjcet = null;

        if (uiDic.TryGetValue(uIPrefab,out UIPrefabData data))
        {
            uiObjcet = Instantiate(Resources.Load(data.path)) as GameObject;
        }
        else
        {
            Debug.LogError($"{uIPrefab}에 해당하는 프리팹을 찾을 수 없습니다.");
        }

        return uiObjcet;
    }

    #endregion 프리팹 생성
}

[Serializable]
public class UIPrefabData
{
    public UIPrefabData(eUIType uIType, string path, UIBase uiClass = null)
    {
        this.uIType = uIType;
        this.path = path;
        this.uiClass = uiClass;
    }

    /// <summary> UI 종류 </summary>
    public eUIType uIType = eUIType.None;
    /// <summary> 프리팹 경로 </summary>
    public string path = null;
    /// <summary> UI의 메인 클래스 </summary>
    public UIBase uiClass = null;
}
