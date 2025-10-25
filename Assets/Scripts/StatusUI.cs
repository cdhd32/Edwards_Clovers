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

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetNum(int num)
    {
        numText.text = num.ToString()+" / "+((num/75+1)*75);
        


        switch (ECUtils.GetRankString(num))
        {
            case "S":
                rankImg.sprite = rankList[0];
                break;
            case "A+":
                rankImg.sprite = rankList[1];
                break;
            case "A":
                rankImg.sprite = rankList[2];
                break;
            case "B+":
                rankImg.sprite = rankList[3];
                break;
            case "B":
                rankImg.sprite = rankList[4];
                break;
            case "C+":
                rankImg.sprite = rankList[5];
                break;
            case "C":
                rankImg.sprite = rankList[6];
                break;
            case "D+":
                rankImg.sprite = rankList[7];
                break;
            case "D":
                rankImg.sprite = rankList[8];
                break;
            default:
                rankImg.sprite = rankList[0];
                break;
        }
        numBar.value = (num % 75.0f)/75f;
    }
}
