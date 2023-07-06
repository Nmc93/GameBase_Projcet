
namespace GEnum
{
    /// <summary> 매니저에 지정된 enum <br/> 
    /// [매니저 클래스와 이름이 동일함] </summary>
    public enum eMgr
    {
        SceneMgr = 0,
        UIMgr,
        SaveMgr,
        TableMgr,
    }

    /// <summary> 씬 타입, 씬 이름과 동일 </summary>
    public enum eScene
    {
        LobbyScene = 0,
        GameScene,
    }

    /// <summary> 캔버스 타입 </summary>
    public enum eCanvas : byte
    {
        Scene = 0,
        Page,
        Popup
    }

    /// <summary> UI를 쉽게 찾기 위해 지정된 enum <br/> 
    /// [UI 프리팹의 대표 컴포넌트와 이름이 동일함] </summary>
    public enum eUI : short
    {
        UILoading = 0,
        UILobby,
    }

    public enum ePage
    {
        
    }

    /// <summary> 로딩 상태 </summary>
    public enum eLoadingState
    {
        /// <summary> 씬 변경 진행중이 아님 </summary>
        None,
        /// <summary> UI 종료 시작 </summary>
        CloseCurScene,
        /// <summary> 씬 변경 시작 </summary>
        SceneChange,
        /// <summary> 씬 변경 대기 </summary>
        WaitChangeScene
    }

}