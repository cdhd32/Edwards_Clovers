using UnityEngine;

public class ECScienceGame : ECMiniGameBase
{
    public ECTiltPourHandle handle;
    public ECLiquidSpawner liquidSpawner;
    public Camera cam;
    public SpriteRenderer renderTextureSR;
    private void Awake()
    {
        FixSpriteScale();
        //UpdateRenderTexture();
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

    void FixSpriteScale()
    {
        var rt = (RenderTexture)renderTextureSR.material.mainTexture;

        float targetWidth = rt.width;
        float unitsPerPixel = 1f / 100; // ∫∏≈Î 100

        float worldSize = targetWidth * unitsPerPixel;

        renderTextureSR.transform.localScale = new Vector3(worldSize, worldSize, 1f);
    }

}
