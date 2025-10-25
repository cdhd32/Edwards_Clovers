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
    public Button cloverBtn;
    private bool isFourLeaf;

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
        if(isFourLeaf)
        {
            Debug.Log("네잎");
            return;
        }
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling(); // 드래그 중에는 제일 위로
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isFourLeaf)
        {
            return;
        }
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
        if (isFourLeaf)
        {
            return;
        }
        canvasGroup.blocksRaycasts = true;
    }

    public void ChangeCloverState(bool isFour)
    {
        isFourLeaf = isFour;
        //스프라이트 변경
    }
}
