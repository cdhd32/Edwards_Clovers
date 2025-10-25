using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ECCheerUp : MonoBehaviour
{
    public Image friendsImage;
    public RectTransform bgImageTr;
    private RectTransform friendRectTransform;
    private Vector2 originPos;
    private Sequence seq;

    public float animationSpeed = 1;

    private void Awake()
    {
        friendRectTransform = friendsImage.transform as RectTransform;
        originPos = friendRectTransform.transform.position;
    }

   
    public RectTransform maskPanel;
    public float targetHeight = 400f;  // 최종 열릴 높이
    public float duration = 0.5f;

    void Start()
    {
        CreateSeq();
        PlayAnimation();
        
    }
    private void CreateSeq()
    {
        seq = DOTween.Sequence().Pause();
        seq.Append(friendRectTransform.DOScale(1.2f, animationSpeed));
        seq.Append(friendRectTransform.DOScale(1f, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y + 50f, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y + 50f, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y, animationSpeed));
        seq.Append(bgImageTr.DOScaleY(0, animationSpeed));

        seq.OnComplete(() =>
        {
            ECPlayerStatManager.Instance.UpdateStatCheer();

            ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
        });

    }

    public void PlayAnimation()
    {
        seq.Play();
    }
}
