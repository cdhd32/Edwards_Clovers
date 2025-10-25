using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EResultState
{
    Bad, Good, Great, Perfect, Count
}
public class ECResultPanel : MonoBehaviour
{
    public TextMeshProUGUI result;
    public GameObject go;
    private ConditionType conditionType;
    private EventType eventType;
    public Sprite[] resultSprites; // 나중에 이미지로 변경하기
    public Image resultImage;

    public void ShowResult(EResultState state, EventType type)
    {
        go.SetActive(true);
        //result.text = state.ToString();
        resultImage.sprite = resultSprites[(int)state];
        eventType = type;
        conditionType = (ConditionType)state;
    }
    public void OnClick_OKButton()
    {
        Debug.Log("메인으로");
        ECPlayerStatManager.Instance.UpdateStat(eventType, conditionType);

        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);

    }
}
