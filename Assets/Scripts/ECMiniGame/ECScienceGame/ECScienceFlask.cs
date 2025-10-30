using UnityEngine;
using UnityEngine.UI;

public class ECScienceFlask : MonoBehaviour
{
    [Header("UI")]
    public Image liquidImage; // Image Type = Filled

    [Header("ECLiquid")]
    public float maxAmount = 1.0f;
    [Range(0f, 1f)]
    public float currentAmount = 0f;

    void Start()
    {
        UpdateUI();
    }

    public float AddECLiquid(float amount)
    {
        float accept = Mathf.Min(maxAmount - currentAmount, amount);
        currentAmount += accept;
        currentAmount = Mathf.Clamp(currentAmount, 0f, maxAmount);
        Invoke("UpdateUI", 2f);
        return accept;
    }

    public void UpdateFillAmount()
    {

    }

    public void UpdateUI()
    {
        if (liquidImage != null)
        {
            liquidImage.fillAmount = Mathf.Clamp01(currentAmount / maxAmount);
        }
    }
}
