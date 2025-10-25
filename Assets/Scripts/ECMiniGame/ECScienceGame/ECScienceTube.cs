using UnityEngine;
using UnityEngine.UI;

public class ECScienceTube : MonoBehaviour
{
    [Header("UI")]
    public Image liquidImage; // Image Type = Filled (Vertical or Horizontal depending art)

    [Header("ECLiquid")]
    public float maxAmount = 2.0f;
    public float currentAmount = 2.0f;

    void Start()
    {
        UpdateUI();
    }

    public float RemoveECLiquid(float amount)
    {
        // amount는 빼려는 양 (단위: amount per second * deltaTime)
        float removed = Mathf.Min(currentAmount, amount);
        currentAmount -= removed;
        currentAmount = Mathf.Max(0f, currentAmount);
        UpdateUI();
        return removed;
    }

    public void AddECLiquid(float amount)
    {
        currentAmount += amount;
        currentAmount = Mathf.Min(maxAmount, currentAmount);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (liquidImage != null)
        {
            // fillAmount 0~1로 매핑
            liquidImage.fillAmount = Mathf.Clamp01(currentAmount / maxAmount);
        }
    }
}
