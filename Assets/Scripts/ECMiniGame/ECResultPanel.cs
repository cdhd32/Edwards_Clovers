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

    public void ShowResult(EResultState state, EventType type)
    {
        go.SetActive(true);
        result.text = state.ToString();
        eventType = type;
        conditionType = (ConditionType)state;
    }
    public void OnClick_OKButton()
    {
        Debug.Log("메인으로");
        ECPlayerStatManager.Instance.SetPlayerStatByEvent(eventType, conditionType);
        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);

    }
}
