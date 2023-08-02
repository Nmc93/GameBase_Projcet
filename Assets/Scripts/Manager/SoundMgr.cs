using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GEnum;

public class SoundMgr : MgrBase
{
    public static SoundMgr instance;

    /// <summary> 클립 모음 <br/> [Key : ID(SoundTable)] <br/> [Value : AudioClip] </summary>
    public static Dictionary<int, List<SoundCell>> dicSoundClip = new Dictionary<int, List<SoundCell>>();

    /// <summary> 배경 음악 소스 저장소 </summary>
    public static SoundCell bgmPlayer;

    /// <summary> 사운드 파일 경로 </summary>
    private const string path = "Sound\\";

    #region 사운드 옵션

    /// <summary> BGM 사운드 음소거 </summary>
    private bool isBGMMute;
    /// <summary> BGM 사운드 볼륨 </summary>
    private float bGMVol;

    /// <summary> 시스템 사운드 음소거 </summary>
    private bool isSystemMute;
    /// <summary> 시스템 사운드 볼륨 </summary>
    private float systemVol;

    /// <summary> 게임 내 사운드 음소거 </summary>
    private bool isEffectMute;
    /// <summary> 게임 내 사운드 볼륨 </summary>
    private float effectVol;

    #endregion 사운드 옵션

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //사운드 클립 세팅
        SetSound();
    }

    /// <summary> 시작 후 무조건 로드되는 사운드 미리 로드 </summary>
    public void SetSound()
    {
        //브금 세팅
        isBGMMute = OptionMgr.GetBoolOption("Sound_Mute_BGM");
        bGMVol = OptionMgr.GetfloatOption("Sound_Vol_BGM");

        //시스템 사운드 세팅
        isSystemMute = OptionMgr.GetBoolOption("Sound_Mute_System");
        systemVol = OptionMgr.GetfloatOption("Sound_Vol_System");

        //이펙트 사운드 세팅
        isEffectMute = OptionMgr.GetBoolOption("Sound_Mute_Effect");
        effectVol = OptionMgr.GetfloatOption("Sound_Vol_Effect");
    }

    /// <summary> 사운드 실행 </summary>
    /// <param name="soundID"> 실행할 사운드의 ID </param>
    public static void Play(int soundID)
    {
        //안에 해당 ID를 가진 사운드셀이 있을 경우
        if(dicSoundClip.TryGetValue(soundID,out List<SoundCell> list))
        {
            //일하지 않는 셀을 찾아서 플레이
            foreach(var cell in list)
            {
                if(!cell.IsPlaying)
                {
                    cell.Play();
                    return;
                }
            }

            //못찾고 나온 경우 생성, 저장, 실행 
            SoundCell copyCell = new SoundCell(list[0]);
            list.Add(copyCell);
            copyCell.Play();
        }
        //해당 ID를 가진 사운드 셀이 없을 경우
        else
        {
            //사운드 추가
        }
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
            2 => eSoundType.System,
            3 => eSoundType.Effect,
            _ => eSoundType.None
        };
    }

    /// <summary> 해당 ID의 사운드를 로드 후 반환 </summary>
    /// <param name="id"> 사운드의 ID <br/> [SoundTable 참조] </param>
    /// <param name="tbl"> 반환할 사운드 테이블 데이터 </param>
    /// <returns> ID를 찾을 수 없을 경우엔 null 반환 </returns>
    public static AudioSource LoadAudioSource(int id, out SoundTableData tbl)
    {
        AudioSource source = new AudioSource();

        if(TableMgr.Get(id, out tbl))
        {
            AudioClip clip = Resources.Load($"{path}{tbl.Path}", typeof(AudioClip)) as AudioClip;

            if (clip != null)
            {
                source.clip = clip;

                source.transform.SetParent(instance.transform);
                source.transform.localPosition = Vector3.zero;
                source.transform.localRotation = Quaternion.identity;

                switch(ConvertIntToSoundType(tbl.SoundType))
                {
                    case eSoundType.BGM:    //브금
                        source.loop = tbl.IsLoop;
                        break;
                    case eSoundType.System: //시스템 사운드
                        source.loop = tbl.IsLoop;
                        //source.volume
                        break;
                    case eSoundType.Effect: //게임 내 사운드
                        source.loop = tbl.IsLoop;
                        break;
                    case eSoundType.None:   //에러

                        break;
                }

                return source;
            }
            else
            {
                Debug.LogError($"{path}{tbl.Path}의 경로에서 해당 Clip을 찾을 수 없습니다.");
                return null;
            }
        }
        else
        {
            Debug.LogError($"{id}의 ID를 가진 테이블값을 찾을 수 없습니다.");
            return null;
        }
    }

    #region 데이터 클래스

    /// <summary> 오디오 소스 </summary>
    [Serializable]
    public class SoundCell
    {
        /// <summary> 사운드의 ID</summary>
        public int ID { get => tbl.ID; }
        /// <summary> 사운드의 타입 </summary>
        public eSoundType SoundType { get => (eSoundType)tbl.SoundType; }
        /// <summary> 사운드의 재생 여부 </summary>
        public bool IsPlaying { get => source.isPlaying; }

        /// <summary> 해당 사운드의 테이블 데이터 </summary>
        private SoundTableData tbl;
        /// <summary> 오디오 소스 </summary>
        private AudioSource source;

        /// <summary> ID와 타입을 세팅하고 그에 맞는 사운드 소스 생성 </summary>
        /// <param name="id"> 사운드의 ID <br/> [SoundTable 참조] </param>
        public SoundCell(int id)
        {
            source = LoadAudioSource(id, out tbl);
        }

        /// <summary> 매개변수로 받은 셀과 같은 설정과 타입의 셀 생성 </summary>
        /// <param name="cell"> 복사 대상 <br/> [SoundTable 참조] </param>
        public SoundCell(SoundCell cell)
        {

        }

        /// <summary> 실행 </summary>
        public void Play()
        {
            if (source.isPlaying)
            {
                return;
            }

            source.Play();
        }

        /// <summary> 정지 </summary>
        public void Stop()
        {
            source.Stop();
        }
    }

    #endregion 데이터 클래스
}
