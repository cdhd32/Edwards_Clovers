using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToMoveTranstion : MonoBehaviour, IPointerDownHandler
{
    public enum Direction
    {
        FromLeft,
        FromRight,
        FromTop,
        FromBottom,
        FromCustom
    }
    [Header("Transition Settings")]
    public Direction direction; // 방향 설정
    public float offset = 300f;                         // 이동 거리
    public float duration = 0.6f;                       // 이동 시간
    public Ease easeType = Ease.OutCubic;               // 움직임 곡선
    public Vector3 customOffset = Vector3.zero;         // FromCustom 시 사용
    public bool playOnEnable = true;                    // 활성화될 때 자동 재생

    private Vector3 originalPos;   // 원래 위치 저장
    private RectTransform rect;    // UI용 RectTransform 캐시
    private bool isRectUI = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        isRectUI = rect != null;
        originalPos = isRectUI ? rect.anchoredPosition3D : transform.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("ttttt");
        originalPos = isRectUI ? rect.anchoredPosition3D : transform.localPosition;
        if (playOnEnable)
            PlayTransition();
    }

    public void PlayTransition()
    {
        // 기존 트윈 정리
        DOTween.Kill(gameObject);

        Vector3 startOffset = GetOffsetVector();

        if (isRectUI)
        {
            rect.anchoredPosition3D = originalPos + startOffset;
            rect.DOAnchorPos3D(originalPos, duration)
                .SetEase(easeType)
                .SetId(gameObject);
        }
        else
        {
            transform.localPosition = originalPos + startOffset;
            transform.DOLocalMove(originalPos, duration)
                     .SetEase(easeType)
                     .SetId(gameObject);
        }
    }

    private Vector3 GetOffsetVector()
    {
        switch (direction)
        {
            case Direction.FromLeft: return new Vector3(-offset, 0, 0);
            case Direction.FromRight: return new Vector3(offset, 0, 0);
            case Direction.FromTop: return new Vector3(0, offset, 0);
            case Direction.FromBottom: return new Vector3(0, -offset, 0);
            case Direction.FromCustom: return customOffset;
            default: return Vector3.zero;
        }
    }
}
