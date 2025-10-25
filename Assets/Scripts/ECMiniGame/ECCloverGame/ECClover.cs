using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ECloverState
{
    Three, Four, Count
}

public class ECClover : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [NonSerialized] public ECloverState cloverState;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    public Image cloverImage;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetSprite(Sprite sp)
    {
        cloverImage.sprite = sp;
    }

    public void FindCloverEvent()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOScale(1.5f, 1));
        seq.Append(canvasGroup.DOFade(0, 0.5f)).OnComplete(() => CompEvent());
        seq.Play();
    }

    private void CompEvent()
    {
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling(); // 드래그 중에는 제일 위로
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out pos
        );
        rectTransform.anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void ChangeCloverState(ECloverState state)
    {
        if (cloverState == state) return;
        cloverState = state;
        //스프라이트 변경
    }
}
