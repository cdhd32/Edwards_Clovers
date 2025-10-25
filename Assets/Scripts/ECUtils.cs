using System;
using System.Collections.Generic;
using UnityEngine;

public class ECUtils
{
    //랭크 상한 하한 수치
    //private static readonly Dictionary<string, (int, int)> rankLimits = new Dictionary<string, (int, int)>()
    //{
    //    {"S", (800, 1000)},
    //    {"A+", (600, 799)},
    //    {"A", (450, 599)},
    //    {"B+", (300, 449)},
    //    {"B", (200, 299)},
    //    {"C+", (150, 199)},
    //    {"C", (100, 149)},
    //    {"D+", (50, 99)},
    //    {"D", (0, 49)}
    //};

    private static readonly Dictionary<PlayerStatType, string> statusNames = new Dictionary<PlayerStatType, string>()
    {
        {PlayerStatType.MOT, "의욕"},
        {PlayerStatType.KOR, "국어"},
        {PlayerStatType.ENG, "영어"},
        {PlayerStatType.MATH, "수학"},
        {PlayerStatType.SCI, "과학"},
        {PlayerStatType.LUK, "운"},
        {PlayerStatType.LEFTDAY, "남은 기간"},
        {PlayerStatType.CLASS, "교시"}
    };

    //
    public static string GetRankString(int num)
    {
        int k = num / 75;
        switch (k)
        {
            case 0:
                return "D";
            case 1:
                return "D+";
            case 2:
                return "C";
            case 3:
                return "C+";
            case 4:
                return "B";
            case 5:
                return "B+";
            case 6:
                return "A";
            case 7:
                return "A+";
            case 8:
                return "S";
        }


        return "S";
        if (num >= 800 && num <= 1000) return "S";
        else if (num >= 600 && num <= 799) return "A+";
        else if (num >= 450 && num <= 599) return "A";
        else if (num >= 300 && num <= 449) return "B+";
        else if (num >= 200 && num <= 299) return "B";
        else if (num >= 150 && num <= 199) return "C+";
        else if (num >= 100 && num <= 149) return "C";
        else if (num >= 50 && num <= 99) return "D+";
        else if (num >= 0 && num <= 49) return "D";
        else return "N";
    }

    //다음 랭크까지 남은 수치 백분율로 반환
    public static float GetNextRankPercent(int num)
    {
        if (num >= 800 && num <= 1000) return 1.0f;
        else if (num >= 600 && num <= 799) return (num - 600) / 200.0f;
        else if (num >= 450 && num <= 599) return (num - 450) / 150.0f;
        else if (num >= 300 && num <= 449) return (num - 300) / 150.0f;
        else if (num >= 200 && num <= 299) return (num - 200) / 100.0f;
        else if (num >= 150 && num <= 199) return (num - 150) / 50.0f;
        else if (num >= 100 && num <= 149) return (num - 100) / 50.0f;
        else if (num >= 50 && num <= 99) return (num - 50) / 50.0f;
        else if (num >= 0 && num <= 49) return num / 50.0f;
        else return 0.0f;
    }

    public static string GetClassString(int num)
    {
        if (num == 1)
            return "1st Class";
        else if (num == 2)
            return "2nd Class";
        else if (num == 3)
            return "3rd Class";
        else if (num == 4)
            return "4th Class";
        else
            return "Unknown Class";
    }

    public static string GetDDayString(int leftDays)
    {
        if (leftDays == 0)
            return "D-Day";
        else
            return "D-" + leftDays.ToString();

    }

    public static string GetStatusName(PlayerStatType index)
    {
        if (statusNames.TryGetValue(index, out string name))
        {
            return name;
        }
        else
        {
            return string.Empty;
        }
    }

    public static int GetEventIndex(EventType eventType, ConditionType conditionType)
    {
        return (int)ConditionType._MAX * (int)eventType + (int)conditionType;
    }
}
