using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GEnum;

public class SceneMgr : MgrBase
{
    public static SceneMgr instance;

    /// <summary> ���� �� </summary>
    private eScene curScene = eScene.LobbyScene;

    /// <summary> ���� �� </summary>
    public eScene CurScene => curScene;

    private Coroutine changeCoroutine;


    private void Awake()
    {
        instance = this;
    }

    /// <summary> ������ ������ ���� </summary>
    /// <param name="scene"> ����� �� </param>
    public void ChangeScene(eScene scene)
    {
        if (changeCoroutine == null)
        {
            //���� �� ����
            CloseCurScene();

            //������ �� ����
            changeCoroutine = StartCoroutine(OpenScene(scene));
        }
        else
        {
            Debug.LogError($"�� �����߿� [{scene}]���� �����Ϸ� �߽��ϴ�.");
        }
    }

    #region �� ����

    /// <summary> �� ����  </summary>
    private IEnumerator OpenScene(eScene sceneType)
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

                }
                break;
            case eScene.GameScene:
                {

                }
                break;
        }
    }

    #endregion �� ����

    #region �� ����

    /// <summary> ���� �� ���� </summary>
    private void CloseCurScene()
    {
        // ��� PageUI ����
        // �ھ� UI ����
        // ������Ʈ ����
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

    #endregion �� ����
}
