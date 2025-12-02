using UnityEngine;

public class ECMainSceneManager : ECSingleton<ECMainSceneManager>
{
    private ECPlayerStatManager playerStatManager;
    protected new void Awake()
    {
        base.Awake();

        playerStatManager = ECPlayerStatManager.Instance;
        playerStatManager.LoadStatData();
    }

    public int GetLeftDayNum()
    {
        return playerStatManager.GetPlayerStat(PlayerStatType.LEFTDAY);
    }

    public int GetClassNum()
    {
        return playerStatManager.GetPlayerStat(PlayerStatType.CLASS);
    }

    public string GetStautsName(PlayerStatType type)
    {
        return playerStatManager.GetPlayerStatName(type);
    }

    public int GetStatusNum(PlayerStatType type)
    {
        return playerStatManager.GetPlayerStat((int)type);
    }

    public int GetStatusNumPriv(PlayerStatType type)
    {
        return playerStatManager.GetPlayerStatPriv((int)type);
    }

    public int GetStatusNum(int index)
    {
        return playerStatManager.GetPlayerStat(index);
    }

    public int GetMotivation()
    {
        return playerStatManager.GetPlayerStat(PlayerStatType.MOT);
    }
}
