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

    public void Init()
    {
        LoadStatData();
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
                SaveStatData();
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

    public void SetPlayerStat(PlayerStatType type, int amount)
    {
        SetPlayerStat((int)type, amount);
    }

    public void SetPlayerStat(int index, int amount)
    {
        if (index < 0 || index >= (int)PlayerStatType._MAX)
            return;

        playerStats[index].data += amount;
        if (playerStats[index].data < 0)
            playerStats[index].data = 0;

        SaveStatData();

        Debug.Log($"PlayerStatManager.SetPlayerStat() {playerStats[index].dataName} Changed : {playerStats[index].data}");
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
