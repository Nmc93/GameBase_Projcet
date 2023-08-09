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
    public static SoundCell curBGMCell;

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

    #region 생성 및 세팅

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

    /// <summary> 해당 ID의 사운드를 로드 후 반환 </summary>
    /// <param name="id"> 사운드의 ID <br/> [SoundTable 참조] </param>
    /// <param name="tbl"> 반환할 사운드 테이블 데이터 </param>
    /// <returns> ID를 찾을 수 없을 경우엔 null 반환 </returns>
    public static AudioSource CreateAudioSource(int id, out SoundTableData tbl)
    {
        AudioSource source = new GameObject().AddComponent<AudioSource>();

        if (TableMgr.Get(id, out tbl))
        {
            AudioClip clip = Resources.Load($"{path}{tbl.Path}", typeof(AudioClip)) as AudioClip;

            if (clip != null)
            {
                //사운드클립 및 오브젝트 위치 세팅
                source.clip = clip;
                source.transform.SetParent(instance.transform);
                source.transform.localPosition = Vector3.zero;
                source.transform.localRotation = Quaternion.identity;

                //해당 사운드의 타입
                eSoundType sType = ConvertIntToSoundType(tbl.SoundType);

                #region 사운드 오브젝트 이름 설정 - 에디터 전용
#if UNITY_EDITOR
                //에디터에서만 구분용으로 이름 변경
                source.transform.name = $"{id}_{sType}Sound";
#endif
                #endregion 사운드 오브젝트 이름 설정 - 에디터 전용
                switch (sType)
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
                        Debug.LogError($"{id}의 사운드 타입이 비정상적입니다. SoundTable 확인이 필요합니다.");
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

    #endregion 생성 및 세팅

    #region 실행

    /// <summary> 사운드 실행 </summary>
    /// <param name="soundID"> 실행할 사운드의 ID </param>
    /// <param name="tf"> 사운드 위치 지정 </param>
    public static void Play(int soundID, Transform tf = null)
    {
        SoundCell playCell = null;

        //1. ID를 가진 사운드 검색
        //안에 해당 ID를 가진 사운드셀 목록이 있을 경우
        if(dicSoundClip.TryGetValue(soundID,out List<SoundCell> list))
        {
            //일하지 않는 셀을 찾음
            for (int i = 0; i < list.Count; ++i)
            {
                if(!list[i].IsPlaying)
                {
                    playCell = list[i];
                    break;
                }
            }

            //모든 셀이 일하고 있을 경우
            if(playCell == null)
            {
                playCell = new SoundCell(list[0], list.Count);
                list.Add(playCell);
            }
        }
        //해당 ID를 가진 사운드 셀이 없을 경우
        else
        {
            //사운드 추가
            playCell = new SoundCell(soundID, 0);
            dicSoundClip.Add(soundID, new List<SoundCell>() { playCell });
        }

        //이랬는데도 문제가 있어 셀이 없을 경우 종료
        if (playCell == null)
        {
            Debug.LogError($"[ID : {soundID}] 사운드 출력에 실패했습니다.");
            return;
        }

        //2. 찾은 사운드를 통해 
        //브금 타입일 경우 이전에 있는 브금을 종료하고 재생함
        if (playCell.SoundType == eSoundType.BGM)
        {
            //이전 실행중인 브금이 있을 경우 브금을 종료하고 셀을 브금으로 등록
            if (curBGMCell != null)
            {
                Stop(curBGMCell);
                curBGMCell = playCell;
            }
        }

        // 위치가 지정되어 있을 경우 위치에 대한 관리가 필요
        if(tf != null)
        {
            // 관리 코루틴 실행
            instance.StartCoroutine(instance.PlayCoroutine(playCell,tf));
        }
        // 지정되어 있지 않아 관리가 필요없음
        else
        {
            // 그냥 재생
            playCell.Play();
        }
    }

    /// <summary> 위치 지정이 사용되는 사운드 관리 코루틴 </summary>
    /// <param name="cell"> 사용되는 사운드 </param>
    /// <param name="tf"> 사운드의 위치 tf </param>
    IEnumerator PlayCoroutine(SoundCell cell, Transform tf)
    {
        //셀의 위치 지정
        cell.SetPos(tf);
        cell.Play();

        yield return GUtility.GetWaitForSeconds(cell.Time);

        //셀 위치 초기화
        cell.Stop();
        cell.SetPos(transform);
    }

    #endregion 실행

    #region 종료

    /// <summary> 사운드 종료 </summary>
    /// <param name="cell"> 종료할 사운드의 cell </param>
    public static void Stop(SoundCell cell)
    {
        cell.Stop();

        //해당 사운드가 현재 사용중인 BGM인지 체크
        if (cell.ID == curBGMCell.ID &&
            cell.idx == curBGMCell.idx)
        {
            curBGMCell = null;
        }
    }

    /// <summary> 사운드 종료 </summary>
    /// <param name="id"> 사운드의 ID </param>
    /// <param name="idx"> 해당 사운드 목록의 인덱스 </param>
    public static void Stop(int id, int idx)
    {
        //사운드 검색
        if(dicSoundClip.TryGetValue(id,out List<SoundCell> cells))
        {
            //idx 유효 검사
            if(cells.Count < idx)
            {
                //정지
                Stop(cells[idx]);
            }
            else
            {
                Debug.LogError($"{id}ID의 사운드 목록에서 {idx}번째 사운드소스를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError($"{id}의 ID를 가진 사운드를 찾을 수 없습니다.");
        }
    }

    /// <summary> 해당 ID가 걸린 사운드 전부 정지 </summary>
    /// <param name="id"> 정지할 사운드의 ID </param>
    public static void IDStop(int id)
    {
        //검색
        if(dicSoundClip.TryGetValue(id, out List<SoundCell> list))
        {
            //플레이중인 모든 사운드 종료
            foreach (var cell in list)
            {
                if (cell.IsPlaying)
                {
                    cell.Stop();

                    //현재 사용중인 bgm인 경우 curBGMCell 정리
                    if (cell.SoundType == eSoundType.BGM &&
                        cell.ID == curBGMCell.ID && cell.idx == curBGMCell.idx)
                    {
                        curBGMCell = null;
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"{id}의 ID를 가진 사운드 그룹이 없습니다.");
        }
    }

    /// <summary> 지정된 타입의 사운드를 모두 종료 </summary>
    /// <param name="type"> 종료할 타입, 지정하지 않을 경우 모든 사운드가 정지됨 </param>
    public static void TypeAllStop(eSoundType type = eSoundType.None)
    {
        //플레이중인 모든 사운드 종료
        foreach (var group in dicSoundClip)
        {
            //None이거나 타입이 지정된 경우 타입과 사운드 타입이 같은 경우
            if (type == eSoundType.None || type == group.Value[0].SoundType)
            {
                foreach (var cell in group.Value)
                {
                    if (cell.IsPlaying)
                    {
                        cell.Stop();

                        //현재 사용중인 bgm인 경우 curBGMCell 정리
                        if (cell.SoundType == eSoundType.BGM &&
                            cell.ID == curBGMCell.ID && cell.idx == curBGMCell.idx)
                        {
                            curBGMCell = null;
                        }
                    }
                }
            }
        }
    }

    #endregion 종료

    #region 컨버트

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

#endregion 컨버트

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
        /// <summary> 사운드의 재생 시간 </summary>
        public float Time { get => source.clip.length; }

        /// <summary> 현재 셀의 인덱스 </summary>
        public int idx;

        /// <summary> 해당 사운드의 테이블 데이터 </summary>
        private SoundTableData tbl;
        /// <summary> 오디오 소스 </summary>
        private AudioSource source;

        /// <summary> ID와 타입을 세팅하고 그에 맞는 사운드 소스 생성 </summary>
        /// <param name="id"> 사운드의 ID <br/> [SoundTable 참조] </param>
        /// <param name="idx"> 사운드 셀의 인덱스 <br/> SoundMgr.dicSoundClip[i]의 idx </param>
        public SoundCell(int id, int idx)
        {
            source = CreateAudioSource(id, out tbl);
            this.idx = idx;
        }

        /// <summary> 매개변수로 받은 셀과 같은 설정과 타입의 셀 생성 </summary>
        /// <param name="cell"> 복사 대상 <br/> [SoundTable 참조] </param>
        public SoundCell(SoundCell cell, int idx)
        {
            tbl = cell.tbl;
            source = Instantiate(cell.source);
            source.transform.SetParent(instance.transform);
            source.transform.localPosition = Vector3.zero;
            this.idx = idx;
        }

        /// <summary> 위치 지정 </summary>
        public void SetPos(Transform tf)
        {
            source.transform.SetParent(tf);
            source.transform.position = Vector3.zero;
        }

        /// <summary> 실행 </summary>
        public void Play()
        {
            if (!source.isPlaying)
            {
                source.Play();
            }
        }

        /// <summary> 정지 </summary>
        public void Stop()
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    #endregion 데이터 클래스
}