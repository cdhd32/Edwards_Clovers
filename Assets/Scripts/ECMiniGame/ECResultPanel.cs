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
        ECPlayerStatManager statManage = ECPlayerStatManager.Instance;
        statManage.UpdateStat(eventType, conditionType);
        int leftDayVal = statManage.GetPlayerStat(PlayerStatType.LEFTDAY);
        int classVal = statManage.GetPlayerStat(PlayerStatType.CLASS);
        //ECGlobalSceneManager.Instance.LoadScene(SceneType.EXAM);
        if (classVal == 1)
        {
            //마지막 교시일때
            ExamEventCheck(leftDayVal);
        }
        else
        {
            ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
        }


    }

    private void ExamEventCheck(int leftDay)
    {
        if (leftDay == 5 || leftDay == 2 || leftDay == 0)
        {
            ECGlobalSceneManager.Instance.LoadScene(SceneType.EXAM);
            //단원 평가를 봐야해요
        }
    }
}
