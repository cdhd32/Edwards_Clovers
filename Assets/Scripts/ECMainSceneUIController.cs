using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ECMainSceneUIController : MonoBehaviour
{
    private ECMainSceneManager mainSceneManager;

    [SerializeField]
    private Button actionButton;

    [SerializeField]
    private StatusUI[] statPanels;

    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private TMP_Text classText;

    [SerializeField]
    private TMP_Text dDayText;

    void Start()
    {
        mainSceneManager = ECMainSceneManager.Instance;

        //Get Data from MainSceneManager

        dDayText.text = ECUtils.GetDDayString(mainSceneManager.GetLeftDayNum());

        classText.text = ECUtils.GetClassString(mainSceneManager.GetClassNum());

        statPanels[0].SetNum(mainSceneManager.GetStatusNum(0));

        hpBar.value = mainSceneManager.GetHP() / 100.0f;
    }
}
