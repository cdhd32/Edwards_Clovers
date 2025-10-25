
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
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
    public int cardCount = 4;
    private int correctCount = 0;
    public TextMeshProUGUI vocabularyBookTMP_Word;
    public TextMeshProUGUI vocabularyBookTMP_Meaning;
    public GameObject vocaPanel;
    private StringBuilder sb = new StringBuilder(100);

    private void Awake()
    {
        string json = englishData.text;
        cardDataTemplate = JsonUtility.FromJson<ECEnglishCardInfoTemplate>(json);
        StartGame();
    }

    private void SetVocaBook(int cardcount)
    {
        for(int i=0; i< cardcount; i++)
        {
            sb.Append(currentCardInfos[i].word);
            sb.AppendLine();
        }
        vocabularyBookTMP_Word.SetText(sb.ToString());
        sb.Clear();
        for (int i = 0; i < cardcount; i++)
        {
            sb.Append(currentCardInfos[i].meaning);
            sb.AppendLine();
        }
        vocabularyBookTMP_Meaning.SetText(sb.ToString());
    }

    public void OnClickVocaBook()
    {
        vocaPanel.SetActive(true);
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
        SetVocaBook(currentCards);

        for (int i=0; i<cardCount; ++i)
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

    public override EResultState GetScore()
    {
        //타이머 경과로 종료된 경우
        return EResultState.Bad;
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
                correctCount++;
                if(correctCount == cardCount / 2)
                {
                    timer.EndTimer(EResultState.Perfect);
                }

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
