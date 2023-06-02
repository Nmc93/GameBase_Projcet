using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GEnum
{
    #region Enum
    public enum eSceneType
    {
        Lobby = 0,
        Game,
        End = 100
    }

    public enum eObserverNoticeType
    {
        LobbySceneChange = 0,
        GameSceneChange,
        GameEnd = 100
    }

    public enum eUIPrefab
    {
        //[Description("Prefab/UI/OptionPopup")]
        OptionPopup = 0
    }

    public enum eUIType
    {
        None = 0,
        Scene,
        Page,
        Popup
    }

    #endregion Enum

    #region Attribute
    //public static class Getter
    //{
    //    public static string GetPrefabPath<T>(this T source)
    //    {
    //        FieldInfo fi = source.GetType().GetField(source.ToString());
    //
    //        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
    //
    //        if (attributes != null && attributes.Length > 0)
    //        {
    //            return attributes[0].Description;
    //        }
    //        else
    //        {
    //            return source.ToString();
    //        }
    //    }
    //}
    #endregion Attribute
}