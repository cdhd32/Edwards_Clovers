using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ECMathChoiceBox : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    //public TextMeshProUGUI answerNumber;
    public Button answerButton;
    public Image answerNumberImage;
    public Image answerCheck;

    public void SetAnswerNumberImage(Sprite sp)
    {
        answerNumberImage.sprite = sp;
    }

    public void ShowCheckImage(bool show)
    {
        answerCheck.enabled = show;
    }
}
