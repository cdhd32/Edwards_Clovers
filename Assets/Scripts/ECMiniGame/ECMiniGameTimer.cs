using UnityEngine;
using UnityEngine.UI;

public class ECMiniGameTimer : MonoBehaviour
{
    public Slider timeSlider;
    public float totalStageTime = 30;
    private float currentTime = 30;
    private bool isStart = false;

    public void StartTimer()
    {
        currentTime = totalStageTime;
        timeSlider.maxValue = totalStageTime;
        timeSlider.minValue = 0;
        timeSlider.value = timeSlider.maxValue;
        isStart = true;
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
            }
            timeSlider.value = currentTime;
        }
    }
}
