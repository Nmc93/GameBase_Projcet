using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GEnum;

public class SceneMgr : MgrBase
{
    public static SceneMgr instance;

    private eScene curScene = eScene.Login;

    public override void Init()
    {
        instance = this;

    }

    public void ChangeScene(eScene scene)
    {
        switch(scene)
        {
            case eScene.Login:
                break;
            case eScene.Game:
                break;
        }
    }

    private void CloseCurScene()
    {

    }

    private void OpenCurScene()
    {

    }

}
