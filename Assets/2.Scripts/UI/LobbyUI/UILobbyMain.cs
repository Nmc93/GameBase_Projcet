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

    #region 버튼 함수

    /// <summary>게임시작 버튼 클릭</summary>
    public void OnClickGameStart()
    {
        SceneMgr.instance.SceneChange(eSceneType.Game);
    }

    /// <summary>게임 옵션 버튼 클릭 </summary>
    public void OnClickGameOption()
    {
        //UI 활성화
        UIMgr.instance.OpenUI(eUIPrefab.OptionPopup);
    }

    /// <summary> 게임 종료 버튼 클릭 </summary>
    public void OnClickGameClose()
    {

    }

    #endregion 버튼 함수
}
