using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GEnum;

public class SceneMgr : MgrBase
{
    public static SceneMgr instance;

    #region 이벤트
    /// <summary> 씬 변경 진행도 갱신 이벤트 </summary>
    public Action<float> onGetchanProgress;
    /// <summary> 씬 변경 상태 이벤트 </summary>
    public Action<eLoadingState> onGetChanState;
    #endregion 이벤트

    #region 프로퍼티

    /// <summary> 현재 씬 </summary>
    public eScene CurScene 
    { 
        get => curScene;
    }

    /// <summary> 현재 씬 변경 상태 </summary>
    public eLoadingState CurState
    {
        get => curState;
        private set
        {
            curState = value;

            if(onGetChanState != null)
            {
                onGetChanState(value);
            }
        }
    }

    #endregion 프로퍼티

    #region private
    /// <summary> 현재 씬 </summary>
    private eScene curScene = eScene.LobbyScene;
    /// <summary> 현재 씬 변경 상태 </summary>
    private eLoadingState curState = eLoadingState.None; 

    /// <summary> 비동기 씬 변경 코루틴 </summary>
    private Coroutine changeCoroutine;
    /// <summary> 비동기 씬 변경 오퍼레이션 </summary>
    private AsyncOperation operation;

    /// <summary> 씬 변경 준비 완료시 바로 변경하지 않고 대기하는지 여부 </summary>
    private bool isWaitForNextScene;
    #endregion private

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //추후 테이블을 읽은 뒤에 테이블에 따라서 변경(프랩스 저장은 하지 않음)
        isWaitForNextScene = OptionMgr.GetBoolOption("IsWaitNextScene", false);
    }

    /// <summary> 지정된 씬으로 변경 </summary>
    /// <param name="scene"> 변경될 씬 </param>
    public void ChangeScene(eScene scene)
    {
        //현재씬에서 현재씬으로 이동하는건 불가능
        if (CurScene == scene)
        {
            Debug.LogError("현재 씬과 같은 씬으로는 이동할 수 없습니다.");
            return;
        }

        //씬 변경 코루틴이 돌고 있지 않을 경우에만
        if (changeCoroutine == null)
        {
            //---------------------------- 씬 변경 UI 오픈 ------------------------------
            if(!UIMgr.OpenUI(eUI.UILoading))
            {
                //UI활성화에 실패했는데 대기할 경우 다음씬 이동이 불가능하니 강제 이동
                isWaitForNextScene = false;
            }

            //----------------------------- 현재 씬 종료 --------------------------------
            CurState = eLoadingState.CloseCurScene;
            // 모든 PageUI 종료
            UIMgr.SceneChangeAllUIClose();

            //----------------------------- 다음 씬 준비 --------------------------------
            //로딩 UI의 상태를 씬 변경중으로 변경
            CurState = eLoadingState.SceneChange;
            //비동기 씬 전환 실행
            changeCoroutine = StartCoroutine(OpenScene(scene));
        }
        else
        {
            Debug.LogError($"씬 변경중에 씬을 변경할 수 없습니다.");
        }
    }

    #region 씬 오픈

    /// <summary> 씬 변경  </summary>
    private IEnumerator OpenScene(eScene sceneType)
    {
        //변경된 씬을 대기시킴
        operation = SceneManager.LoadSceneAsync(sceneType.ToString());
        operation.allowSceneActivation = false;

        //----------------------------- 씬 로딩 시작 --------------------------------
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            //로딩이 90% 진행된다면 설정에 따라 씬을 변경하거나 대기
            if (operation.progress >= 0.90f)
            {
                //바로 변경하지 않고 대기한다면
                if (isWaitForNextScene)
                {
                    CurState = eLoadingState.WaitChangeScene;
                }
                //준비된대로 바로 씬을 변경할 경우
                else
                {
                    operation.allowSceneActivation = true;
                }
                break;
            }

            // 현재 씬 변경 진행도 퍼센트 갱신
            if (onGetchanProgress != null)
            {
                onGetchanProgress(operation.progress);
            }
        }

        //씬 준비 퍼센트를 100%로 변경
        if (onGetchanProgress != null)
        {
            onGetchanProgress(1f);
        }

        //----------------------------- 씬 변경 대기 --------------------------------
        while (!operation.allowSceneActivation)
        {
            yield return new WaitForSeconds(0.5f);
        }

        //------------------------------ 씬 변경 ------------------------------------
        //현재 씬 변경, 오픈된 씬 대응
        curScene = sceneType;
        OpenCurScene();
    }

    /// <summary> 씬 오픈 </summary>
    public void OpenCurScene()
    {
        //씬 변경 코루틴이 실행중이라면 종료
        if (changeCoroutine != null)
        {
            StopCoroutine(changeCoroutine);
            changeCoroutine = null;
        }

        //완료하고 씬이 변했음, 씬에 맞게 세팅
        switch (CurScene)
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

        //씬 변경 종료
        CurState = eLoadingState.None;
    }

    #endregion 씬 오픈

    /// <summary> 씬 변경 허가 </summary>
    public void MoveNextScene()
    {
        //진행중이고 진행도가 일정 이상 높아졌을 경우
        if(operation != null && operation.progress >= 0.9f)
        {
            //씬 변경 허가
            operation.allowSceneActivation = true;
        }
    }
}
