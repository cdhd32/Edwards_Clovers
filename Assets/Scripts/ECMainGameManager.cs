using UnityEngine;

public class ECMainSceneManager : ECSingleton<ECMainSceneManager>
{
 
    public int GetLeftDayNum()
    {
        return 2;
    }

    public int GetClassNum()
    {
        return 2;
    }

    public string GetStautsName(int index)
    {
        return ECUtils.GetStatusName(index);
    }

    public int GetStatusNum(int index)
    {
        return 333;
    }

    public int GetHP()
    {
        return 77;
    }
}
