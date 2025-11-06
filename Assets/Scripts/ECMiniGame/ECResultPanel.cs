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

        int leftDayVal = statManage.GetPlayerStat(PlayerStatType.LEFTDAY);
        int classVal = statManage.GetPlayerStat(PlayerStatType.CLASS);
        Debug.Log("남은 날 :" + leftDayVal +"교시" + classVal);
        
        if (classVal == 4 || leftDayVal <=0)
        {
            //마지막 교시거나 d - day일 때
            ExamEventCheck(leftDayVal, classVal);
        }
        else
        {
            statManage.UpdateStat(eventType, conditionType);
            ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
        }
    }

    private void ExamEventCheck(int leftDay, int classCount)
    {
        ECPlayerStatManager statManage = ECPlayerStatManager.Instance;

        Debug.Log("남은날" + leftDay);
        //leftDay++;

        if (leftDay == 5 || leftDay == 2 || leftDay == 1)
        {
            if (classCount == 4)
            {
                statManage.UpdateStat(eventType, conditionType);
                ECGlobalSceneManager.Instance.LoadScene(SceneType.EXAM);
                return;
            }

            if (leftDay == 0)
            {
                statManage.UpdateStat(eventType, conditionType);
                ECGlobalSceneManager.Instance.LoadScene(SceneType.EXAM);
                return;
            }


            //단원 평가를 봐야해요
        }

        statManage.UpdateStat(eventType, conditionType);
        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);

    }
}
