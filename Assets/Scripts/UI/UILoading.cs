using GEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{
    public override eUIType uiType => eUIType.Page;

    public override eUIName uiName => eUIName.UILoading;

    #region �ν�����

    [Header("[�ε� ��ũ�� ��]"), Tooltip("�ε� ��ũ�� ��")]
    [SerializeField] private Scrollbar scrollbar;

    #endregion �ν�����

    /// <summary> �ε� ���� ��ũ�� </summary>
    public float ScrollProgress
    {
        set => scrollbar.size = value;
    }
}