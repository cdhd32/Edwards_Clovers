using UnityEngine;

public class ECMiniGameBase : MonoBehaviour
{
    public ECMiniGameTimer timer;
    public virtual void StartGame()
    {
        timer.StartTimer();
    }

}
