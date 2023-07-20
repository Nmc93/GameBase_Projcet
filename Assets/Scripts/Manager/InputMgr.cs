using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GEnum;

public class InputMgr : MgrBase
{
    public static InputMgr instance;

    [Serializable]
    public class ClickData
    {
        public ClickData(KeyCode key)
        {
            this.key = key;
        }

        /// <summary> 키를 누르고 있는지 여부 </summary>
        public bool isClickKey;
        /// <summary> 터치키 </summary>
        public KeyCode key;
        /// <summary> 터치 이벤트 등록 </summary>
        public HashSet<Action> Actions = new HashSet<Action>();
    }

    /// <summary> 사용하는 키 목록 [상황에 따라 데이터가 삭제되고 추가됨]</summary>
    private static List<ClickData> keyList = new List<ClickData>();
    /// <summary> 기능에 할당된 키 확인 [데이터가 삭제되는일은 없어야함] </summary>
    private static Dictionary<eInputType, KeyCode?> dicInUseData = new Dictionary<eInputType, KeyCode?>();

    #region 최초 세팅
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        //최초 키 세팅
        KeySetting();
    }

    /// <summary> 저장되어 있는 상태로 키 세팅 </summary>
    private void KeySetting()
    {
        //키 저장시 필요한것
        // eInputType       - .Tostring()은 테이블과 프랩스의 키값과 동일
        // InputKeyTable    - 디폴트 키 세팅 정보
        // PlayerPrefs      - 변경한 키 데이터 저장, 데이터가 없다면 테이블값으로 저장및 사용
        // InputKeyTable과 PlayerPrefs에 저장된 값은 KeyCode.*.ToString()과 같음
        // 지정되지 않아 사용하지 않는 기능은 값을 Null로 저장

        //키 세팅
        int count = (int)eInputType.Count;
        for (int i = 0; i < count; ++i)
        {
            eInputType type = (eInputType)i;
            string typeName = type.ToString();

            //키가 없을 경우 디폴트값의 키 생성 및 저장
            if (!PlayerPrefs.HasKey(typeName))
            {
                PlayerPrefs.SetString(typeName, TableMgr.Get<InputKeyTableData>(typeName).KeyString);
                PlayerPrefs.Save();
            }

            //해당 키 세팅
            KeyCode? code = ConvertStringToKeyCode(PlayerPrefs.GetString(typeName));
            dicInUseData.Add(type, code);
        }
    }
    #endregion 최초 세팅

    #region 이벤트 검사

    private void Update()
    {
        // 입력이 없다면 종료
        if (!Input.anyKey)
        {
            return;
        }

        //키 검사
        foreach(var data in keyList)
        {
            // 1. 첫번째 버튼 다운일 경우
            if (!data.isClickKey)
            {
                // 1-1. 눌린키가 지정된 키일 경우거나 None타입일 경우(None은 AnyKey로 사용함)
                if (Input.GetKeyDown(data.key) || data.key == KeyCode.None)
                {
                    //클릭 체크
                    data.isClickKey = true;

                    //실행할 수 있는 터치 이벤트가 있을 경우 실행
                    if (data.Actions.Count > 0)
                    {
                        foreach (var action in data.Actions)
                        {
                            action();
                        }
                    }
                }
            }
            // 2. 버튼 다운을 지속중일 경우
            else
            {
                // 2-1. 버튼 업 이벤트일 경우
                if (Input.GetKeyUp(data.key))
                {
                    //클릭 해제
                    data.isClickKey = true;
                }
            }
        }
    }
    
    #endregion 이벤트 검사

    #region 키 이벤트 세팅

    /// <summary> 키 클릭 이벤트 등록 </summary>
    public static void AddKeyEvent(eInputType type, Action callback)
    {
        //타입에 지정된 키 검색
        if(dicInUseData.TryGetValue(type, out KeyCode? code) && code != null)
        {
            //목록에 데이터가 없을 경우 데이터 세팅
            ClickData data = keyList.Find(item => item.key == code.Value);
            if (data == null)
            {
                data = new ClickData(code.Value);
                keyList.Add(data);
            }

            //이벤트 등록
            if (!data.Actions.Add(callback))
            {
                Debug.LogError($"{type} 이벤트가 중복 저장되고 있습니다.");
            }
        }
        else
        {
            Debug.LogError($"{type}타입의 데이터를 찾을 수 없습니다.");
        }
    }

    /// <summary> 키 클릭 이벤트 해제 </summary>
    public static void RemoveKeyEvent(eInputType type, Action callback)
    { 
        // 타입에 지정된 키 검색
        if (dicInUseData.TryGetValue(type, out KeyCode? code) && code != null)
        {
            // 지정된 키가 있을 경우 이벤트 제거
            ClickData data = keyList.Find(item => item.key == code.Value);
            if (data != null)
            {
                // 등록된 이벤트일 경우
                if (data.Actions.Contains(callback))
                {
                    //이벤트 해제
                    data.Actions.Remove(callback);

                    //모든 이벤트가 해제됐을 경우 해당 클릭 이벤트 제거
                    if(data.Actions.Count <= 0)
                    {
                        keyList.Remove(data);
                    }
                }
                // 등록되지 않은 이벤트일 경우
                else
                {
                    Debug.LogError($"삭제할 {type}타입의 데이터를 찾을 수 없습니다.");
                }
            }
            //지정된 키가 없을 경우
            else
            {
                Debug.LogError($"{type}에 지정된 키가 없습니다.");
            }
        }
        //해당 타입에 관련된 데이터가 없을 경우
        else
        {
            Debug.LogError($"{type}타입의 데이터를 찾을 수 없습니다.");
        }
    }

    #endregion 키 이벤트 세팅

    #region 변환
    /// <summary> string을 KeyCode로 변환 </summary>
    /// <param name="key"> KeyCode의 항목과 이름이 동일해야함</param>
    /// <returns> 찾을 수 없다면 null 반환 </returns>
    private KeyCode? ConvertStringToKeyCode(string key)
    {
        if(key == "Null")
        {
            return null;
        }

        try
        {
            KeyCode code = (KeyCode)Enum.Parse(typeof(KeyCode), key);
            return code;
        }
        catch (Exception ex)
        {
            Debug.LogError($"KeyCode 변환 에러 : [{ex.Message}], Key : [{key}]");
            return null;
        }
    }
    #endregion 변환

}