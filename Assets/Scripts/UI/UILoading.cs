using GEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{
    public override eCanvas canvasType => eCanvas.Page;

    public override eUI uiType => eUI.UILoading;

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