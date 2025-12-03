using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ECFirstSceneUIController : MonoBehaviour
{
    [SerializeField]
    private Button progressButton;

    [SerializeField]
    private TMP_Text progressText;

    //리셋 버튼 사용 안함
    //[SerializeField]
    //private Button resetButton;

    //[SerializeField]
    //private TMP_Text resetButtonText;

    [SerializeField]
    private Image fadeImage;

    private const string resetCompletionText = "초기화 완료";
    private bool isEditor;


    private void Awake()
    {
#if UNITY_EDITOR
        //isEditor = true;
#endif

        progressButton.onClick.AddListener(() =>
        {
            fadeImage.DOFade(1.0f, 3.0f).OnComplete(() =>
            {
                ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
            });
        });

        //resetButton.onClick.AddListener(() =>
        //{
        //    ECPlayerStatManager.Instance.DeleteStatData();

        //    resetButtonText.text = resetCompletionText;
        //    resetButton.interactable = false;
        //});
    }

    private void Start()
    {
        if(!isEditor)
        {
            ECPlayerStatManager.Instance.DeleteStatData();
        }

        progressText.DOFade(0.1f, 1.75f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnClickBtnSkip()
    {
        if (!isEditor)
        {
            ECPlayerStatManager.Instance.DeleteStatData();
        }

        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
    }

}
