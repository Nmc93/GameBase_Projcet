using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

// 1. Awake단에서 매니저를 생성하고 등록함
// 2. 지정된 순서대로 매니저를 초기화 시킴(순서에 의한 문제 방지)

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /// <summary> 매니저 클래스 저장 </summary>
    public static Dictionary<eMgr, MgrBase> mgrDic = new Dictionary<eMgr, MgrBase>();

    /// <summary> 시작시 최초 매니저 세팅 </summary>
    private void Awake()
    {
        //이미 있다면 현재 오브젝트 파괴
        if(instance != null)
            Destroy(this);

        //파괴불가 오브젝트 지정
        DontDestroyOnLoad(gameObject);
        instance = this;

        //테이블 매니저 세팅
        GameObject tableMgr = new GameObject();
        tableMgr.name = "TableMgr";
        mgrDic.Add(eMgr.TableMgr, tableMgr.AddComponent<TableMgr>());
        
        //옵션 매니저
        GameObject optionMgrObj = new GameObject();
        optionMgrObj.name = "OptionMgr";
        mgrDic.Add(eMgr.OptionMgr, optionMgrObj.AddComponent<OptionMgr>());

        //사운드 매니저
        GameObject soundMgr = new GameObject();
        soundMgr.name = "SoundMgr";
        mgrDic.Add(eMgr.SoundMgr, soundMgr.AddComponent<SoundMgr>());

        //키 입력 매니저
        GameObject inputMgr = new GameObject();
        inputMgr.name = "InputMgr";
        mgrDic.Add(eMgr.InputMgr, inputMgr.AddComponent<InputMgr>());

        //UI 매니저 세팅
        GameObject uiMgrObj = new GameObject();
        uiMgrObj.name = "UIMgr";
        mgrDic.Add(eMgr.UIMgr, uiMgrObj.AddComponent<UIMgr>());

        //씬 매니저
        GameObject sceneMgrObj = new GameObject();
        sceneMgrObj.name = "SceneMgr";
        mgrDic.Add(eMgr.SceneMgr, sceneMgrObj.AddComponent<SceneMgr>());

        #region 추후 추가

        ////세이브 매니저
        //GameObject saveMgrObj = new GameObject();
        //saveMgrObj.name = "SaveMgr";
        //mgrDic.Add(eMgr.SaveMgr, saveMgrObj.AddComponent<SaveMgr>());

        #endregion 추후 추가

        //게임 시작
        GameStart();
    }

    /// <summary> 게임 시작 </summary>
    public void GameStart()
    {
        SceneMgr.instance.OpenCurScene();
    }

    #region Get

    /// <summary> 매니저 클래스를 반환하는 함수 </summary>
    /// <param name="type"> 매니저의 enum 타입 </param>
    public static MgrBase GetMgr(eMgr type)
    {
        if (!mgrDic.TryGetValue(type, out MgrBase mgr))
        {
            Debug.LogError($"{type}에 해당하는 매니저가 없습니다.");
            return null;
        }

        return mgr;
    }

    #endregion Get
}