using System;
using System.IO;
using System.Reflection;
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
    private PlayerStatData[] playerStats;

    private BehaviorEventData[] behaviorEventData;

    public void Init()
    {
        LoadStatData();
        LoadEventData();

        //SaveEventData();
    }

    public void LoadStatData()
    {
        //Application.persistentDataPath 에서 json 파일 불러오기
        var path = Application.persistentDataPath + "/playerStat.json";
        if (File.Exists(path))
        {
            string plainString = File.ReadAllText(path);

            //파일이 비어있으면 새로 생성
            if (string.IsNullOrEmpty(plainString) || plainString.Equals("{}"))
            {
                playerStats = new PlayerStatData[(int)PlayerStatType._MAX];
                for (int i = 0; i < (int)PlayerStatType._MAX; i++)
                {
                    playerStats[i] = new PlayerStatData();
                    playerStats[i].dataName = ECUtils.GetStatusName((PlayerStatType)i);
                    playerStats[i].data = 0;
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
            //파일이 없으면 새로 생성
            playerStats = new PlayerStatData[(int)PlayerStatType._MAX];
            for (int i = 0; i < (int)PlayerStatType._MAX; i++)
            {
                playerStats[i] = new PlayerStatData();
                playerStats[i].dataName = ECUtils.GetStatusName((PlayerStatType)i);
                playerStats[i].data = 0;
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

    public void LoadEventData()
    {
        //Application.persistentDataPath 에서 json 파일 불러오기
        var path = Application.persistentDataPath + "/behaviorEventData.json";
        const int typeMax = (int)PlayerStatType._MAX * (int)EventType._MAX;

        if (File.Exists(path))
        {
            string plainString = File.ReadAllText(path);

            //파일이 비어있으면 새로 생성
            if (string.IsNullOrEmpty(plainString) || plainString.Equals("{}"))
            {
                behaviorEventData = new BehaviorEventData[typeMax];
                for (int i = 0; i < typeMax; i++)
                {
                    behaviorEventData[i] = new BehaviorEventData();
                    behaviorEventData[i].korStat = 3;
                    behaviorEventData[i].engStat = 4;
                    behaviorEventData[i].mathStat = 5;
                    behaviorEventData[i].sciStat = 6;
                    behaviorEventData[i].lukStat = 7;
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
                behaviorEventData[i].korStat = 3;
                behaviorEventData[i].engStat = 4;
                behaviorEventData[i].mathStat = 5;
                behaviorEventData[i].sciStat = 6;
                behaviorEventData[i].lukStat = 7;
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

    public void SetPlayerStat(PlayerStatType type, int amount)
    {
        SetPlayerStat((int)type, amount);
    }

    public void SetPlayerStat(int index, int amount, bool isSave = false)
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

    public void SetPlayerStatByEvent(EventType eventType, ConditionType conditionType)
    {
        int index = ECUtils.GetEventIndex(eventType, conditionType);
        SetPlayerStat(PlayerStatType.KOR, behaviorEventData[index].korStat);
        SetPlayerStat(PlayerStatType.ENG, behaviorEventData[index].engStat);
        SetPlayerStat(PlayerStatType.MATH, behaviorEventData[index].mathStat);
        SetPlayerStat(PlayerStatType.SCI, behaviorEventData[index].sciStat);
        SetPlayerStat(PlayerStatType.LUK, behaviorEventData[index].lukStat);

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
            Debug.Log($"PlayerStatManager.GetPlayerStat() Invailed Index");
            return -1;
        }

        Debug.Log($"PlayerStatManager.GetPlayerStat() {playerStats[index].dataName} : {playerStats[index].data}");

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
            Debug.Log($"PlayerStatManager.GetPlayerStatName() Invailed Index");
            return string.Empty;
        }
        Debug.Log($"PlayerStatManager.GetPlayerStatName() {playerStats[index].dataName}");
        return playerStats[index].dataName;
    }
}
