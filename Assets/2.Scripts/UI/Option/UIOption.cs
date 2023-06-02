using GEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIBase
{
    protected override eUIPrefab PrefabType { get => eUIPrefab.OptionPopup; }

    [System.Serializable]
    public class SoundOption
    {
        public Text text = null;
        public Slider slider = null;

        public void SetOption(float value)
        {
            slider.value = value;
        }
    }

    [Header("[사운드]")]
    public SoundOption bGM;
    public SoundOption uiSound;
    public SoundOption effectSound;

    private OptionMgr manager = OptionMgr.instance;

    public override void Init()
    {
        bGM.SetOption(manager.bGM);
        uiSound.SetOption(manager.uiSound);
        effectSound.SetOption(manager.effectSount);
    }

    /// <summary>확인 버튼</summary>
    public void OnClickConfirm()
    {
        //사운드 저장
        manager.bGM = bGM.slider.value;
        manager.uiSound = uiSound.slider.value;
        manager.effectSount = effectSound.slider.value;

        //옵션 저장
        manager.Save();

        Close();
    }

    public override void OnClickClose()
    {
        base.OnClickClose();

        //사운드 초기화
        bGM.SetOption(manager.bGM);
        uiSound.SetOption(manager.uiSound);
        effectSound.SetOption(manager.effectSount);
    }
}
