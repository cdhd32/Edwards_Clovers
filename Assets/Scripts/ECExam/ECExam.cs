using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ECExam : MonoBehaviour
{
    public ECExamReportBox[] reportBox;
    private int currentStage = 0;
    private int[] scorethreshold = new int[3] { 375, 375, 375 };
    private int currentThreshold;

    private int[] playerScores = new int[4]; // 국수과영
    private int[] playerStats = new int[5]; // 국수과영+행운
    private int[] playerRank = new int[4]; // 국수과영

    public Slider[] edwardSliders;
    public Slider[] enemySliders;
    public Image[] edwardMaxImage;
    public TextMeshProUGUI buttonTMP;

    public GameObject defaultPanel;
    public GameObject lastExamPanel;

    public int ENEMY_SCORE = 400;
    public int MAX_SCORE = 600;
    public float graphDuration = 2;

    private bool isClick = true;
    public GameObject Btn;

    private void Awake()
    {
        int leftDay = ECPlayerStatManager.Instance.GetPlayerStat(PlayerStatType.LEFTDAY);
        leftDay++;

        if (leftDay == ECConst.UNIT_TEST_DAY_1)
        {
            currentStage = 1;
        }
        else if (leftDay == ECConst.MIDTERM_DAY)
        {
            currentStage = 2;
            buttonTMP.SetText("엔딩으로");
            //중간고사
        }

        Debug.Log("남은 날짜:" + leftDay);
        currentThreshold = scorethreshold[currentStage];



        //나중에 주석풀기
        if (currentStage != 2)
        {
            defaultPanel.SetActive(true);
            lastExamPanel.SetActive(false);
            SetReportBoxs();
        }
        else
        {
            defaultPanel.SetActive(false);
            lastExamPanel.SetActive(true);
        }
        for (int i = 0; i < 4; ++i)
        {
            edwardSliders[i].maxValue = MAX_SCORE;
            enemySliders[i].maxValue = MAX_SCORE;
            enemySliders[i].value = ENEMY_SCORE;
        }
        SetEndingBox();
        ButtonActive();
        //}
        //Invoke("ButtonActive", 2f);
    }

    private void ButtonActive()
    {
        isClick = false;
        Btn.SetActive(true);
        RectTransform rect = Btn.transform as RectTransform;
        rect.DOLocalMoveX(650, 2).SetEase(Ease.OutCubic);
    }

    public void OnClickMainScene()
    {
        if (isClick) return;
        if (currentStage == 2)
        {
            ECGlobalSceneManager.Instance.LoadScene(SceneType.ENDING);
            return;
        }
        ECGlobalSceneManager.Instance.LoadScene(SceneType.MAIN);
        isClick = true;
    }

    private void SetEndingBox()
    {
        GetPlayerScores();
        int luckScore = playerStats[4] / 3;
        float[] resultval = new float[4];
        resultval[0] = playerStats[0] + luckScore;
        resultval[1] = playerStats[2] + luckScore;
        resultval[2] = playerStats[1] + luckScore;
        resultval[3] = playerStats[3] + luckScore;
        edwardSliders[0].DOValue(resultval[0], graphDuration);
        edwardSliders[1].DOValue(resultval[1], graphDuration);
        edwardSliders[2].DOValue(resultval[2], graphDuration);
        edwardSliders[3].DOValue(resultval[3], graphDuration);

        int result = 0;
        for (int i = 0; i < resultval.Length; i++)
        {
            if (resultval[i] >= ENEMY_SCORE)
            {
                if (edwardSliders[i].value == MAX_SCORE)
                {
                    edwardMaxImage[i].enabled = true;
                }
                result++;
            }
        }

        if (result >= 3)
        {
            PlayerPrefs.SetInt("examResult", 0);
            Debug.Log("승리!");
        }
        else
        {
            PlayerPrefs.SetInt("examResult", 1);
            Debug.Log("패배!");
        }
    }

    private void GetPlayerScores()
    {
        ECPlayerStatManager manager = ECPlayerStatManager.Instance;

        playerStats[0] = manager.GetPlayerStat(PlayerStatType.KOR);
        playerScores[0] = GetScore(playerStats[0]);

        playerStats[1] = manager.GetPlayerStat(PlayerStatType.MATH);
        playerScores[1] = GetScore(playerStats[1]);

        playerStats[2] = manager.GetPlayerStat(PlayerStatType.SCI);
        playerScores[2] = GetScore(playerStats[2]);
        playerStats[3] = manager.GetPlayerStat(PlayerStatType.ENG);
        playerScores[3] = GetScore(playerStats[3]);
        int luckStat = manager.GetPlayerStat(PlayerStatType.LUK);
        playerStats[4] = luckStat;
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
    }

    private void SetReportBoxs()
    {
        GetPlayerScores();
        //과수 순서바꿔서
        reportBox[0].SetReportBox("국어", playerScores[0], playerRank[0]);
        reportBox[2].SetReportBox("수학", playerScores[1], playerRank[1]);
        reportBox[1].SetReportBox("과학", playerScores[2], playerRank[2]);
        reportBox[3].SetReportBox("영어", playerScores[3], playerRank[3]);
    }

    private int GetScore(int stat)
    {
        int score = 100;
        if (stat >= currentThreshold)
        {
            return score;
        }
        else
        {
            int diff = currentThreshold - stat;
            while (diff > 0)
            {
                diff -= 10;
                score--;
            }
        }
        return score;
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
            rank = UnityEngine.Random.Range(12,25);
        }
        else if (score < 65 && score > 39)
        {
            rank = UnityEngine.Random.Range(28,45);
        }
        else if (score < 40)
        {
            rank = UnityEngine.Random.Range(56, 78);
        }

        return rank;
    }
}
