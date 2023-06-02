using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Awake단에서 매니저를 생성하고 등록함
// 2. 지정된 순서대로 매니저를 초기화 시킴(순서에 의한 문제 방지)
// 

public class GameManager : MonoBehaviour
{
    public enum eMgr
    {
        ObserverManager = 0,
        OptionManager,
        CanvasMgr,
        SceneManager,
        UIManager,
        PoolManager,
    }

    /// <summary> 게임 매니저 인스턴스 </summary>
    public static GameManager instance = null;

    /// <summary> 매니저 목록 </summary>
    private Dictionary<eMgr, MgrBase> dic = null;

    #region 유니티 라이프 사이클

    //게임 시작
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //인스턴스 생성
        instance = this;
        DontDestroyOnLoad(gameObject);

        //매니저 저장용 딕셔너리
        dic = new Dictionary<eMgr, MgrBase>();

        //순서대로 매니저 초기화함
        //옵저버매니저
        GameObject observObject = new GameObject();
        Add(eMgr.ObserverManager, observObject.AddComponent<ObserverMgr>());

        //옵션 매니저
        GameObject optionObject = new GameObject();
        Add(eMgr.OptionManager, optionObject.AddComponent<OptionMgr>());

        //캔버스 매니저 생성
        GameObject CanvasManager = new GameObject();
        Add(eMgr.CanvasMgr, CanvasManager.AddComponent<CanvasMgr>());

        //씬 매니저 생성
        GameObject sceneObject = new GameObject();
        Add(eMgr.SceneManager, sceneObject.AddComponent<SceneMgr>());

        //UI매니저 생성
        GameObject uiManager = new GameObject();
        Add(eMgr.UIManager, uiManager.AddComponent<UIMgr>());

        //pool매니저 생성
        GameObject poolManager = new GameObject();
        Add(eMgr.PoolManager, poolManager.AddComponent<PoolMgr>());

    }

    #endregion 유니티 라이프 사이클

    #region 매니저 딕셔너리

    /// <summary>매니저 추가, 초기화 함수</summary>
    /// /// <param name="managerName"> [key] 매니저 이름 </param>
    /// /// <param name="manager">[Value] 매니저 클래스</param>
    public void Add(eMgr managerName, MgrBase manager)
    {
        if(dic.ContainsKey(managerName))
        {
            Debug.LogError(managerName + ": 이미 있는 매니저입니다.");
            return;
        }

        //이름변경, 부모지정, 매니저 목록에 등록, 초기화
        manager.gameObject.name = managerName.ToString();
        manager.transform.SetParent(transform);
        dic.Add(managerName, manager);
        dic[managerName].Init();
        Debug.Log($"Add : {managerName}");
    }

    #endregion 매니저 딕셔너리
}