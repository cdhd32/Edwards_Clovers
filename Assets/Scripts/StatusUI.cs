using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public TMP_Text nameText;

    public Image rankImg;
    public List<Sprite> rankList;

    public Slider numBar;

    private Sprite rankSprite;
    private Sprite rankSpritePriv;

    [SerializeField]
    private Image rankUpImage;

    private Sequence rankSequence;

    private Sequence barSequence;

    private float animationSpeed = 0.5f;

    private readonly float SCALE_VALUE_BAR = 75.0f;

    public void Awake()
    {
        rankUpImage.gameObject.SetActive(false);
        rankImg.gameObject.SetActive(true);
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void DoSetNum(int num, int numPriv)
    {
        string rankString = ChangeSpriteByNumber(ref rankSprite, num);
        string rankStringPriv = ChangeSpriteByNumber(ref rankSpritePriv, numPriv);

        if (num != numPriv)
            DoChangeValueBar(num, numPriv);
        else
            numBar.value = (num % SCALE_VALUE_BAR) / SCALE_VALUE_BAR;

        //랭크가 바뀌었을 때만 애니메이션 실행
        if (!rankString.Equals(rankStringPriv))
            DoRankSptireTransition();
        else
            rankImg.sprite = rankSprite;
    }

    private string ChangeSpriteByNumber(ref Sprite target, int num)
    {
        string rankStr = ECUtils.GetRankString(num);
        switch (rankStr)
        {
            case "S":
                target = rankList[0];
                return rankStr;
            case "A+":
                target = rankList[1];
                return rankStr;
            case "A":
                target = rankList[2];
                return rankStr;
            case "B+":
                target = rankList[3];
                return rankStr;
            case "B":
                target = rankList[4];
                return rankStr;
            case "C+":
                target = rankList[5];
                return rankStr;
            case "C":
                target = rankList[6];
                return rankStr;
            case "D+":
                target = rankList[7];
                return rankStr;
            case "D":
                target = rankList[8];
                return rankStr;
            default:
                target = rankList[0];
                return rankStr;
        }
    }

    private void DoRankSptireTransition()
    {
        //DoTween 으로 priv -> arrow -> next 변경 애니메이션
        if (rankSequence == null)
            rankSequence = DOTween.Sequence();

        rankImg.sprite = rankSpritePriv;

        rankSequence.AppendInterval(animationSpeed);
        rankSequence.AppendCallback(() =>
        {
            rankUpImage.transform.DOScale(0.8f, 0f);
            rankUpImage.gameObject.SetActive(true);
            rankImg.gameObject.SetActive(false);
        });
        rankSequence.Append(rankUpImage.transform.DOScale(1.2f, animationSpeed).SetEase(Ease.InCubic));
        rankSequence.AppendCallback(() =>
        {
            rankImg.sprite = rankSprite;
        });
        rankSequence.Append(rankUpImage.transform.DOScale(0.8f, animationSpeed).SetEase(Ease.OutCubic));
        rankSequence.AppendCallback(() =>
        {
            rankUpImage.gameObject.SetActive(false);
            rankImg.gameObject.SetActive(true);
        });
    }

    private void DoChangeValueBar(int value, int valPriv)
    {
        float barValue = (value % SCALE_VALUE_BAR) / SCALE_VALUE_BAR;
        float barValuePriv = (valPriv % SCALE_VALUE_BAR) / SCALE_VALUE_BAR;

        float diff = barValue - barValuePriv;

        if (barSequence == null)
            barSequence = DOTween.Sequence();

        if (barValue < barValuePriv)
        {
            float animSpeed0 = animationSpeed * (1.0f - barValuePriv) / (1.0f - diff);
            float animSpeed1 = animationSpeed * barValue / (1.0f - diff);

            numBar.value = barValuePriv;

            //100% 채우기
            barSequence.AppendInterval(animationSpeed);
            barSequence.Append(numBar.DOValue(1.0f, animSpeed0));

            //다시 0부터 채우기
            barSequence.Append(numBar.DOValue(0.0f, 0.0f));
            barSequence.Append(numBar.DOValue(barValue, animSpeed1));
        }
        else
        {
            numBar.value = barValuePriv;

            barSequence.AppendInterval(animationSpeed);
            barSequence.Append(numBar.DOValue(barValue, animationSpeed));
        }
    }
}
