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

    private float animationSpeed = 0.5f;

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
        seq.Append(friendRectTransform.DOMoveY(originPos.y + 30f, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y + 30f, animationSpeed));
        seq.Append(friendRectTransform.DOMoveY(originPos.y, animationSpeed));


        seq.OnComplete(() =>
        {
            ECPlayerStatManager.Instance.GoNextTurn(EventType.CHEER);
        });

    }

    public void PlayAnimation()
    {
        seq.Play();
    }
}
