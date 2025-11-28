using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text numText;

    public Image rankImg;
    public List<Sprite> rankList;

    public Slider numBar;

    private Sprite rankSprite;
    private Sprite rankSpritePriv;

    [SerializeField]
    private Sprite arrowSprite;

    private Sequence seq;

    private float animationSpeed = 0.25f;

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void DoSetNum(int num, int numPriv)
    {
         numText.text = num.ToString()+" / "+((num/75+1)*75);

        string rankString = ChangeSpriteByNumber(ref rankSprite, num);
        string rankStringPriv = ChangeSpriteByNumber(ref rankSpritePriv, numPriv);

        numBar.value = (num % 75.0f)/75f;

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
        if (seq == null)
            seq = DOTween.Sequence();

        seq.Append(rankImg.transform.DOScale(0f, animationSpeed).SetEase(Ease.InCubic));
        seq.AppendCallback(() => rankImg.sprite = rankSpritePriv);
        seq.Append(rankImg.transform.DOScale(1f, animationSpeed).SetEase(Ease.OutCubic));
        seq.Append(rankImg.transform.DOScale(0f, animationSpeed).SetEase(Ease.InCubic));
        seq.AppendCallback(() => rankImg.sprite = arrowSprite);
        seq.Append(rankImg.transform.DOScale(1f, animationSpeed).SetEase(Ease.OutCubic));
        seq.Append(rankImg.transform.DOScale(0f, animationSpeed).SetEase(Ease.InCubic));
        seq.AppendCallback(() => rankImg.sprite = rankSprite);
        seq.Append(rankImg.transform.DOScale(1f, animationSpeed).SetEase(Ease.OutCubic));
    }
}
