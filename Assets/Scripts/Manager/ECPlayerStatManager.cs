using System;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

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

    private BehaviorEventData[] behaviorEventData;

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
                }
            }
            else
            {
                playerStats = JSONHelper.FromJson<PlayerStatData>(plainString);
            }

            Debug.Log($"PlayerStatManager.LoadStatData() Data Loaded");
        }
        else
        {
            var textAsset = Resources.Load<TextAsset>("playerStatInit");

            if (textAsset != null)
            {
                //파일이 없으면 새로 생성
                playerStats = JSONHelper.FromJson<PlayerStatData>(textAsset.text);
            }

            SaveStatData();
            Debug.Log($"PlayerStatManager.LoadStatData() Data Created");
        }
    }

    public void SaveStatData()
    {
        //Application.persistentDataPath 에서 json 파일 불러오기
        var path = Application.persistentDataPath + "/playerStat.json";

        string plainString = JSONHelper.ToJson<PlayerStatData>(playerStats);

        File.WriteAllText(path, plainString);

        Debug.Log($"PlayerStatManager.SaveStatData() Data Saved");
    }

    public void DeleteStatData()
    {
        var path = Application.persistentDataPath + "/playerStat.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"PlayerStatManager.ClearStatData() Data Cleared");
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
                    behaviorEventData[i].korStat = 0;
                    behaviorEventData[i].engStat = 0;
                    behaviorEventData[i].mathStat = 0;
                    behaviorEventData[i].sciStat = 0;
                    behaviorEventData[i].lukStat = 0;
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
                behaviorEventData[i].korStat = 0;
                behaviorEventData[i].engStat = 0;
                behaviorEventData[i].mathStat = 0;
                behaviorEventData[i].sciStat = 0;
                behaviorEventData[i].lukStat = 0;
            }
        }

        Debug.Log($"PlayerStatManager.LoadEventData() Data Created");
    }

    public void SaveEventData()
    {
        //Application.persistentDataPath 에서 json 파일 불러오기
        var path = Application.persistentDataPath + "/behaviorEventData.json";

        string plainString = JSONHelper.ToJson<BehaviorEventData>(behaviorEventData);

        File.WriteAllText(path, plainString);

        Debug.Log($"PlayerStatManager.SaveEventData() Data Saved");
    }

    private void AddPlayerStat(PlayerStatType type, int amount, bool isSave = false)
    {
        AddPlayerStat((int)type, amount, isSave);
    }

    private void AddPlayerStat(int index, int amount, bool isSave = false)
    {
        if (index < 0 || index >= (int)PlayerStatType._MAX)
            return;

        playerStats[index].data += amount;
        if (playerStats[index].data < 0)
            playerStats[index].data = 0;

        if (isSave)
            SaveStatData();

        Debug.Log($"PlayerStatManager.SetPlayerStat() {playerStats[index].dataName} Changed : {playerStats[index].data}");
    }

    private void SetPlayerStat(PlayerStatType type, int amount, bool isSave = false)
    {
        SetPlayerStat((int)type, amount, isSave);
    }

    private void SetPlayerStat(int index, int amount, bool isSave = false)
    {
        if (index < 0 || index >= (int)PlayerStatType._MAX)
            return;

        playerStats[index].data = amount;

        if (playerStats[index].data < 0)
            playerStats[index].data = 0;

        if (isSave)
            SaveStatData();
        //Debug.Log($"PlayerStatManager.SetPlayerStat() {playerStats[index].dataName} Set : {playerStats[index].data}");
    }

    public void SetPlayerStatByEvent(EventType eventType, ConditionType conditionType)
    {
        int index = ECUtils.GetEventIndex(eventType, conditionType);
        AddPlayerStat(PlayerStatType.KOR, behaviorEventData[index].korStat);
        AddPlayerStat(PlayerStatType.ENG, behaviorEventData[index].engStat);
        AddPlayerStat(PlayerStatType.MATH, behaviorEventData[index].mathStat);
        AddPlayerStat(PlayerStatType.SCI, behaviorEventData[index].sciStat);
        AddPlayerStat(PlayerStatType.LUK, behaviorEventData[index].lukStat);

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
        //의욕 게이지 차감
        AddPlayerStat(PlayerStatType.MOT, -ECConst.MOTVIATION_PAY);
        SetPlayerStatByEvent(eventType, conditionType);
        UpdateTimeStat();
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
        playerStats[(int)PlayerStatType.CLASS].data = 4;
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
        PlayerStatData lowest = playerStats[0];

        for (int i = 1; i < playerStats.Length; i++)
        {
            if (playerStats[i].data < lowest.data)
                lowest = playerStats[i];
        }

        return lowest.dataName;
    }
}
