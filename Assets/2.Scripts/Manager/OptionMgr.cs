using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMgr : MgrBase
{
    public static OptionMgr instance = null;

    /// <summary>배경 음악 사운드</summary>
    public float bGM = 0;

    /// <summary>UI 관련 사운드</summary>
    public float uiSound = 0;

    /// <summary>효과음</summary>
    public float effectSount = 0;

    public override void Init()
    {
        //1.인스턴스 생성
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //2. 옵션 기본 세팅
        bool isSave = false;

        //백그라운드 사운드
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

        //UI 관련 이펙트 사운드
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

        //게임 내 이펙트 사운드
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

        //프리팹스에 변경된 사항이 생긴 경우 저장
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
