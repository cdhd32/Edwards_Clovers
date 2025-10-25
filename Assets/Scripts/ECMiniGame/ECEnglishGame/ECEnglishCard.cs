using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ECardState
{
    Open, Close, Correct, Count
}

public class ECEnglishCard : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public Image backImage;
    public Outline cardOutline;
    [NonSerialized] public ECardState cardState = ECardState.Count;
    [NonSerialized] public ECEnglishCardInfo cardInfo;
    private ECEnglishGame egGame;
    private bool isDelay;

    public void SetCardTMP(string cardInfo, ECEnglishGame game)
    {
        tmp.SetText(cardInfo);
        egGame = game;
    }

    public void OnClickEnglishCard()
    {
        if(isDelay || cardState == ECardState.Correct)
        {
            return;
        }
        ChangeCardState(ECardState.Open);
        egGame.OpenCard(this);
    }

    public void ChangeCardState(ECardState state)
    {
        if (cardState == state)
        {
            return;
        }
        if(state == ECardState.Open)
        {
            backImage.color = Color.clear;
        }
        else if (state == ECardState.Close)
        {
            if(cardState == ECardState.Open && state == ECardState.Close)
            {
                //backImage.color = Color.red;
                cardOutline.effectColor = Color.red;
                isDelay = true;
                backImage.DOColor(Color.white, 0.5f).SetAutoKill().OnComplete(() => comp());
                cardOutline.DOColor(Color.clear, 0.1f);
            }
            else
            {
                backImage.color = Color.white;
            }
        }
        else if (state == ECardState.Correct)
        {
            Color correctColor = Color.green;
            correctColor.a = 0.5f;
            cardOutline.effectColor = Color.green;
            //backImage.color = correctColor;
        }

        cardState = state;
        //카드 뒤집기
    }

    private void comp()
    {
        isDelay = false;
        //나중에 뒤집히는 동안 클릭 막기??
    }
}
