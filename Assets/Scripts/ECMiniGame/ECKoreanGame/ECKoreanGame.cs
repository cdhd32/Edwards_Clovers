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
    public ECMathChoiceBox choiceBoxPrefab;
    public RectTransform choiceBoxParent;
    public ECMathChoiceBox[] choiceButtons;  // 객관식 버튼 3개
    private int answerCount = 3;

    [Header("Data")]
    public TextAsset jsonFile;      // ItemDatabase.json
    public ECKRItemDataTemplate itemList;
    private ECKRItemData correctItem;

    private int currentScore = 0;

    void Start()
    {
        // JSON 데이터 로드
        choiceButtons = new ECMathChoiceBox[answerCount];
        itemList = JsonUtility.FromJson<ECKRItemDataTemplate>(jsonFile.text);
        for(int i=0; i< answerCount; ++i)
        {
            ECMathChoiceBox box = Instantiate(choiceBoxPrefab, choiceBoxParent);
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
        Random random = new System.Random();
        correctItem = itemList.items[random.Next(0, itemList.items.Length)];

        //Sprite loadedSprite = Resources.Load<Sprite>($"ItemSprites/{correctItem.name}");
        //itemImage.sprite = loadedSprite; 나중에 이미지 나오면 변경 / 임시로 텍스트
        questionTMP.SetText(correctItem.name);

        List<ECKRItemData> choices = new List<ECKRItemData> { correctItem };
        while (choices.Count < 3)
        {
            var randomItem = itemList.items[random.Next(0, itemList.items.Length)];
            if (!choices.Contains(randomItem))
                choices.Add(randomItem);
        }

        Utils.Shuffle(choices);

        // 버튼 세팅
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;
            choiceButtons[i].tmp.SetText(choices[i].name);
            choiceButtons[i].answerButton.onClick.RemoveAllListeners();
            choiceButtons[i].answerButton.onClick.AddListener(() => OnChoiceSelected(choices[index]));
        }
    }

    void OnChoiceSelected(ECKRItemData selected)
    {
        if (selected.id == correctItem.id)
        {
            Debug.Log("정답");
        }
        else
        {
            Debug.Log("오답");
        }

        Invoke(nameof(GenerateNewQuestion), 1.0f);
    }

}
