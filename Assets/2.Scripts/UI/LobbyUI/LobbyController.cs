using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GEnum;

public class LobbyController : MonoBehaviour
{
    #region 인스펙터
    [Header("[버튼 오브젝트]")]
    [SerializeField][Tooltip("게임 스타트 버튼")]
    private Button gameStartBtn = null;
    [SerializeField][Tooltip("옵션 버튼")]
    private Button optionBtm = null;
    [SerializeField][Tooltip("종료버튼")]
    private Button gameCloseBtn = null;

    [Header("[UI 캔버스]")]
    [SerializeField][Tooltip("로비씬 UI 위치 포인트")]
    private GameObject lobbyCanvas = null;
    [SerializeField][Tooltip("로비씬 UI 위치 포인트")]
    private GameObject lobbyPopupPoint = null;
    #endregion 인스펙터

    private void Start()
    {
        //버튼 이벤트 삽입
        gameStartBtn.onClick.AddListener(OnClickGameStart);
        optionBtm.onClick.AddListener(OnClickGameOption);
        gameCloseBtn.onClick.AddListener(OnClickGameClose);
    }

    #region 버튼 함수
    /// <summary>게임시작 버튼 클릭</summary>
    private void OnClickGameStart()
    {
        SceneMgr.instance.SceneChange(eSceneType.Game);
    }

    /// <summary>게임 옵션 버튼 클릭 </summary>
    private void OnClickGameOption()
    {
        //UI 활성화
        UIMgr.instance.OpenUI(eUIPrefab.OptionPopup, lobbyCanvas, lobbyPopupPoint.transform.position);
    }

    /// <summary> 게임 종료 버튼 클릭 </summary>
    private void OnClickGameClose()
    {

    }
    #endregion 버튼 함수
}
