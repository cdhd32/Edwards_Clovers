using TMPro;
using UnityEngine;

public class ECExamReportBox : MonoBehaviour
{
    public TextMeshProUGUI titleTMP;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI rankTMP;

    public void SetReportBox(string title, int score, int rank)
    {
        Debug.Log(title + "·©Å©" + rank);
        titleTMP.SetText(title);
        scoreTMP.SetText(score.ToString());
        rankTMP.SetText(rank.ToString());
    }
}
