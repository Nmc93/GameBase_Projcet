using GEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{
    public override eCanvas canvasType => eCanvas.Page;

    public override eUI uiType => eUI.UILoading;

    /// <summary> �ε� ���� </summary>
    public enum eLoadingState
    {
        /// <summary> UI ���� ���� </summary>
        CloseCurScene,
        /// <summary> �� ���� ���� </summary>
        SceneChange,
    }

    #region �ν�����

    [Header("------------------ UI -----------------")]
    [Header("[�ε� ��ũ�� ��]"), Tooltip("�ε� ��ũ�� ��")]
    [SerializeField] private Scrollbar scrollbar;

    #endregion �ν�����

    /// <summary> �ε� ���� ��ũ�� </summary>
    public float ScrollProgress
    {
        set => scrollbar.size = value;
    }

    /// <summary> �ε� ���� ���� </summary>
    /// <param name="state"></param>
    public void ChangeState(eLoadingState state)
    {
        switch(state)
        {
            case eLoadingState.CloseCurScene:
                break;
            case eLoadingState.SceneChange:
                break;
        }
    }
}