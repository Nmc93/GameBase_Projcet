using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMgr : MgrBase
{
    public static OptionMgr instance = null;

    /// <summary>��� ���� ����</summary>
    public float bGM = 0;

    /// <summary>UI ���� ����</summary>
    public float uiSound = 0;

    /// <summary>ȿ����</summary>
    public float effectSount = 0;

    public override void Init()
    {
        //1.�ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //2. �ɼ� �⺻ ����
        bool isSave = false;

        //��׶��� ����
        if (PlayerPrefs.HasKey("BGM"))
        {
            bGM = PlayerPrefs.GetFloat("BGM");
        }
        else
        {
            bGM = 1f;
            PlayerPrefs.SetFloat("BGM", bGM);
            isSave = true;
        }

        //UI ���� ����Ʈ ����
        if (PlayerPrefs.HasKey("UISound"))
        {
            uiSound = PlayerPrefs.GetFloat("UISound");
        }
        else
        {
            uiSound = 1f;
            PlayerPrefs.SetFloat("UISound", uiSound);
            isSave = true;
        }

        //���� �� ����Ʈ ����
        if (PlayerPrefs.HasKey("EffectSount"))
        {
            effectSount = PlayerPrefs.GetFloat("EffectSount");
        }
        else
        {
            effectSount = 1f;
            PlayerPrefs.SetFloat("EffectSount", effectSount);
            isSave = true;
        }

        //�����ս��� ����� ������ ���� ��� ����
        if(isSave)
        {
            PlayerPrefs.Save();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("BGM", bGM);
        PlayerPrefs.SetFloat("UISound", uiSound);
        PlayerPrefs.SetFloat("EffectSount", effectSount);

        PlayerPrefs.Save();
    }
}
