using UnityEngine;

public class ECFirstSceneManager : MonoBehaviour
{
    void Awake()
    {
        //전역 매니저 초기화
        ECPlayerStatManager.Instance.Init();
    }

    void Start()
    {
        //첫 씬에서 바로 메인 씬으로 전환
        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
    }
}
