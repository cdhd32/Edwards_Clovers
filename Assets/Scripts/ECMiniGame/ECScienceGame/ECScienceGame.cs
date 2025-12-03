using UnityEngine;

public class ECScienceGame : ECMiniGameBase
{
    public ECTiltPourHandle handle;
    public ECLiquidSpawner liquidSpawner;
    public Camera cam;
    private void Awake()
    {
    }

    private void Start()
    {
        base.StartGame();   
    }


    public override EResultState GetScore()
    {
        EResultState state = liquidSpawner.ReturnGameResult();
        return state;
    }


}
