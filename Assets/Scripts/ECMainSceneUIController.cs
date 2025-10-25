using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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

    //버튼 5개 추가[

    private void Awake()
    {
        //테스트 Scene 전환, 씬 이름 저장해서 불러오기

        actionButtonKr.onClick.AddListener(() =>
        {
            StartCoroutine(LoadScene(SceneType.KR));
        });

        actionButtonEn.onClick.AddListener(() =>
        {
            StartCoroutine(LoadScene(SceneType.EG));
        });

        actionButtonMath.onClick.AddListener(() =>
        {
            StartCoroutine(LoadScene(SceneType.MATH));
        });

        actionButtonSci.onClick.AddListener(() =>
        {
            StartCoroutine(LoadScene(SceneType.SCIENCE));
        });

        actionButtonLuk.onClick.AddListener(() =>
        {
            StartCoroutine(LoadScene(SceneType.LUCKY));
        });

    }
    IEnumerator LoadScene(SceneType sceneType) {
        yield return new WaitForSeconds(0.5f);
        ECGlobalSceneManager.Instance.LoadScene(sceneType);
    }

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
