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

    [Header("[����]")]
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

    /// <summary>Ȯ�� ��ư</summary>
    public void OnClickConfirm()
    {
        //���� ����
        manager.bGM = bGM.slider.value;
        manager.uiSound = uiSound.slider.value;
        manager.effectSount = effectSound.slider.value;

        //�ɼ� ����
        manager.Save();

        Close();
    }

    public override void OnClickClose()
    {
        base.OnClickClose();

        //���� �ʱ�ȭ
        bGM.SetOption(manager.bGM);
        uiSound.SetOption(manager.uiSound);
        effectSound.SetOption(manager.effectSount);
    }
}
