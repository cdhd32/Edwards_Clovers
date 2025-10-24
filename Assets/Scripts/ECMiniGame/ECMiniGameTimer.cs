using System;
using UnityEngine;
using UnityEngine.UI;

public class ECMiniGameTimer : MonoBehaviour
{
    public Slider timeSlider;
    public float totalStageTime = 30;
    private float currentTime = 30;
    private bool isStart = false;
    public ECResultPanel resultPanel;
    public ECMiniGameBase miniGameBase;

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
        resultPanel.ShowResult(state);
    }

    private void Update()
    {
        if (isStart)
        {
            currentTime -= Time.deltaTime;
            if(currentTime < 0)
            {
                currentTime = 0;
                timeSlider.value = 0;
                isStart = false;
                EndTimer();
            }
            timeSlider.value = currentTime;
        }
    }
}
