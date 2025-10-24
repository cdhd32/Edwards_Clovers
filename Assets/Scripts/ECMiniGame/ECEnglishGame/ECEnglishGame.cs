
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ECEnglishCardInfo
{
    public int id;
    public string word;
    public string meaning;
}
[Serializable]
public class ECEnglishCardInfoTemplate
{
    public ECEnglishCardInfo[] cardInfos;
}

public class ECEnglishGame : ECMiniGameBase
{
    public TextAsset englishData;
    private ECEnglishCardInfoTemplate cardDataTemplate = new();
    public ECEnglishCard cardPrefab;
    public RectTransform cardParent;
    private ECEnglishCard[] cards;
    private ECEnglishCardInfo[] currentCardInfos;
    [NonSerialized] public ECEnglishCard befCard;
    private int cardCount = 14;

    private void Awake()
    {
        string json = englishData.text;
        cardDataTemplate = JsonUtility.FromJson<ECEnglishCardInfoTemplate>(json);
        StartGame();
    }

    public void CreateCards()
    {
        cards = new ECEnglishCard[cardCount];
        Utils.Shuffle(cardDataTemplate.cardInfos);
        int currentCards = (int)(cardCount * 0.5f);
        currentCardInfos = new ECEnglishCardInfo[currentCards];
        for(int i=0; i< currentCards; i++) 
        {
            currentCardInfos[i] = cardDataTemplate.cardInfos[i];
        }

        for(int i=0; i<cardCount; ++i)
        {
            ECEnglishCard c = Instantiate(cardPrefab, cardParent);
            c.ChangeCardState(ECardState.Close);
            cards[i] = c;
        }

        for(int i = 0; i < currentCards; i++) 
        {
            ECEnglishCard c = cards[i];
            c.cardInfo = currentCardInfos[i];
            c.SetCardTMP(c.cardInfo.meaning, this);
        }

        Utils.Shuffle(currentCardInfos);

        for (int i = 0; i < currentCards; i++)
        {
            ECEnglishCard c = cards[currentCards + i];
            c.cardInfo = currentCardInfos[i];
            if(c == null || c.cardInfo == null)
            {
                Debug.Log("d");
            }
            c.SetCardTMP(c.cardInfo.word, this);
        }
    }


    public void OpenCard(ECEnglishCard newCard)
    {
        if(befCard == null)
        {
            befCard = newCard;
        }
        else
        {
            if(newCard.cardInfo.id == befCard.cardInfo.id)
            {
                //정답 이벤트
                newCard.ChangeCardState(ECardState.Correct);
                befCard.ChangeCardState(ECardState.Correct);
                befCard = null;

            }
            else
            {
                newCard.ChangeCardState(ECardState.Close);
                befCard.ChangeCardState(ECardState.Close);
                befCard = null;
            }
        }
    }

    public override void StartGame()
    {
        base.StartGame();
        CreateCards();
    }
}
