using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text numText;
    public TMP_Text rankText;

    public Slider numBar;

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetNum(int num)
    {
        numText.text = num.ToString()+" / 1000";
        
        rankText.text = ECUtils.GetRankString(num);

        numBar.value = num / 1000.0f;
    }
}
