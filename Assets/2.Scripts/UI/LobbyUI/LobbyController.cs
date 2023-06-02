using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GEnum;

public class LobbyController : MonoBehaviour
{
    #region �ν�����
    [Header("[��ư ������Ʈ]")]
    [SerializeField][Tooltip("���� ��ŸƮ ��ư")]
    private Button gameStartBtn = null;
    [SerializeField][Tooltip("�ɼ� ��ư")]
    private Button optionBtm = null;
    [SerializeField][Tooltip("�����ư")]
    private Button gameCloseBtn = null;

    [Header("[UI ĵ����]")]
    [SerializeField][Tooltip("�κ�� UI ��ġ ����Ʈ")]
    private GameObject lobbyCanvas = null;
    [SerializeField][Tooltip("�κ�� UI ��ġ ����Ʈ")]
    private GameObject lobbyPopupPoint = null;
    #endregion �ν�����

    private void Start()
    {
        //��ư �̺�Ʈ ����
        gameStartBtn.onClick.AddListener(OnClickGameStart);
        optionBtm.onClick.AddListener(OnClickGameOption);
        gameCloseBtn.onClick.AddListener(OnClickGameClose);
    }

    #region ��ư �Լ�
    /// <summary>���ӽ��� ��ư Ŭ��</summary>
    private void OnClickGameStart()
    {
        SceneMgr.instance.SceneChange(eSceneType.Game);
    }

    /// <summary>���� �ɼ� ��ư Ŭ�� </summary>
    private void OnClickGameOption()
    {
        //UI Ȱ��ȭ
        UIMgr.instance.OpenUI(eUIPrefab.OptionPopup, lobbyCanvas, lobbyPopupPoint.transform.position);
    }

    /// <summary> ���� ���� ��ư Ŭ�� </summary>
    private void OnClickGameClose()
    {

    }
    #endregion ��ư �Լ�
}
