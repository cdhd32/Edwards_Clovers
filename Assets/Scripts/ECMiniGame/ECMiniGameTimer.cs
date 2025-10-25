using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ECMiniGameTimer : MonoBehaviour
{
    public Slider timeSlider;
    public float totalStageTime = 30;
    private float currentTime = 30;
    private bool isStart = false;
    public bool IsStart => isStart;
    public ECResultPanel resultPanel;
    public ECMiniGameBase miniGameBase;
    public TextMeshProUGUI tmp;
    public EventType gameType;

    private void Awake()
    {
        
    }

    public void StartTimer()
    {
        currentTime = totalStageTime;
        timeSlider.maxValue = totalStageTime;
        timeSlider.minValue = 0;
        timeSlider.value = timeSlider.maxValue;
        isStart = true;
    }

    public void EndTimer()
    {
        EResultState state = miniGameBase.GetScore();
        resultPanel.ShowResult(state, gameType);
    }

    public void EndTimer(EResultState _state)
    {
        isStart = false;
        resultPanel.ShowResult(_state, gameType);
    }

    private void Update()
    {
        if (isStart)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                currentTime = 0;
                timeSlider.value = 0;
                isStart = false;
                EndTimer();
            }
            int intTime = (int)currentTime;
            tmp.SetText(intTime.ToString() + "ÃÊ");
            timeSlider.value = currentTime;
        }
    }
}
