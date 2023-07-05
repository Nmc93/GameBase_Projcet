using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GEnum;

public class SceneMgr : MgrBase
{
    public static SceneMgr instance;


    /// <summary> ���� �� </summary>
    public eScene CurScene => curScene;

    /// <summary> ���� �� </summary>
    private eScene curScene = eScene.LobbyScene;
    /// <summary> �񵿱� �� ���� �ڷ�ƾ </summary>
    private Coroutine changeCoroutine;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    /// <summary> ������ ������ ���� </summary>
    /// <param name="scene"> ����� �� </param>
    public void ChangeScene(eScene scene)
    {
        if (curScene == scene)
        {
            Debug.LogError("���� ���� ���� �����δ� �̵��� �� �����ϴ�.");
            return;
        }

        //�� ���� �ڷ�ƾ�� ���� ���� ���� ��쿡��
        if (changeCoroutine == null)
        {
            //�ε� UI Ȱ��ȭ �� ����
            if (UIMgr.OpenUI(eUI.UILoading) && UIMgr.instance.GetUI(out UILoading loading))
            {
                //�ε� UI�� �ҷ����鼭 �ε� UI�� ���´� �ڵ������� ����

                //----------------------------- �� ���� --------------------------------
                // ��� PageUI ����
                UIMgr.UIAllClose();

                //----------------------------- �� ���� --------------------------------
                //�ε� UI�� ���¸� �� ���������� ����
                loading.ChangeState(UILoading.eLoadingState.SceneChange);
                //�񵿱� �� ��ȯ ����
                changeCoroutine = StartCoroutine(OpenScene(scene, loading));
            }
        }
        else
        {
            Debug.LogError($"�� �����߿� ���� ������ �� �����ϴ�.");
        }
    }

    #region �� ��ȯ

    /// <summary> �� ����  </summary>
    private IEnumerator OpenScene(eScene sceneType, UILoading loadingUI)
    {
        //����� ���� ����Ŵ
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneType.ToString());
        operation.allowSceneActivation = false;

        // �� UI�� �����
        //

        //�Ϸ��ϱ� ������ ���
        while (!operation.isDone)
        {
            //����� �� �� ������ �ɵ�
            yield return new WaitForSeconds(0.1f);

            //�� ���� �㰡
            if(operation.progress > 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            // �� UI���� �ε� ���൵�� ������
            //
        }


        //���� �� ����, ���µ� �� ����
        curScene = sceneType;
        OpenCurScene();

        // �� UI ����
        //
    }

    /// <summary> �� ���� </summary>
    public void OpenCurScene()
    {
        //�Ϸ��ϰ� ���� ������, ���� �°� ����
        switch (curScene)
        {
            case eScene.LobbyScene:
                {
                    //�κ� �� ����
                    UIMgr.OpenUI(eUI.UILobby);
                }
                break;
            case eScene.GameScene:
                {

                }
                break;
        }
    }

    #endregion �� ��ȯ
}
