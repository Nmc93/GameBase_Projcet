using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GEnum;

public class SceneMgr : MgrBase
{
    public static SceneMgr instance = null;

    private eSceneType sceneType;

    #region ������Ƽ

    /// <summary> �� Ÿ�� </summary>
    public eSceneType CurSceneType { get => sceneType; }

    /// <summary> �ε� ������ </summary>
    public float LoadingGauge { get; private set; }

    #endregion ������Ƽ

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

        sceneType = eSceneType.Lobby;
    }

    /// <summary>�� ���� �Լ�</summary>
    /// <param name="scene"> ������ �� </param>
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
                    Debug.LogError("[" + sceneType + "] ���� �����ϴ�");
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
