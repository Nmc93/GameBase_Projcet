using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GEnum;

public class SceneMgr : MgrBase
{
    public static SceneMgr instance;

    /// <summary> 현재 씬 </summary>
    private eScene curScene = eScene.LobbyScene;

    /// <summary> 현재 씬 </summary>
    public eScene CurScene => curScene;

    private Coroutine changeCoroutine;


    private void Awake()
    {
        instance = this;
    }

    /// <summary> 지정된 씬으로 변경 </summary>
    /// <param name="scene"> 변경될 씬 </param>
    public void ChangeScene(eScene scene)
    {
        if (changeCoroutine == null)
        {
            //현재 씬 종료
            CloseCurScene();

            //지정된 씬 오픈
            changeCoroutine = StartCoroutine(OpenScene(scene));
        }
        else
        {
            Debug.LogError($"씬 변경중에 [{scene}]으로 변경하려 했습니다.");
        }
    }

    #region 씬 오픈

    /// <summary> 씬 변경  </summary>
    private IEnumerator OpenScene(eScene sceneType)
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

                }
                break;
            case eScene.GameScene:
                {

                }
                break;
        }
    }

    #endregion 씬 오픈

    #region 씬 종료

    /// <summary> 현재 씬 종료 </summary>
    private void CloseCurScene()
    {
        // 모든 PageUI 종료
        // 코어 UI 종료
        // 오브젝트 정리
        switch (curScene)
        {
            case eScene.LobbyScene:
                {
                }
                break;
            case eScene.GameScene:
                {
                }
                break;
        }
    }

    #endregion 씬 종료
}
