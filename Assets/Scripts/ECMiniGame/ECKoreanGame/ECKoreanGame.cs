using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = System.Random;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ECKRItemData
{
    public int id;       
    public string name; 
    [NonSerialized] public Sprite sprite;
}
[Serializable]
public class ECKRItemDataTemplate
{
    public ECKRItemData[] items;
}

public class ECKoreanGame : ECMiniGameBase
{
    [Header("UI References")]
    public Image questionImage; // 칠판 이미지
    public TextMeshProUGUI questionTMP;
    public ECKRChoiceBox choiceBoxPrefab;
    public RectTransform choiceBoxParent;
    public ECKRChoiceBox[] choiceButtons;  // 객관식 버튼 3개
    private int answerCount = 4;

    public Sprite[] answerCheckImages; // 0 틀림 1 맞춤
    public Image answerCheck;

    [Header("Data")]
    public TextAsset jsonFile;      // ItemDatabase.json
    public ECKRItemDataTemplate itemList;
    private ECKRItemData correctItem;

    public Sprite[] itemSprites;

    private int currentScore = 0;
    private List<ECKRItemData> itemDatas;

    private bool isDelay;

    void Start()
    {
        // JSON 데이터 로드
        choiceButtons = new ECKRChoiceBox[answerCount];
        itemList = JsonUtility.FromJson<ECKRItemDataTemplate>(jsonFile.text);

        for (int i = 0; i < itemList.items.Length; i++)
        {
            itemList.items[i].sprite = itemSprites[i];
        }
        itemDatas = itemList.items.ToList();
        for (int i=0; i< answerCount; ++i)
        {
            ECKRChoiceBox box = Instantiate(choiceBoxPrefab, choiceBoxParent);
            choiceButtons[i] = box;
        }
        GenerateNewQuestion();
        base.StartGame();
    }

    public override EResultState GetScore()
    {
        EResultState state = SendScore();
        return state;
    }

    private EResultState SendScore()
    {
        EResultState state = EResultState.Good;
        if (currentScore >= 3)
        {
            state = EResultState.Perfect;
        }
        else if (currentScore >= 1 && currentScore < 3)
        {
            state = EResultState.Great;
        }
        else if (currentScore >= 0 && currentScore < 1)
        {
            state = EResultState.Good;
        }
        else if (currentScore < -1)
        {
            state = EResultState.Bad;
        }
        return state;
    }

    void GenerateNewQuestion()
    {
        if(itemDatas.Count == 0)
        {
            timer.EndTimer(EResultState.Perfect);
            return;
        }
        answerCheck.enabled = false;
        Random random = new System.Random();
        correctItem = itemDatas[random.Next(0, itemDatas.Count)];
        //Sprite loadedSprite = Resources.Load<Sprite>($"ItemSprites/{correctItem.name}");
        questionImage.sprite = correctItem.sprite; //나중에 이미지 나오면 변경 / 임시로 텍스트
        //questionTMP.SetText(correctItem.name);

        List<ECKRItemData> choices = new List<ECKRItemData> { correctItem };
        while (choices.Count < answerCount)
        {
            var randomItem = itemList.items[random.Next(0, itemList.items.Length)];
            if (!choices.Contains(randomItem))
                choices.Add(randomItem);
        }

        Utils.Shuffle(choices);
        itemDatas.Remove(correctItem);
        // 버튼 세팅
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;
            choiceButtons[i].tmp.SetText(choices[i].name);
            choiceButtons[i].answerButton.interactable = true;
            choiceButtons[i].answerButton.onClick.RemoveAllListeners();
            choiceButtons[i].answerButton.onClick.AddListener(() => OnChoiceSelected(choices[index]));
        }
    }

    void OnChoiceSelected(ECKRItemData selected)
    {
        answerCheck.enabled = true;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].answerButton.interactable = false;
        }
        if (selected.id == correctItem.id)
        {
            answerCheck.sprite = answerCheckImages[0];
        }
        else
        {
            answerCheck.sprite = answerCheckImages[1];
        }

        Invoke(nameof(GenerateNewQuestion), 0.5f);
    }

}
