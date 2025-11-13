using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacteIdleMove : MonoBehaviour, IPointerDownHandler
{
    [Header("idle 관련 변수")]
    [SerializeField] float moveY = 10f;    // 위아래 움직임 (픽셀 단위)
    [SerializeField] float duration = 1.5f;

    [Header("클릭 흔들림 관련 변수")]
    [SerializeField] float shakeStrength = 10f; 
    [SerializeField] int shakeVibrato = 10;     
    [SerializeField] float shakeTime = 0.25f;

    RectTransform rect;
    Tween idleMoveTween;
    Tween idleScaleTween;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        Vector2 basePos = rect.anchoredPosition;


        idleMoveTween = rect.DOAnchorPosY(basePos.y + moveY, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // idle 일시정지
        idleMoveTween.Pause();
        idleScaleTween.Pause();

        // 좌우로 살짝 흔들림
        rect.DOShakeAnchorPos(shakeTime, new Vector2(shakeStrength, 0), shakeVibrato, 0, false, true)
            .OnComplete(() =>
            {
                // idle 재개
                idleMoveTween.Play();
                idleScaleTween.Play();
            });
    }
}
