using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GEnum;

public class SoundMgr : MgrBase
{
    public static SoundMgr instance;

    /// <summary> Ŭ�� ���� <br/> [Key : ID(SoundTable)] <br/> [Value : AudioClip] </summary>
    public static Dictionary<int, AudioClip> dicSoundClip = new Dictionary<int, AudioClip>();

    /// <summary> ��� ���� �ҽ� ����� </summary>
    public static SoundPlayer bgmPlayer;

    /// <summary> ���� �ҽ� ����� </summary>
    public static Queue<SoundPlayer> SoundPool = new Queue<SoundPlayer>();

    public Transform Pool => pool;
    private Transform pool;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //����ҽ� ������Ʈ ����� Ǯ
        pool = new GameObject("SoundPool").transform;
        pool.transform.SetParent(transform);
        pool.transform.localPosition = Vector3.zero;
        pool.transform.localRotation = Quaternion.identity;
        pool.transform.localScale = Vector3.one;

        //���� Ŭ�� ����
        SetSound();
    }

    /// <summary> ���� �� ������ �ε�Ǵ� ���� �̸� �ε� </summary>
    public void SetSound()
    {

    }

    /// <summary> ���� ���� </summary>
    /// <param name="soundID"> ������ ������ ID </param>
    public static void Play(int soundID)
    {

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
            2 => eSoundType.Effect,
            _ => eSoundType.None
        };
    }

    #region ������ Ŭ����

    /// <summary> ����� �ҽ� </summary>
    [Serializable]
    public class SoundPlayer
    {
        /// <summary> ������ ID <br/> [Default ID : -1]</summary>
        public int ID = -1;
        
        /// <summary> ����� �ҽ� </summary>
        public AudioSource Source { get => source; }
        /// <summary> ����� �ҽ� </summary>
        private AudioSource source;

        /// <summary> ����� �ҽ��� ���� ������Ʈ </summary>
        public GameObject Obj { get => obj; }
        /// <summary> ����� �ҽ��� ���� ������Ʈ </summary>
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

        /// <summary> Ŭ�� �� ������ ���� </summary>
        /// <param name="ID"> ����� ������ ID </param>
        public void DataSet(int ID)
        {
            TableMgr.Get<SoundTableData>(ID,out SoundTableData tbl);
        }

        /// <summary> ���� </summary>
        public void Play()
        {

        }

        /// <summary> ���� </summary>
        public void Stop()
        {

        }
    }

    #endregion ������ Ŭ����
}
