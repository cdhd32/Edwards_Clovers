using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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

    //[SerializeField]
    //private TMP_Text classText;

    [SerializeField]
    private TMP_Text dDayText;

    [SerializeField]
    private Image classImage;

    [SerializeField]
    private List<Sprite> classImgSprites;
    //버튼 5개 추가[

    private void Awake()
    {
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

        UpdateUIs();
    }

    public void UpdateUIs()
    {
        dDayText.text = ECUtils.GetDDayString(mainSceneManager.GetLeftDayNum());

        //classText.text = ECUtils.GetClassString(mainSceneManager.GetClassNum());
        classImage.sprite = classImgSprites[mainSceneManager.GetClassNum()-1];

        int max = statPanels.Length + 1;

        for (int i = 1; i < max; i++)
        {
            statPanels[i - 1].SetName(mainSceneManager.GetStautsName((PlayerStatType)i));

            statPanels[i - 1].SetNum(mainSceneManager.GetStatusNum((PlayerStatType)i));
        }

        motivationBar.value = mainSceneManager.GetMotivation() / 100.0f;
    }
}
