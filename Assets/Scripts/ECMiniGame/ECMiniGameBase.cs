using System;
using UnityEngine;

public class ECMiniGameBase : MonoBehaviour
{
    public ECMiniGameTimer timer;
    [NonSerialized] public EResultState stageScore;
    public virtual void StartGame()
    {
        timer.StartTimer();
    }

    public virtual EResultState GetScore()
    {
        return stageScore;
    }

}
