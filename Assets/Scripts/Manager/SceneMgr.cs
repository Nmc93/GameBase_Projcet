using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GEnum;

public class SceneMgr : MgrBase
{
    public static SceneMgr instance;


    /// <summary> 현재 씬 </summary>
    public eScene CurScene => curScene;

    /// <summary> 현재 씬 </summary>
    private eScene curScene = eScene.LobbyScene;
    /// <summary> 비동기 씬 변경 코루틴 </summary>
    private Coroutine changeCoroutine;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    /// <summary> 지정된 씬으로 변경 </summary>
    /// <param name="scene"> 변경될 씬 </param>
    public void ChangeScene(eScene scene)
    {
        if (curScene == scene)
        {
            Debug.LogError("현재 씬과 같은 씬으로는 이동할 수 없습니다.");
            return;
        }

        //씬 변경 코루틴이 돌고 있지 않을 경우에만
        if (changeCoroutine == null)
        {
            //로딩 UI 활성화 및 저장
            if (UIMgr.OpenUI(eUI.UILoading) && UIMgr.instance.GetUI(out UILoading loading))
            {
                //로딩 UI를 불러오면서 로딩 UI의 상태는 자동적으로 실행

                //----------------------------- 씬 종료 --------------------------------
                // 모든 PageUI 종료
                UIMgr.UIAllClose();

                //----------------------------- 씬 시작 --------------------------------
                //로딩 UI의 상태를 씬 변경중으로 변경
                loading.ChangeState(UILoading.eLoadingState.SceneChange);
                //비동기 씬 전환 실행
                changeCoroutine = StartCoroutine(OpenScene(scene, loading));
            }
        }
        else
        {
            Debug.LogError($"씬 변경중에 씬을 변경할 수 없습니다.");
        }
    }

    #region 씬 전환

    /// <summary> 씬 변경  </summary>
    private IEnumerator OpenScene(eScene sceneType, UILoading loadingUI)
    {
        //변경된 씬을 대기시킴
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneType.ToString());
        operation.allowSceneActivation = false;

        // 씬 UI를 출력함
        //

        //완료하기 전까지 대기
        while (!operation.isDone)
        {
            //대기중 할 일 넣으면 될듯
            yield return new WaitForSeconds(0.1f);

            //씬 변경 허가
            if(operation.progress > 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            // 씬 UI에서 로딩 진행도를 보여줌
            //
        }


        //현재 씬 변경, 오픈된 씬 대응
        curScene = sceneType;
        OpenCurScene();

        // 씬 UI 종료
        //
    }

    /// <summary> 씬 오픈 </summary>
    public void OpenCurScene()
    {
        //완료하고 씬이 변했음, 씬에 맞게 세팅
        switch (curScene)
        {
            case eScene.LobbyScene:
                {
                    //로비 씬 오픈
                    UIMgr.OpenUI(eUI.UILobby);
                }
                break;
            case eScene.GameScene:
                {

                }
                break;
        }
    }

    #endregion 씬 전환
}
