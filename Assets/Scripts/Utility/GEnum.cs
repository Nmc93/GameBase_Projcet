using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GEnum
{
    public enum eMgr
    {
        SceneMgr = 0,
        UIMgr,
        SaveMgr,
    }

    /// <summary> �� Ÿ��, �� �̸��� ���� </summary>
    public enum eScene
    {
        LobbyScene = 0,
        GameScene,
    }

    /// <summary> UI�� Page, Popup ���� </summary>
    public enum eUIType : byte
    {
        Scene = 0,
        Page,
        Popup
    }

    /// <summary> UI�� �з� </summary>
    public enum eUIName : short
    {
        UILoading = 0,
    }

    public enum ePage
    {
        
    }

}