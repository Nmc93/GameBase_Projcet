using GEnum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{
    public override eCanvas canvasType => eCanvas.Page;

    public override eUI uiType => eUI.UILoading;

    /// <summary> �ε� ���� </summary>
    public enum eLoadingState
    {
        /// <summary> UI ���� ���� </summary>
        CloseCurScene,
        /// <summary> �� ���� ���� </summary>
        SceneChange,
        /// <summary> �� ���� ��� </summary>
        WaitChangeScene
    }

    #region �ν�����

    [Header("------------------ UI -----------------")]
    [Header("[�ε� �ؽ�Ʈ]"), Tooltip("[�ε� �ؽ�Ʈ]")]
    [SerializeField] private TextMeshProUGUI txt;
    [Header("[�ε� ��ũ�� ��]"), Tooltip("�ε� ��ũ�� ��")]
    [SerializeField] private Scrollbar scrollbar;

    #endregion �ν�����

    /// <summary> �ε� ���� ��ũ�� </summary>
    public float ScrollProgress
    {
        set => scrollbar.size = value;
    }

    /// <summary> �ؽ�Ʈ ����Ʈ </summary>
    private Coroutine textMove;
    /// <summary> UI�� ���� ���� </summary>
    private eLoadingState curState = eLoadingState.CloseCurScene;
    /// <summary> �ؽ�Ʈ�� ����� ���� </summary>
    private string moveString;

    public override void DataSetting()
    {
        base.DataSetting();

        //������ ���� ����
        ChangeState(eLoadingState.CloseCurScene);

        //�ڷ�ƾ ����
        textMove = StartCoroutine(TextMove());
    }

    public override void Close()
    {
        base.Close();

        //�ڷ�ƾ ����
        StopCoroutine(textMove);
    }

    /// <summary> �ε� ���� ���� </summary>
    /// <param name="state"></param>
    public void ChangeState(eLoadingState state)
    {
        curState = state;

        switch (curState)
        {
            case eLoadingState.CloseCurScene:
                moveString = TableMgr.Get<StringTableData>("Loading1").Text;
                scrollbar.gameObject.SetActive(false);
                break;
            case eLoadingState.SceneChange:
                moveString = TableMgr.Get<StringTableData>("Loading2").Text;
                scrollbar.gameObject.SetActive(true);
                break;
            case eLoadingState.WaitChangeScene:
                moveString = TableMgr.Get<StringTableData>("LoadingCom").Text;
                break;
        }
    }

    /// <summary> �ؽ�Ʈ �̵� �ڷ�ƾ </summary>
    IEnumerator TextMove()
    {
        string middleStr = string.Empty;
        while (true)
        {
            //�ؽ�Ʈ ����Ʈ
            if(middleStr.Length > 6)
                middleStr = $"{middleStr}.";
            else
                middleStr = string.Empty;

            txt.text = string.Format("{0}{1}", moveString, middleStr);
            yield return new WaitForSeconds(0.5f);

            //��ũ�ѹٸ� ���� ä���ų� ���� ���°� �� ���� ��� Ÿ���� ���
            if(scrollbar.size >= 1 || curState == eLoadingState.WaitChangeScene)
            {
                txt.text = string.Format("{0}{1}", moveString, ".");
                break;
            }
        }
    }
}