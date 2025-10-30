using UnityEngine;
using UnityEngine.UI;

public class ECScienceTube : MonoBehaviour
{
    [Header("UI")]
    public Image liquidImage; // Image Type = Filled (Vertical or Horizontal depending art)

    [Header("ECLiquid")]
    public int maxAmount = 300;
    public int currentAmount = 300;

    void Start()
    {
        UpdateUI();
    }

    public int RemoveECLiquid(int amount)
    {
        // amount는 빼려는 양 (단위: amount per second * deltaTime)
        int removed = Mathf.Min(currentAmount, amount);
       // removed = (int)(removed * 0.5f);
        currentAmount -= removed;
        currentAmount = Mathf.Max(0, currentAmount);
        UpdateUI();
        return removed;
    }

    //public void AddECLiquid(float amount)
    //{
    //    currentAmount += amount;
    //    currentAmount = Mathf.Min(maxAmount, currentAmount);
    //    UpdateUI();
    //}

    private void UpdateUI()
    {
        if (liquidImage != null)
        {
            float val = Mathf.Clamp01((float)currentAmount / maxAmount);
            liquidImage.fillAmount = val;
        }
    }
}
