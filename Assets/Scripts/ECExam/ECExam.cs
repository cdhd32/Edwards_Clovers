using UnityEngine;

public class ECExam : MonoBehaviour
{
    public ECExamReportBox[] reportBox;
    public ECPaperStacker stacker;
    private int currentStage = 0;
    private int[] scorethreshold = new int[3] { 150, 225, 375 };
    private int currentThreshold;

    private int[] playerScores = new int[4]; // 국수과영
    private int[] playerRank = new int[4]; // 국수과영

    private bool isClick = true;

    private void Awake()
    {
        int leftDay = ECPlayerStatManager.Instance.GetPlayerStat(PlayerStatType.LEFTDAY);
        //if(leftDay!=0)
        //{
        //    leftDay++;
        //}
        if (leftDay == 5)
        {
            currentStage = 0;
        }
        else if (leftDay == 2)
        {
            currentStage = 1;
        }
        else if (leftDay == 0)
        {
            currentStage = 2;
            //중간고사
        }
        ////임시
        //currentStage = 0;
        currentThreshold = scorethreshold[currentStage];
        SetReportBoxs();
        Invoke("ButtonActive", 2f);
    }

    private void ButtonActive()
    {
        isClick = false;
    }

    public void OnClickMainScene()
    {
        if (isClick) return;
        if(currentStage == 2)
        {
            Debug.Log("이따 엔딩컷신으로 연결");
            ECGlobalSceneManager.Instance.LoadScene(SceneType.ENDING);
            return;
        }
        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
        isClick = true;
    }

    private void SetReportBoxs()
    {
        ECPlayerStatManager manager = ECPlayerStatManager.Instance;

        int krScore = manager.GetPlayerStat(PlayerStatType.KOR);
        playerScores[0] = GetScore(krScore);

        int mathScore = manager.GetPlayerStat(PlayerStatType.MATH);
        playerScores[1] = GetScore(mathScore);

        int sciScore = manager.GetPlayerStat(PlayerStatType.SCI);
        playerScores[2] = GetScore(sciScore);
        int engScore = manager.GetPlayerStat(PlayerStatType.ENG);
        playerScores[3] = GetScore(engScore);
        int luckStat = manager.GetPlayerStat(PlayerStatType.LUK);
        int plusVal = 0;
        if (luckStat >= 20)
        {
            while (luckStat > 0)
            {
                plusVal++;
                luckStat -= 20;
            }
        }


        for (int i = 0; i < playerScores.Length; i++)
        {
            playerScores[i] += plusVal;
            if (playerScores[i] > 100)
            {
                playerScores[i] = 100;
            }
            playerRank[i] = GetRank(playerScores[i]);
        }

        //과수 순서바꿔서
        reportBox[0].SetReportBox("국어", playerScores[0], playerRank[0]);
        reportBox[2].SetReportBox("수학", playerScores[1], playerRank[1]);
        reportBox[1].SetReportBox("과학", playerScores[2], playerRank[2]);
        reportBox[3].SetReportBox("영어", playerScores[3], playerRank[3]);

    }

    private int GetScore(int stat)
    {
        int score = 0;
        if (stat >= currentThreshold)
        {
            score = 100;
            return score;
        }
        else
        {
            while (stat > 0)
            {
                stat -= 10;
                score--;
            }
        }


        return 0;
    }

    private int GetRank(int score)
    {
        int rank = 0;
        if (score > 94)
        {
            rank = 1;
        }
        else if (score < 95 && score > 89)
        {
            rank = 2;
        }
        else if (score < 90 && score > 84)
        {
            rank = 3;
        }
        else if (score < 85 && score > 79)
        {
            rank = 5;
        }
        else if (score < 80 && score > 64)
        {
            rank = 12;
        }
        else if (score < 65 && score > 39)
        {
            rank = 32;
        }
        else if (score < 40)
        {
            rank = 78;
        }

        return rank;
    }
}
