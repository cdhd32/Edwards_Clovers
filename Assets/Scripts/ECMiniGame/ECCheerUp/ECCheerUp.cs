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

    private void Start()
    {
        CreateSeq();
        PlayAnimation();
    }

    private void CreateSeq()
    {
        seq = DOTween.Sequence().Pause();
        seq.Append(bgImageTr.DOScaleY(1, animationSpeed));
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
