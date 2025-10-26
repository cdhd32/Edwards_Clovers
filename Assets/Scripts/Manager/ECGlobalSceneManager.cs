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
    LUCKY,
    EXAM,
    ENDING
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
        "ECMiniGame_LuckyScene",
        "ECExamScene",
        "EndingScene"
    };

    public void LoadScene(SceneType sceneType)
    {
        // Implementation for loading a scene
        Debug.Log($"Loading scene: {sceneType.ToString()}");

        SceneManager.LoadScene(sceneNames[(int)sceneType]);
    }


    private bool ExamEventCheck(int leftDay)
    {
        if (leftDay == 4 || leftDay == 1 || leftDay == 0)
        {
            return true;

            //단원 평가를 봐야해요
        }
        return false;

    }
}
