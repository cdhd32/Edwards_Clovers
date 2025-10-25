using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ECCloverGame : ECMiniGameBase
{
    public RectTransform cloverParent;
    private ECClover[] clovers;
    private int totalCloverCount = 30;
    //손을 드래그로 움직이기
    //잡아서 옮길 수 있음

    public RectTransform panel;
    public ECClover threeLeafPrefab;
    public ECClover fourLeafPrefab;

    public Sprite[] threeLeafSprites;
    public Sprite[] fourLeafSprites;

    public int cloverCount = 10;
    public int fourLeafCloverCount = 3;
    public float cloverPadding = 50f;

    private int foundCloverCount;

    private int[] correctIndex;

    private void Awake()
    {
        correctIndex = new int[fourLeafCloverCount];
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

        var list = Utils.GetUniqueRandomNumbers(0, cloverCount);
        for (int i=0; i< list.Count; ++i)
        {
            correctIndex[i] = list[i];
        }
        clovers = new ECClover[cloverCount];

        for (int i = 0; i < cloverCount; i++)
        {
            float x = Random.Range(-panelSize.x / 2 + cloverPadding, panelSize.x / 2 - cloverPadding);
            float y = Random.Range(-panelSize.y / 2 + cloverPadding, panelSize.y / 2 - cloverPadding);
            Vector2 position = new Vector2(x, y);

            bool isFourLeaf = false;
            for(int j = 0; j<correctIndex.Length; ++j)
            {
                if(i == correctIndex[j])
                {
                    isFourLeaf = true;
                    break;
                }
            }

            ECClover prefab = isFourLeaf ? fourLeafPrefab : threeLeafPrefab;

            ECClover clover = Instantiate(prefab, panel);
            clovers[i] = clover;
            RectTransform rt = clover.GetComponent<RectTransform>();
            rt.anchoredPosition = position;
            rt.pivot = new Vector2(0.5f, -0.5f);

            int index = i;
            if (isFourLeaf)
            {
                Debug.Log("나는네잎클로버");
                Button btn = clover.GetComponent<Button>();
                btn.onClick.AddListener(() => OnCloverClicked(index));
                clover.SetSprite(fourLeafSprites[Random.Range(0,fourLeafSprites.Length)]);
            }
            else
            {
                clover.SetSprite(threeLeafSprites[Random.Range(0, threeLeafSprites.Length)]);
            }
        }
    }

    private void OnCloverClicked(int index)
    {
        bool isFourLeaf = false;
        for (int j = 0; j < correctIndex.Length; ++j)
        {
            if (index == correctIndex[j])
            {
                isFourLeaf = true;
                break;
            }
        }

        if(isFourLeaf)
        {
            foundCloverCount++;
            ECClover clover = clovers[index];
            clover.transform.SetAsLastSibling();
            clover.FindCloverEvent();
            Debug.Log("네잎");
        }
        else
        {
            Debug.Log("세잎");
        }
    }
}
