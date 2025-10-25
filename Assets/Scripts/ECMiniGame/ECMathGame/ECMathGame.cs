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
        answerCheck.sprite = null;
        if (currentSelectBox != null)
        {
            currentSelectBox.ShowCheckImage(false);
            currentSelectBox = null;
        }
        currentQuestion = MathQuiz.GenerateQuestion();
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
        box.ShowCheckImage(true);
        currentSelectBox = box;
        if (selectedAnswer == currentQuestion.CorrectAnswer)
        {
            Debug.Log("정답");
            answerCheck.sprite = answerCheckImages[1];
            currentScore++;
        }
        else
        {
            answerCheck.sprite = answerCheckImages[0];
            currentScore -= 3;
            Debug.Log("오답");
        }

        Invoke(nameof(GenerateNewQuestion), 0.5f);
    }
}

public class MathQuiz
{
    private static System.Random rand = new System.Random();

    public class Question
    {
        public string Problem;
        public int CorrectAnswer;
        public List<int> Choices;
    }

    public static Question GenerateQuestion()
    {
        string[] operators = { "+", "-", "*", "/" };
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

        HashSet<int> choicesSet = new HashSet<int> { correct };

        while (choicesSet.Count < 5)
        {
            int fake;
            do
            {
                fake = correct + rand.Next(-10, 11);
            } while (fake == correct || fake < 0);

            choicesSet.Add(fake);
        }

        List<int> choices = new List<int>(choicesSet);
        Utils.Shuffle(choices);

        return new Question
        {
            Problem = $"{a} {op} {b} = ?",
            CorrectAnswer = correct,
            Choices = choices
        };
    }
}