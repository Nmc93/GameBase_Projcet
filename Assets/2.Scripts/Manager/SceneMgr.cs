using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GEnum;

public class SceneMgr : MgrBase
{
    public static SceneMgr instance = null;

    private eSceneType sceneType;

    #region 프로퍼티

    /// <summary> 씬 타입 </summary>
    public eSceneType CurSceneType { get => sceneType; }

    /// <summary> 로딩 게이지 </summary>
    public float LoadingGauge { get; private set; }

    #endregion 프로퍼티

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

        sceneType = eSceneType.Lobby;
    }

    /// <summary>씬 변경 함수</summary>
    /// <param name="scene"> 변경할 씬 </param>
    public void SceneChange(eSceneType scene)
    {
        sceneType = scene;

        switch (sceneType)
        {
            case eSceneType.Lobby:
                {
                    StartCoroutine(LoadingScene("LobbyScene"));
                    ObserverMgr.NoticeCall(eObserverNoticeType.LobbySceneChange);
                    Debug.Log(sceneType + ": Change");
                }
                break;
            case eSceneType.Game:
                {
                    StartCoroutine(LoadingScene("GameScene"));
                    ObserverMgr.NoticeCall(eObserverNoticeType.GameSceneChange);
                    Debug.Log(sceneType + ": Change");
                }
                break;
            default:
                {
                    Debug.LogError("[" + sceneType + "] 씬이 없습니다");
                }
                break;
        }
    }

    IEnumerator LoadingScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        LoadingGauge = 0f;
        
        while (!operation.isDone)
        {
            LoadingGauge = operation.progress;

            if(operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
                yield break;
            }
        }

        yield return null;
    }
}
