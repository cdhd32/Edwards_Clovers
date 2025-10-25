using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ECMainSceneUIController : MonoBehaviour
{
    private ECMainSceneManager mainSceneManager;

    [SerializeField]
    private Button actionButtonKr;

    [SerializeField]
    private Button actionButtonEn;

    [SerializeField]
    private Button actionButtonMath;

    [SerializeField]
    private Button actionButtonSci;

    [SerializeField]
    private Button actionButtonLuk;

    [SerializeField]
    private StatusUI[] statPanels;

    [SerializeField]
    private Slider motivationBar;

    [SerializeField]
    private TMP_Text classText;

    [SerializeField]
    private TMP_Text dDayText;

    private void Awake()
    {
        actionButtonKr.onClick.AddListener(() =>
        {
            ECGlobalSceneManager.Instance.LoadScene(SceneType.KR);
        });

        actionButtonEn.onClick.AddListener(() =>
        {
            ECGlobalSceneManager.Instance.LoadScene(SceneType.EG);
        });

        actionButtonMath.onClick.AddListener(() =>
        {
            ECGlobalSceneManager.Instance.LoadScene(SceneType.MATH);
        });

        actionButtonSci.onClick.AddListener(() =>
        {
            ECGlobalSceneManager.Instance.LoadScene(SceneType.SCIENCE);
        });

        actionButtonLuk.onClick.AddListener(() =>
        {
            ECGlobalSceneManager.Instance.LoadScene(SceneType.LUCKY);
        });

    }

    void Start()
    {
        mainSceneManager = ECMainSceneManager.Instance;

        UpdateUIs();
    }

    public void UpdateUIs()
    {
        dDayText.text = ECUtils.GetDDayString(mainSceneManager.GetLeftDayNum());

        classText.text = ECUtils.GetClassString(mainSceneManager.GetClassNum());


        int max = statPanels.Length + 1;

        for (int i = 1; i < max; i++)
        {
            statPanels[i - 1].SetName(mainSceneManager.GetStautsName((PlayerStatType)i));

            statPanels[i - 1].SetNum(mainSceneManager.GetStatusNum((PlayerStatType)i));
        }

        motivationBar.value = mainSceneManager.GetMotivation() / 100.0f;
    }
}
