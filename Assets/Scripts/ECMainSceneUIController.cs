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
    private Slider motivationBar;

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

        statPanels[0].SetName(mainSceneManager.GetStautsName(PlayerStatType.KOR));

        statPanels[0].SetNum(mainSceneManager.GetStatusNum(PlayerStatType.KOR));

        motivationBar.value = mainSceneManager.GetMotivation() / 100.0f;
    }
}
