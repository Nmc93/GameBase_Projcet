
namespace GEnum
{
    /// <summary> �Ŵ����� ������ enum <br/> 
    /// [�Ŵ��� Ŭ������ �̸��� ������] </summary>
    public enum eMgr
    {
        SceneMgr = 0,
        UIMgr,
        SaveMgr,
        TableMgr,
    }

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
    }

    public enum ePage
    {
        
    }

}