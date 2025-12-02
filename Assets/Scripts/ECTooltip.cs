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
        if (manager.playerStats == null) return;
        if(manager.GetPlayerStat(type) == 0)
        {
            Time.timeScale = 0;
            panel.SetActive(true);
        }
    }

    public void OnClicKBtn_Close()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
    }
}
