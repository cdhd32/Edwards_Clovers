using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    FIRST,
    MAIN,
    CHEERUP,
    KR,
    EG,
    MATH,
    SCIENCE,
    LUCKY
}

public class ECGlobalSceneManager : ECSingletonDontDestroy<ECGlobalSceneManager>
{
    private string[] sceneNames = {
        "FirstScene",
        "MainScene",
        "ECMiniGame_CheerUp",
        "ECMiniGame_KRScene",
        "ECMiniGame_EGScene",
        "ECMiniGame_MathGame",
        "ECMiniGame_ScienceScene",
        "ECMiniGame_LuckyScene"
    };

    public void LoadScene(SceneType sceneType)
    {
        // Implementation for loading a scene
        Debug.Log($"Loading scene: {sceneType.ToString()}");

        SceneManager.LoadScene(sceneNames[(int)sceneType]);
    }
}
