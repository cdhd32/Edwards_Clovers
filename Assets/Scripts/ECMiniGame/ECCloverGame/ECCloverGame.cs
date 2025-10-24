using UnityEngine;
using UnityEngine.UI;

public class ECCloverGame : ECMiniGameBase
{
    public RectTransform cloverParent;
    private int totalCloverCount = 30;
    //손을 드래그로 움직이기
    //잡아서 옮길 수 있음

    public RectTransform panel;
    public ECClover threeLeafPrefab;
    public ECClover fourLeafPrefab;

    public int cloverCount = 10;
    public float cloverPadding = 50f;

    private int correctIndex;

    private void Awake()
    {
        StartGame();
    }

    public override void StartGame()
    {
        base.StartGame();
        SpawnClovers();
    }

    private void SpawnClovers()
    {
        Vector2 panelSize = panel.rect.size;

        correctIndex = Random.Range(0, cloverCount);

        for (int i = 0; i < cloverCount; i++)
        {
            float x = Random.Range(-panelSize.x / 2 + cloverPadding, panelSize.x / 2 - cloverPadding);
            float y = Random.Range(-panelSize.y / 2 + cloverPadding, panelSize.y / 2 - cloverPadding);
            Vector2 position = new Vector2(x, y);

            ECClover prefab = (i == correctIndex) ? fourLeafPrefab : threeLeafPrefab;

            ECClover clover = Instantiate(prefab, panel);
            RectTransform rt = clover.GetComponent<RectTransform>();
            rt.anchoredPosition = position;
            rt.pivot = new Vector2(0.5f, -0.5f);

            int index = i;
            if (correctIndex == index)
            {
                Button btn = clover.GetComponent<Button>();
                btn.onClick.AddListener(() => OnCloverClicked(index));
            }
        }
    }

    private void OnCloverClicked(int index)
    {
        if (index == correctIndex)
        {
            Debug.Log("찾음");

        }
    }
}
