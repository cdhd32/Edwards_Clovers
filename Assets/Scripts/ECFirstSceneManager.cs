using UnityEngine;

public class ECFirstSceneManager : MonoBehaviour
{
    void Awake()
    {
        //전역 매니저 초기화
        ECPlayerStatManager.Instance.Init();
        PlayerPrefs.SetInt("examResult", -1);
        PlayerPrefs.SetInt("state", 0);
    }
}
