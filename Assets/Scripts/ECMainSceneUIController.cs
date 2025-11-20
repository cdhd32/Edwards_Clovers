using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

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
    private Button actionButtonCheerUp;

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
    [SerializeField]
    private GameObject cheerUpPannel;

    public RectTransform buttonsRect;
    public Image openImage;
    public CanvasGroup cg;
    private bool isUIMoving;

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

        actionButtonCheerUp.onClick.AddListener(() =>
        {
            StartCoroutine(activeCheerUpPannel());
        });

    }
    IEnumerator LoadScene(SceneType sceneType) {
        yield return new WaitForSeconds(0.5f);
        ECGlobalSceneManager.Instance.LoadScene(sceneType);
    }
    IEnumerator activeCheerUpPannel()
    {
        cheerUpPannel.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        cheerUpPannel.SetActive(false);

    }

    void Start()
    {
        mainSceneManager = ECMainSceneManager.Instance;

        UpdateUIs();
    }

    public void OnClickShowActionButton()
    {
        if(isUIMoving)
        {
            return;
        }
        isUIMoving = true;
        bool isOpen = buttonsRect.anchoredPosition.x == 0;
        float pos = isOpen ? 297 : 0;
        openImage.enabled = !isOpen;
        buttonsRect.DOAnchorPosX(pos, 0.5f).OnComplete(() => OnCompleteMovePos());
    }

    private void OnCompleteMovePos()
    {
        isUIMoving = false;
        if(buttonsRect.anchoredPosition.x == 0)
        {
            cg.interactable = true;
        }
        else
        {
            //openImage.enabled = false;
            cg.interactable = false;
        }
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

            statPanels[i - 1].DoSetNum(mainSceneManager.GetStatusNum((PlayerStatType)i), mainSceneManager.GetStatusNum((PlayerStatType)i));
        }

        motivationBar.value = mainSceneManager.GetMotivation() / 100.0f;                                    

        if (mainSceneManager.GetMotivation() < ECConst.MOTVIATION_PAY)
        {
            SetEnableActionButtons(false);
        }
        else
        {
            SetEnableActionButtons(true);
        }
    }

    public void SetEnableActionButtons(bool isEnable)
    {
        actionButtonKr.interactable = isEnable;
        actionButtonEn.interactable = isEnable;
        actionButtonMath.interactable = isEnable;
        actionButtonSci.interactable = isEnable;
        actionButtonLuk.interactable = isEnable;
    }
}
