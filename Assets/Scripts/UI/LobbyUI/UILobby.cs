using GEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : UIBase
{
    public override eCanvas canvasType => eCanvas.Scene;

    public override eUI uiType => eUI.UILobby;


    /// <summary> 게임 시작 버튼 </summary>
    public void OnClickStartGame()
    {
        SceneMgr.instance.ChangeScene(eScene.GameScene);
    }
}