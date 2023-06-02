using GEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyMain : MonoBehaviour
{
    private void Start()
    {
    }

    #region ��ư �Լ�

    /// <summary>���ӽ��� ��ư Ŭ��</summary>
    public void OnClickGameStart()
    {
        SceneMgr.instance.SceneChange(eSceneType.Game);
    }

    /// <summary>���� �ɼ� ��ư Ŭ�� </summary>
    public void OnClickGameOption()
    {
        //UI Ȱ��ȭ
        UIMgr.instance.OpenUI(eUIPrefab.OptionPopup);
    }

    /// <summary> ���� ���� ��ư Ŭ�� </summary>
    public void OnClickGameClose()
    {

    }

    #endregion ��ư �Լ�
}
