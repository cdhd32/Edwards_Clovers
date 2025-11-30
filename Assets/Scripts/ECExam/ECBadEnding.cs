using UnityEngine;

public class ECBadEnding : MonoBehaviour
{
    public GameObject panel;
    public GameObject anim;
    private void Awake()
    {
        int val = PlayerPrefs.GetInt("examResult");
        if (val == 1)
        {
            panel.SetActive(true);
            anim.SetActive(false);
            //베드엔딩
        }
        else
        {
            panel.SetActive(false);
            anim.SetActive(true);
        }
    }

    public void OnClickBtn_Replay()
    {
        //다시 시작하기
        ECPlayerStatManager.Instance.DeleteStatData();
        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
    }
}
