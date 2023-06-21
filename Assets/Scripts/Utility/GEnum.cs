
namespace GEnum
{
    public enum eMgr
    {
        SceneMgr = 0,
        UIMgr,
        SaveMgr,
    }

    /// <summary> 씬 타입, 씬 이름과 동일 </summary>
    public enum eScene
    {
        LobbyScene = 0,
        GameScene,
    }

    /// <summary> UI의 Page, Popup 여부 </summary>
    public enum eUIType : byte
    {
        Scene = 0,
        Page,
        Popup
    }

    /// <summary> UI의 분류 </summary>
    public enum eUIName : short
    {
        UILoading = 0,
    }

    public enum ePage
    {
        
    }

}