using UnityEngine;

public class ECScienceGame : ECMiniGameBase
{
    public ECTiltPourHandle handle;

    private void Start()
    {
        base.StartGame();   
    }

    public override EResultState GetScore()
    {
        return base.GetScore();
    }
}
