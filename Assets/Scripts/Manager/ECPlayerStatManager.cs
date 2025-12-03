using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;

[Serializable]
public class PlayerStatData
{
    public string dataName;
    public int data;
}

[Serializable]
public class BehaviorEventData
{
    public int korStat;
    public int engStat;
    public int mathStat;
    public int sciStat;
    public int lukStat;
}

public enum EventType : int
{
    KOR,
    ENG,
    MATH,
    SCI,
    LUK,
    CHEER,
    _MAX
}

public enum  ConditionType : int
{
    BAD,
    GOOD,
    GREAT,
    PERFECT,
    _MAX
}

public enum PlayerStatType
{
    MOT = 0, //Motivation
    KOR = 1, //Korean
    ENG = 2, //English
    MATH = 3, //Mathematics
    SCI = 4, //Science
    LUK = 5, //Luck
    LEFTDAY = 6, //Left Day
    CLASS = 7, //Class Progress
    _MAX
}

//전역 매니저
public class ECPlayerStatManager : ECSingletonDontDestroy<ECPlayerStatManager>
{
    [NonSerialized] public PlayerStatData[] playerStats;

    private int[] playerDataPriv = null;

    private BehaviorEventData[] behaviorEventData;
    private bool[] tooltipData = new bool[(int)PlayerStatType._MAX];

    [NonSerialized] public bool isFirstLoad = false;

    public void Init()
    {
        LoadEventData();
    }

    public void LoadStatData()
    {
        //Application.persistentDataPath 에서 json 파일 불러오기
        var path = Application.persistentDataPath + "/playerStat.json";
        if (File.Exists(path))
        {
            string plainString = File.ReadAllText(path);

            //파일이 비어있으면 Resources에서 초기화 데이터를 불러온다
            if (string.IsNullOrEmpty(plainString) || plainString.Equals("{}"))
            {
                var textAsset = Resources.Load<TextAsset>("playerStatInit");

                if (textAsset != null)
                {
                    plainString = textAsset.text;

                    playerStats = JSONHelper.FromJson<PlayerStatData>(plainString);
                    
                    if (!isFirstLoad)
                        SavePrivStatData();
                    
                }
            }
            else
            {
                playerStats = JSONHelper.FromJson<PlayerStatData>(plainString);

                if (!isFirstLoad)
                    SavePrivStatData();
                
            }

            //Debug.Log($"PlayerStatManager.LoadStatData() Data Loaded");
        }
        else
        {
            var textAsset = Resources.Load<TextAsset>("playerStatInit");

            if (textAsset != null)
            {
                //파일이 없으면 새로 생성
                playerStats = JSONHelper.FromJson<PlayerStatData>(textAsset.text);
                
                if (!isFirstLoad)
                    SavePrivStatData();
            }
            
            SaveStatData();
            //Debug.Log($"PlayerStatManager.LoadStatData() Data Created");
        }

        if (!isFirstLoad)
            isFirstLoad = true;
    }

    public void SaveStatData()
    {
        //Application.persistentDataPath 에서 json 파일 불러오기
        var path = Application.persistentDataPath + "/playerStat.json";

        string plainString = JSONHelper.ToJson<PlayerStatData>(playerStats);

        File.WriteAllText(path, plainString);

        //Debug.Log($"PlayerStatManager.SaveStatData() Data Saved");
    }

    //수정 전 스탯 데이터 저장
    private void SavePrivStatData()
    {
        if (playerDataPriv == null)
            playerDataPriv = new int[(int)PlayerStatType._MAX];

        for (int i = 0; i < (int)PlayerStatType._MAX; i++)
        {
            if (playerStats != null)
                playerDataPriv[i] = playerStats[i].data;
            else
                playerDataPriv[i] = 0;
        }
    }

    public void DeleteStatData()
    {
        var path = Application.persistentDataPath + "/playerStat.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            isFirstLoad = false;
            //Debug.Log($"PlayerStatManager.ClearStatData() Data Cleared");
        }
    }

    public void LoadEventData()
    {
        //Application.persistentDataPath 에서 json 파일 불러오기
        var textAsset = Resources.Load<TextAsset>("behaviorEventData");
        const int typeMax = (int)EventType._MAX * (int)ConditionType._MAX;

        if (textAsset != null)
        {
            string plainString = textAsset.text;

            //파일이 비어있으면 새로 생성
            if (string.IsNullOrEmpty(plainString) || plainString.Equals("{}"))
            {
                behaviorEventData = new BehaviorEventData[typeMax];
                for (int i = 0; i < typeMax; i++)
                {
                    behaviorEventData[i] = new BehaviorEventData();
                    behaviorEventData[i].korStat = 150;
                    behaviorEventData[i].engStat = 150;
                    behaviorEventData[i].mathStat = 150;
                    behaviorEventData[i].sciStat = 150;
                    behaviorEventData[i].lukStat = 150;
                }
            }
            else
            {
                behaviorEventData = JSONHelper.FromJson<BehaviorEventData>(plainString);
            }
        }
        else
        {
            //파일이 없으면 새로 생성
            behaviorEventData = new BehaviorEventData[typeMax];
            for (int i = 0; i < typeMax; i++)
            {
                behaviorEventData[i] = new BehaviorEventData();
                behaviorEventData[i].korStat = 150;
                behaviorEventData[i].engStat = 150;
                behaviorEventData[i].mathStat = 150;
                behaviorEventData[i].sciStat = 150;
                behaviorEventData[i].lukStat = 150;
            }
        }

        //Debug.Log($"PlayerStatManager.LoadEventData() Data Created");
    }

    public void SaveEventData()
    {
        //Application.persistentDataPath 에서 json 파일 불러오기
        var path = Application.persistentDataPath + "/behaviorEventData.json";

        string plainString = JSONHelper.ToJson<BehaviorEventData>(behaviorEventData);

        File.WriteAllText(path, plainString);

        //Debug.Log($"PlayerStatManager.SaveEventData() Data Saved");
    }

    private void AddPlayerStat(PlayerStatType type, int amount, bool isSave = false)
    {
        AddPlayerStat((int)type, amount, isSave);
    }

    private void AddPlayerStat(int index, int amount, bool isSave = false)
    {
        if (index < 0 || index >= (int)PlayerStatType._MAX)
            return;

        playerDataPriv[index] = playerStats[index].data;

        playerStats[index].data += amount;
        if (playerStats[index].data < 0)
            playerStats[index].data = 0;

        if (isSave)
            SaveStatData();

        //Debug.Log($"PlayerStatManager.AddPlayerStat() {playerStats[index].dataName} Changed : {playerStats[index].data}");
    }

    private void SetPlayerStat(PlayerStatType type, int amount, bool isSave = false)
    {
        SetPlayerStat((int)type, amount, isSave);
    }

    private void SetPlayerStat(int index, int amount, bool isSave = false)
    {
        if (index < 0 || index >= (int)PlayerStatType._MAX)
            return;

        playerDataPriv[index] = playerStats[index].data;

        playerStats[index].data = amount;

        if (playerStats[index].data < 0)
            playerStats[index].data = 0;

        if (isSave)
            SaveStatData();
        Debug.Log($"PlayerStatManager.SetPlayerStat() {playerStats[index].dataName} Set : {playerStats[index].data}");
    }

    public void SetPlayerStatByEvent(EventType eventType, ConditionType conditionType)
    {
        if (eventType == EventType.CHEER)
        {
            AddPlayerStat(PlayerStatType.KOR, 0);
            AddPlayerStat(PlayerStatType.ENG, 0);
            AddPlayerStat(PlayerStatType.MATH, 0);
            AddPlayerStat(PlayerStatType.SCI, 0);
            AddPlayerStat(PlayerStatType.LUK, 0);
        }
        else
        {
            int index = ECUtils.GetEventIndex(eventType, conditionType);
            AddPlayerStat(PlayerStatType.KOR, (int)(behaviorEventData[index].korStat * UnityEngine.Random.Range(1.25f, 1.33f)));
            AddPlayerStat(PlayerStatType.ENG, (int)(behaviorEventData[index].engStat * UnityEngine.Random.Range(1.25f, 1.33f)));
            AddPlayerStat(PlayerStatType.MATH, (int)(behaviorEventData[index].mathStat * UnityEngine.Random.Range(1.25f, 1.33f)));
            AddPlayerStat(PlayerStatType.SCI, (int)(behaviorEventData[index].sciStat * UnityEngine.Random.Range(1.25f, 1.33f)));
            AddPlayerStat(PlayerStatType.LUK, (int)(behaviorEventData[index].lukStat * UnityEngine.Random.Range(1.25f, 1.33f)));
        }

        SaveStatData();
    }

    public int GetPlayerStat(PlayerStatType statType)
    {
        return GetPlayerStat((int)statType);
    }

    public int GetPlayerStat(int index)
    {
        if (index < 0 || index >= (int)PlayerStatType._MAX)
        {
            //Debug.Log($"PlayerStatManager.GetPlayerStat() Invailed Index");
            return -1;
        }

        //Debug.Log($"PlayerStatManager.GetPlayerStat() {playerStats[index].dataName} : {playerStats[index].data}");

        return playerStats[index].data;
    }

    public int GetplayerStatPriv(PlayerStatType statType)
    {
        return GetPlayerStatPriv((int)statType);
    }

    //수정 직전 스탯 데이터 반환
    public int GetPlayerStatPriv(int index)
    {
        if (index < 0 || index >= (int)PlayerStatType._MAX)
        {
            //Debug.Log($"PlayerStatManager.GetPlayerStat() Invailed Index");
            return -1;
        }

        return playerDataPriv[index];
    }


    public string GetPlayerStatName(PlayerStatType statType)
    {
        return GetPlayerStatName((int)statType);
    }

    public string GetPlayerStatName(int index)
    {
        if (index < 0 || index >= (int)PlayerStatType._MAX)
        {
            //Debug.Log($"PlayerStatManager.GetPlayerStatName() Invailed Index");
            return string.Empty;
        }
        //Debug.Log($"PlayerStatManager.GetPlayerStatName() {playerStats[index].dataName}");
        return playerStats[index].dataName;
    }

    //행동 이벤트 후 스탯 업데이트
    public void UpdateStat(EventType eventType, ConditionType conditionType)
    {
        //의욕 게이지 증가/차감
        if (eventType == EventType.CHEER)
        {
            //행운 이벤트일 때는 의욕 게이지 최대치로 변경
            AddPlayerStat(PlayerStatType.MOT, ECConst.MOTVIATION_MAX);
        }
        else
        {
            //그 외에는 의욕 게이지 차감 및 이벤트 스탯 증가
            AddPlayerStat(PlayerStatType.MOT, -ECConst.MOTVIATION_PAY);
        }

        SetPlayerStatByEvent(eventType, conditionType);

        //1교시 증가
        AddPlayerStat(PlayerStatType.CLASS, 1);

        //4교시가 지나면 하루 차감, 1교시로 초기화
        if (playerStats[(int)PlayerStatType.CLASS].data > ECConst.CLASS_PER_DAY)
        {
            playerStats[(int)PlayerStatType.CLASS].data = 1;
            AddPlayerStat(PlayerStatType.LEFTDAY, -1); //남은 일수 감소
        }

        SaveStatData();
    }

    public void UpdateTimeStat()
    {
        //1교시 증가
        AddPlayerStat(PlayerStatType.CLASS, 1);

        //4교시가 지나면 하루 차감, 1교시로 초기화
        if (playerStats[(int)PlayerStatType.CLASS].data > ECConst.CLASS_PER_DAY)
        {
            playerStats[(int)PlayerStatType.CLASS].data = 1;
            AddPlayerStat(PlayerStatType.LEFTDAY, -1); //남은 일수 감소
        }

        SaveStatData();
    }

    //의욕 충전 후 스탯 업데이트
    public void UpdateStat_EndExam()
    {
        playerStats[(int)PlayerStatType.CLASS].data = ECConst.CLASS_PER_DAY;
        AddPlayerStat(PlayerStatType.LEFTDAY, -1); //남은 일수 감소
        SaveStatData();
    }

    //의욕 충전 후 스탯 업데이트
    public void UpdateStatCheer()
    {
        //의욕 게이지 최대치로 변경
        SetPlayerStat(PlayerStatType.MOT, ECConst.MOTVIATION_MAX);
        UpdateTimeStat();
    }

    public string GetLowestStatSubject()
    {
        PlayerStatData lowest = playerStats[1];

        List<PlayerStatData> data = ListPool<PlayerStatData>.Get();
        //운 전까지만ㅈ
        for (int i = 1; i < 5; i++)
        {
            if (playerStats[i].data <= lowest.data)
            {
                if (i != (int)PlayerStatType.LUK)
                {
                    data.Add(playerStats[i]);
                }
            }
        }

        lowest = data[UnityEngine.Random.Range(0, data.Count)];

        return lowest.dataName;
    }

    public void GoNextTurn(EventType resultState, ConditionType conditionType = ConditionType.BAD)
    {
        int leftDayVal = GetPlayerStat(PlayerStatType.LEFTDAY);
        int classVal = GetPlayerStat(PlayerStatType.CLASS);
        //Debug.Log("남은 날 :" + leftDayVal + "교시" + classVal);

        //마지막 교시거나 d - day일 때
        if (leftDayVal == ECConst.UNIT_TEST_DAY_1 ||
            leftDayVal == ECConst.MIDTERM_DAY)
        {
            if (classVal == ECConst.CLASS_PER_DAY)
            {
                UpdateStat(resultState, conditionType);
                ECGlobalSceneManager.Instance.LoadScene(SceneType.EXAM);
                return;
            }

            if (leftDayVal == 0)
            {
                UpdateStat(resultState, conditionType);
                ECGlobalSceneManager.Instance.LoadScene(SceneType.EXAM);
                return;
            }
        }

        UpdateStat(resultState, conditionType);
        //응원 이벤트인 경우 말풍선 상태 초기화
        if (resultState == EventType.CHEER)
            PlayerPrefs.SetInt("state", 0);
        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
    }

    public void PlayMiniGame(PlayerStatType type)
    {
        tooltipData[(int)type] = true;
    }

    public bool GetPlayMiniGame(PlayerStatType type)
    {
        bool d = tooltipData[(int)type];
        return d;
    }


}
