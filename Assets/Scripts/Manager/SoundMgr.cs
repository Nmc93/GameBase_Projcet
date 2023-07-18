using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GEnum;

public class SoundMgr : MgrBase
{
    public static SoundMgr instance;

    /// <summary> 클립 모음 <br/> [Key : ID(SoundTable)] <br/> [Value : AudioClip] </summary>
    public static Dictionary<int, AudioClip> dicSoundClip = new Dictionary<int, AudioClip>();

    /// <summary> 배경 음악 소스 저장소 </summary>
    public static SoundPlayer bgmPlayer;

    /// <summary> 사운드 소스 저장소 </summary>
    public static Queue<SoundPlayer> SoundPool = new Queue<SoundPlayer>();

    public Transform Pool => pool;
    private Transform pool;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //사운드소스 오브젝트 저장용 풀
        pool = new GameObject("SoundPool").transform;
        pool.transform.SetParent(transform);
        pool.transform.localPosition = Vector3.zero;
        pool.transform.localRotation = Quaternion.identity;
        pool.transform.localScale = Vector3.one;

        //사운드 클립 세팅
        SetSound();
    }

    /// <summary> 시작 후 무조건 로드되는 사운드 미리 로드 </summary>
    public void SetSound()
    {

    }

    /// <summary> 사운드 실행 </summary>
    /// <param name="soundID"> 실행할 사운드의 ID </param>
    public static void Play(int soundID)
    {

    }

    /// <summary> 사운드 종료 </summary>
    /// <param name="soundID"> 종료할 사운드의 ID </param>
    public static void Stop(int soundID)
    {

    }

    /// <summary> 타입번호를 사운드타입으로 변환 </summary>
    /// <param name="typeNum"> GEnum.eSoundType 참조 </param>
    /// <returns> 0 or 지정되지 않은 번호 선택 시 eSoundType.None 반환 </returns>
    public static eSoundType ConvertIntToSoundType(int typeNum)
    {
        return typeNum switch
        {
            1 => eSoundType.BGM,
            2 => eSoundType.Effect,
            _ => eSoundType.None
        };
    }

    #region 데이터 클래스

    /// <summary> 오디오 소스 </summary>
    [Serializable]
    public class SoundPlayer
    {
        /// <summary> 사운드의 ID <br/> [Default ID : -1]</summary>
        public int ID = -1;
        
        /// <summary> 오디오 소스 </summary>
        public AudioSource Source { get => source; }
        /// <summary> 오디오 소스 </summary>
        private AudioSource source;

        /// <summary> 오디오 소스의 게임 오브젝트 </summary>
        public GameObject Obj { get => obj; }
        /// <summary> 오디오 소스의 게임 오브젝트 </summary>
        private GameObject obj;

        public SoundPlayer(Transform parent)
        {
            obj = new GameObject();
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;

            source = obj.AddComponent<AudioSource>();
        }

        /// <summary> 클립 및 데이터 세팅 </summary>
        /// <param name="ID"> 사용할 사운드의 ID </param>
        public void DataSet(int ID)
        {
            TableMgr.Get<SoundTableData>(ID,out SoundTableData tbl);
        }

        /// <summary> 실행 </summary>
        public void Play()
        {

        }

        /// <summary> 정지 </summary>
        public void Stop()
        {

        }
    }

    #endregion 데이터 클래스
}
