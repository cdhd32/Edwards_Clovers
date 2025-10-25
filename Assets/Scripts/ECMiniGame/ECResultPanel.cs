using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EResultState
{
    Perfect, Great, Good, Bad, Count
}
public class ECResultPanel : MonoBehaviour
{
    public TextMeshProUGUI result;
    public GameObject go;

    public void ShowResult(EResultState state)
    {
        go.SetActive(true);
        result.text = state.ToString();
        //1초뒤 사라지기??
    }

    public void OnClick_OKButton()
    {
        Debug.Log("메인으로");
        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
    }
}
