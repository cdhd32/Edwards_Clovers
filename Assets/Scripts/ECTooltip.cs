using UnityEngine;

public class ECTooltip : MonoBehaviour
{
    public PlayerStatType type;
    public GameObject panel;

    private void OnEnable()
    {
        ShowTooltip();
    }
    public void ShowTooltip()
    {
        ECPlayerStatManager manager = ECPlayerStatManager.Instance;
        if(manager.GetPlayerStat(type) == 0)
        {
            panel.SetActive(true);
        }
    }
}
