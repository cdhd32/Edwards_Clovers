using DG.Tweening;
using System;
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

        ChangeSpriteByNumber(ref rankSprite, num);
        ChangeSpriteByNumber(ref rankSpritePriv, numPriv);

        numBar.value = (num % 75.0f)/75f;

        DoRankSptireTransition();
    }

    private void ChangeSpriteByNumber(ref Sprite target, int num)
    {
        switch (ECUtils.GetRankString(num))
        {
            case "S":
                target = rankList[0];
                break;
            case "A+":
                target = rankList[1];
                break;
            case "A":
                target = rankList[2];
                break;
            case "B+":
                target = rankList[3];
                break;
            case "B":
                target = rankList[4];
                break;
            case "C+":
                target = rankList[5];
                break;
            case "C":
                target = rankList[6];
                break;
            case "D+":
                target = rankList[7];
                break;
            case "D":
                target = rankList[8];
                break;
            default:
                target = rankList[0];
                break;
        }
    }

    private void DoRankSptireTransition()
    {
        //DoTween 으로 priv -> arrow -> next 변경 애니메이션
        if (seq == null)
            seq = DOTween.Sequence();

        seq.Append(rankImg.transform.DOScale(0f, animationSpeed));
        seq.AppendCallback(() => rankImg.sprite = rankSpritePriv);
        seq.Append(rankImg.transform.DOScale(1f, animationSpeed));
        seq.Append(rankImg.transform.DOScale(0f, animationSpeed));
        seq.AppendCallback(() => rankImg.sprite = arrowSprite);
        seq.Append(rankImg.transform.DOScale(1f, animationSpeed));
        seq.Append(rankImg.transform.DOScale(0f, animationSpeed));
        seq.AppendCallback(() => rankImg.sprite = rankSprite);
        seq.Append(rankImg.transform.DOScale(1f, animationSpeed));
    }
}
