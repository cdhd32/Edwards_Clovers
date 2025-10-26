using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ECMathGame : ECMiniGameBase
{
    public TextMeshProUGUI problemText;
    public TextMeshProUGUI problemCount;
    public int choiceCount = 3;
    [NonSerialized] public ECMathChoiceBox[] choiceButtons;
    public ECMathChoiceBox choiceBoxPrefab;
    public RectTransform boxParent;
    private int problemNumber = 1;
    private int currentScore = 0;
    public Sprite[] answerNumberImages;
    public Sprite[] answerCheckImages; // 0 틀림 1 맞춤
    public Image answerCheck;
    private bool isClick;

    private MathQuiz mathQuiz = new MathQuiz();

    private ECMathChoiceBox currentSelectBox;
    private MathQuiz.Question currentQuestion;

    void Start()
    {
        currentScore = 0;
        problemNumber = 1;
        choiceButtons = new ECMathChoiceBox[choiceCount];
        for (int i = 0; i < choiceCount; i++) 
        {
            choiceButtons[i] = Instantiate(choiceBoxPrefab, boxParent);
            choiceButtons[i].SetAnswerNumberImage(answerNumberImages[i]);
        }

        base.StartGame();
        GenerateNewQuestion();
    }

    public override EResultState GetScore()
    {
        EResultState state = SendScore();
        return state;
    }

    private EResultState SendScore()
    {
        EResultState state = EResultState.Good;
        if (currentScore >= 15)
        {
            state = EResultState.Perfect;
        }
        else if (currentScore >= 8 && currentScore < 15)
        {
            state = EResultState.Great;
        }
        else if (currentScore >= 0 && currentScore < 8)
        {
            state = EResultState.Good;
        }
        else if (currentScore < 0)
        {
            state = EResultState.Bad;
        }
        return state;
    }


    void GenerateNewQuestion()
    {
        isClick = false;
        answerCheck.color = Color.clear;
        if (currentSelectBox != null)
        {
            currentSelectBox.ShowCheckImage(false);
            currentSelectBox = null;
        }
        currentQuestion = mathQuiz.GenerateQuestion();
        problemCount.SetText(problemNumber.ToString() + "번문항");
        problemText.text = currentQuestion.Problem;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int answer = currentQuestion.Choices[i];
            choiceButtons[i].tmp.SetText(answer.ToString());
            choiceButtons[i].answerButton.onClick.RemoveAllListeners();
            int index = i;
            choiceButtons[i].answerButton.onClick.AddListener(() => OnChoiceSelected(answer, choiceButtons[index]));
        }
        problemNumber++;
    }

    void OnChoiceSelected(int selectedAnswer, ECMathChoiceBox box)
    {
        if (isClick) return;
        isClick = true;
        box.ShowCheckImage(true);
        currentSelectBox = box;
        answerCheck.color = Color.white;
        if (selectedAnswer == currentQuestion.CorrectAnswer)
        {
            answerCheck.sprite = answerCheckImages[0];
            currentScore++;
        }
        else
        {
            answerCheck.sprite = answerCheckImages[1];
            currentScore -= 3;
        }

        Invoke(nameof(GenerateNewQuestion), 0.8f);
    }
}

public class MathQuiz
{
    private System.Random rand = new System.Random();
    private string[] operators = { "+", "-", "*", "/" };
    private HashSet<int> choicesSet = new HashSet<int>();
    private List<int> choices = new List<int>();

    public class Question
    {
        public string Problem;
        public int CorrectAnswer;
        public List<int> Choices;
    }

    public Question GenerateQuestion()
    {
        string op = operators[rand.Next(operators.Length)];
        int a = rand.Next(1, 21);
        int b = rand.Next(1, 21);

        if (op == "/")
        {
            b = rand.Next(1, 11);     
            int temp = rand.Next(1, 11);
            a = b * temp;
        }

        int correct = op switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => a / b,
            _ => 0
        };

        choicesSet.Clear();
        choicesSet.Add(correct);

        while (choicesSet.Count < 5)
        {
            int fake;
            fake = correct + rand.Next(-10, 11);
            choicesSet.Add(fake);
        }
        choices.Clear();
        choices.AddRange(choicesSet);
        Utils.Shuffle(choices);

        return new Question
        {
            Problem = $"{a} {op} {b} = ?",
            CorrectAnswer = correct,
            Choices = choices
        };
    }
}