using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ECCheerUp : MonoBehaviour
{
    public Image friendsImage;
    public RectTransform bgImageTr;
    private RectTransform friendRectTransform;
    private Vector2 originPos;
    private Sequence seq;

    private float animationSpeed = 0.5f;

    private void Awake()
    {
        friendRectTransform = friendsImage.transform as RectTransform;
        originPos = friendRectTransform.transform.position;
    }

   
    public RectTransform maskPanel;
    public float targetHeight = 400f;  // 최종 열릴 높이
    public float duration = 0.5f;

    void Start()
    {
        CreateSeq();
        PlayAnimation();
        
    }
    private void CreateSeq()
    {
        seq = DOTween.Sequence().Pause();
        seq.Append(friendRectTransform.DOScale(1.2f, animationSpeed));
        seq.Append(friendRectTransform.DOScale(1f, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y + 30f, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y + 30f, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y, animationSpeed));


        seq.OnComplete(() =>
        {
            ECPlayerStatManager statManage = ECPlayerStatManager.Instance;
            int leftDayVal = statManage.GetPlayerStat(PlayerStatType.LEFTDAY);
            int classVal = statManage.GetPlayerStat(PlayerStatType.CLASS);
            ExamEventCheck(leftDayVal, classVal);
            //ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
        });

    }

    private void ExamEventCheck(int leftDay, int classCount)
    {
        Debug.Log("남은날"+ leftDay + "교시" + classCount);
        //leftDay++;

        if (leftDay == 5 || leftDay == 2 || leftDay == 1)
        {
            if(classCount == 4)
            {
                ECPlayerStatManager.Instance.UpdateStatCheer();
                ECGlobalSceneManager.Instance.LoadScene(SceneType.EXAM);
                return;
            }

            if(leftDay == 0)
            {
                ECPlayerStatManager.Instance.UpdateStatCheer();
                ECGlobalSceneManager.Instance.LoadScene(SceneType.EXAM);
                return;
            }


            //단원 평가를 봐야해요
        }

        ECPlayerStatManager.Instance.UpdateStatCheer();
        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
    }

    public void PlayAnimation()
    {
        seq.Play();
    }
}
