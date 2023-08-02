using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GEnum;

public class SoundMgr : MgrBase
{
    public static SoundMgr instance;

    /// <summary> Ŭ�� ���� <br/> [Key : ID(SoundTable)] <br/> [Value : AudioClip] </summary>
    public static Dictionary<int, List<SoundCell>> dicSoundClip = new Dictionary<int, List<SoundCell>>();

    /// <summary> ��� ���� �ҽ� ����� </summary>
    public static SoundCell bgmPlayer;

    /// <summary> ���� ���� ��� </summary>
    private const string path = "Sound\\";

    #region ���� �ɼ�

    /// <summary> BGM ���� ���Ұ� </summary>
    private bool isBGMMute;
    /// <summary> BGM ���� ���� </summary>
    private float bGMVol;

    /// <summary> �ý��� ���� ���Ұ� </summary>
    private bool isSystemMute;
    /// <summary> �ý��� ���� ���� </summary>
    private float systemVol;

    /// <summary> ���� �� ���� ���Ұ� </summary>
    private bool isEffectMute;
    /// <summary> ���� �� ���� ���� </summary>
    private float effectVol;

    #endregion ���� �ɼ�

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //���� Ŭ�� ����
        SetSound();
    }

    /// <summary> ���� �� ������ �ε�Ǵ� ���� �̸� �ε� </summary>
    public void SetSound()
    {
        //��� ����
        isBGMMute = OptionMgr.GetBoolOption("Sound_Mute_BGM");
        bGMVol = OptionMgr.GetfloatOption("Sound_Vol_BGM");

        //�ý��� ���� ����
        isSystemMute = OptionMgr.GetBoolOption("Sound_Mute_System");
        systemVol = OptionMgr.GetfloatOption("Sound_Vol_System");

        //����Ʈ ���� ����
        isEffectMute = OptionMgr.GetBoolOption("Sound_Mute_Effect");
        effectVol = OptionMgr.GetfloatOption("Sound_Vol_Effect");
    }

    /// <summary> ���� ���� </summary>
    /// <param name="soundID"> ������ ������ ID </param>
    public static void Play(int soundID)
    {
        //�ȿ� �ش� ID�� ���� ���弿�� ���� ���
        if(dicSoundClip.TryGetValue(soundID,out List<SoundCell> list))
        {
            //������ �ʴ� ���� ã�Ƽ� �÷���
            foreach(var cell in list)
            {
                if(!cell.IsPlaying)
                {
                    cell.Play();
                    return;
                }
            }

            //��ã�� ���� ��� ����, ����, ���� 
            SoundCell copyCell = new SoundCell(list[0]);
            list.Add(copyCell);
            copyCell.Play();
        }
        //�ش� ID�� ���� ���� ���� ���� ���
        else
        {
            //���� �߰�
        }
    }

    /// <summary> ���� ���� </summary>
    /// <param name="soundID"> ������ ������ ID </param>
    public static void Stop(int soundID)
    {

    }

    /// <summary> Ÿ�Թ�ȣ�� ����Ÿ������ ��ȯ </summary>
    /// <param name="typeNum"> GEnum.eSoundType ���� </param>
    /// <returns> 0 or �������� ���� ��ȣ ���� �� eSoundType.None ��ȯ </returns>
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

    /// <summary> �ش� ID�� ���带 �ε� �� ��ȯ </summary>
    /// <param name="id"> ������ ID <br/> [SoundTable ����] </param>
    /// <param name="tbl"> ��ȯ�� ���� ���̺� ������ </param>
    /// <returns> ID�� ã�� �� ���� ��쿣 null ��ȯ </returns>
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
                    case eSoundType.BGM:    //���
                        source.loop = tbl.IsLoop;
                        break;
                    case eSoundType.System: //�ý��� ����
                        source.loop = tbl.IsLoop;
                        //source.volume
                        break;
                    case eSoundType.Effect: //���� �� ����
                        source.loop = tbl.IsLoop;
                        break;
                    case eSoundType.None:   //����

                        break;
                }

                return source;
            }
            else
            {
                Debug.LogError($"{path}{tbl.Path}�� ��ο��� �ش� Clip�� ã�� �� �����ϴ�.");
                return null;
            }
        }
        else
        {
            Debug.LogError($"{id}�� ID�� ���� ���̺��� ã�� �� �����ϴ�.");
            return null;
        }
    }

    #region ������ Ŭ����

    /// <summary> ����� �ҽ� </summary>
    [Serializable]
    public class SoundCell
    {
        /// <summary> ������ ID</summary>
        public int ID { get => tbl.ID; }
        /// <summary> ������ Ÿ�� </summary>
        public eSoundType SoundType { get => (eSoundType)tbl.SoundType; }
        /// <summary> ������ ��� ���� </summary>
        public bool IsPlaying { get => source.isPlaying; }

        /// <summary> �ش� ������ ���̺� ������ </summary>
        private SoundTableData tbl;
        /// <summary> ����� �ҽ� </summary>
        private AudioSource source;

        /// <summary> ID�� Ÿ���� �����ϰ� �׿� �´� ���� �ҽ� ���� </summary>
        /// <param name="id"> ������ ID <br/> [SoundTable ����] </param>
        public SoundCell(int id)
        {
            source = LoadAudioSource(id, out tbl);
        }

        /// <summary> �Ű������� ���� ���� ���� ������ Ÿ���� �� ���� </summary>
        /// <param name="cell"> ���� ��� <br/> [SoundTable ����] </param>
        public SoundCell(SoundCell cell)
        {

        }

        /// <summary> ���� </summary>
        public void Play()
        {
            if (source.isPlaying)
            {
                return;
            }

            source.Play();
        }

        /// <summary> ���� </summary>
        public void Stop()
        {
            source.Stop();
        }
    }

    #endregion ������ Ŭ����
}
