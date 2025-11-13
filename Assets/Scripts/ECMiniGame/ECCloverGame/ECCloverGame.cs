using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ECCloverGame : ECMiniGameBase
{
    public RectTransform cloverParent;
    private ECClover[] clovers;
    private int totalCloverCount = 30;
    //손을 드래그로 움직이기
    //잡아서 옮길 수 있음

    public Texture2D maskTexture;

    public RectTransform panel;
    public ECClover threeLeafPrefab;
    public ECClover fourLeafPrefab;

    public Sprite[] threeLeafSprites;
    public Sprite[] fourLeafSprites;

    public Image[] cloverUI;

    public int cloverCount = 10;
    public int fourLeafCloverCount = 3;
    public float cloverPadding = 50f;

    private int foundCloverCount;
    private int befCloverIndex = -1;

    private int[] correctIndex;

    public AudioClip clickSFX;     // 재생할 효과음
    private AudioSource audioSource;

    private void Awake()
    {
        correctIndex = new int[fourLeafCloverCount];
        StartGame();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    public override void StartGame()
    {
        base.StartGame();
        SpawnClovers();
    }

    public override EResultState GetScore()
    {
        EResultState state = EResultState.Count;
        if(foundCloverCount == 1)
        {
            state = EResultState.Good;
        }
        else if(foundCloverCount == 2)
        {
            state = EResultState.Great;
        }
        else if (foundCloverCount == 3)
        {
            state = EResultState.Great;
        }
        else if(foundCloverCount == 0)
        {
            state = EResultState.Bad;
        }
        return state;
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
            //float x = Random.Range(-panelSize.x / 2 + cloverPadding, panelSize.x / 2 - cloverPadding);
           //float y = Random.Range(-panelSize.y / 2 + cloverPadding, panelSize.y / 2 - cloverPadding);
            Vector2 position = GetRandomPointFromMask();
            // position = new Vector2(x, y);

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
            //rt.pivot = new Vector2(0.5f, -0.5f);

            int index = i;
            if (isFourLeaf)
            {
                Button btn = clover.cloverBtn;
                btn.onClick.AddListener(() => OnCloverClicked(index));
                
                clover.SetSprite(fourLeafSprites[Random.Range(0,fourLeafSprites.Length)]);
                clover.ChangeCloverState(true);
            }
            else
            {
                clover.SetSprite(threeLeafSprites[Random.Range(0, threeLeafSprites.Length)]);
            }
        }
    }

    Vector2 GetRandomPointFromMask()
    {
        int width = maskTexture.width;
        int height = maskTexture.height;

        for (int i = 0; i < 1000; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (maskTexture.GetPixel(x, y).a > 0.5f) // 흰색 영역
            {
                Vector2 uiPos = new Vector2(
                    (float)x / width * Screen.width,
                    (float)y / height * Screen.height
                );
                uiPos.x = uiPos.x - (Screen.width /2);
                uiPos.y = uiPos.y - (Screen.height / 2);
                return uiPos;
            }
        }

        return Vector2.zero;
    }

    private void OnCloverClicked(int index)
    {
        if(index == befCloverIndex)
        {
            return;
        }
        bool isFourLeaf = false;
        for (int j = 0; j < correctIndex.Length; ++j)
        {
            if (index == correctIndex[j])
            {
                befCloverIndex = index;
                isFourLeaf = true;
                break;
            }
        }

        if(isFourLeaf)
        {
            cloverUI[foundCloverCount].color = Color.white;
            foundCloverCount++;
            ECClover clover = clovers[index];
            clover.transform.SetAsLastSibling();
            clover.FindCloverEvent();
            if (foundCloverCount == 2)
                audioSource.pitch = 0.5f;
            else
                audioSource.pitch = 1f;
            audioSource.PlayOneShot(clickSFX);
            if (foundCloverCount == fourLeafCloverCount)
            {
                timer.EndTimer(EResultState.Perfect);
            }
        }
        else
        {
            Debug.Log("세잎");
        }
    }
}
