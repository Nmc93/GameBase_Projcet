using System;

namespace GEnum
{
    #region ���� �Ŵ���
    /// <summary> �Ŵ����� ������ enum <br/> 
    /// [�Ŵ��� Ŭ������ �̸��� ������] </summary>
    public enum eMgr
    {
        SceneMgr = 0,
        UIMgr,
        SaveMgr,
        TableMgr,
        OptionMgr,
        InputMgr,
    }
    #endregion ���� �Ŵ���

    #region UI ����
    /// <summary> �� Ÿ��, �� �̸��� ���� </summary>
    public enum eScene
    {
        LobbyScene = 0,
        GameScene,
    }

    /// <summary> ĵ���� Ÿ�� </summary>
    public enum eCanvas : byte
    {
        Scene = 0,
        Page,
        Popup
    }

    /// <summary> UI�� ���� ã�� ���� ������ enum <br/> 
    /// [UI �������� ��ǥ ������Ʈ�� �̸��� ������] </summary>
    public enum eUI : short
    {
        UILoading = 0,
        UILobby,
    }

    public enum ePage
    {
        
    }
    #endregion UI ����

    #region �ε�
    /// <summary> �ε� ���� </summary>
    public enum eLoadingState
    {
        /// <summary> �� ���� �������� �ƴ� </summary>
        None,
        /// <summary> UI ���� ���� </summary>
        CloseCurScene,
        /// <summary> �� ���� ���� </summary>
        SceneChange,
        /// <summary> �� ���� ��� </summary>
        WaitChangeScene
    }
    #endregion �ε�

    #region �Է� Ÿ��

    public enum eInputType
    {
        /// <summary> ���� �� �̵� </summary>
        MoveNextScene = 0,
        /// <summary> Ű�� ������, ������� ���� </summary>
        Count
    }

    #endregion �Է� Ÿ��

    #region ����
    public enum eSoundType
    {
        BGM = 0,
        Effect,
    }


    #endregion ����
}