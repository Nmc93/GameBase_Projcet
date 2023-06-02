using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GEnum;

public class PoolMgr : MgrBase
{
    public static PoolMgr instance = null;

    /// <summary>UI를 저장하는 딕셔너리</summary>
    private Dictionary<eUIPrefab , GameObject> uiBasePool = new Dictionary<eUIPrefab, GameObject>();

    [SerializeField]
    private List<eUIPrefab> inUIPoolList = new List<eUIPrefab>();

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
    }

    /// <summary>사용하지 않는 UI를 풀에 저장하는 함수</summary>
    /// <returns>이미 UI가 풀에 있다면 false를 반환 </returns>
    public bool SetUIPool(eUIPrefab Type , GameObject uiObject)
    {
        //해당 UI가 딕셔너리에 없을 경우
        if(!uiBasePool.ContainsKey(Type))
        {
            //UI를 풀에 추가
            uiBasePool.Add(Type, uiObject);
            //테스트용
            inUIPoolList.Add(Type);
            //풀에 들어왔으니 비활성화
            uiBasePool[Type].SetActive(false);

            return true;
        }

        //이미 풀에 있어서 저장 실패
        return false;
    }


    /// <summary> 풀에 있는 UI를 가져오고 성공 여부를 반환하는 함수 </summary>
    /// <returns> 풀에 없을 경우 false를 반환 </returns>
    public bool GetUIPool(eUIPrefab Type, out GameObject uiObject)
    {
        //해당 UI가 딕셔너리에 있을 경우
        if (uiBasePool.TryGetValue(Type, out uiObject))
        {
            //더 이상 풀에 있을 필요가 없으므로 풀에서 제거
            uiBasePool.Remove(Type);

            //테스트용
            for (int i = 0; i < inUIPoolList.Count; ++i)
            {
                if (inUIPoolList[i] == Type)
                {
                    inUIPoolList.RemoveAt(i);
                    break;
                }
            }

            return true;
        }

        //해당 UI가 풀에 없음
        return false;
    }

    /// <summary> 풀에 있는 UI를 반환하는 함수 </summary>
    /// <returns> 풀에 없을 경우 null을 반환 </returns>
    public GameObject GetUIPool(eUIPrefab Type)
    {
        GameObject uiObject = null;

        //해당 UI가 딕셔너리에 있을 경우
        if (uiBasePool.TryGetValue(Type, out uiObject))
        {
            //더 이상 풀에 있을 필요가 없으므로 풀에서 제거
            uiBasePool.Remove(Type);

            //테스트용
            for (int i = 0; i < inUIPoolList.Count; ++i)
            {
                if (inUIPoolList[i] == Type)
                {
                    inUIPoolList.RemoveAt(i);
                    break;
                }
            }

            return uiObject;
        }

        return null;
    }
}
