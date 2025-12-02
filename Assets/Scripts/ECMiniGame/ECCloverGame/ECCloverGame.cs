using System.Collections.Generic;
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
            state = EResultState.Perfect;
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
       List<Vector2> positions = GeneratePoissonPointsFromMask(cloverCount, cloverPadding);
        for (int i = 0; i < cloverCount; i++)
        {
            Vector2 position = positions[i];

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
            rt.pivot = new Vector2(0.5f, 0.5f);

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

    List<Vector2> GeneratePoissonPointsFromMask(int count, float minDistance, int maxSampleAttempts = 50)
    {
        // 결과 리스트
        List<Vector2> result = new List<Vector2>(count);

        int width = maskTexture.width;
        int height = maskTexture.height;

        int attempts = 0;
        int maxAttemptsTotal = count * maxSampleAttempts;

        while (result.Count < count && attempts < maxAttemptsTotal)
        {
            attempts++;

            // 임의 좌표 샘플링
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            // 마스크가 흰색 영역인지 검사
            if (maskTexture.GetPixel(x, y).a <= 0.5f)
                continue;

            // UI 좌표로 변환
            Vector2 point = new Vector2(
                (float)x / width * Screen.width,
                (float)y / height * Screen.height
            );

            point.x -= Screen.width / 2;
            point.y -= Screen.height / 2;

            // 이미 생성한 포인트들과 거리 비교
            bool tooClose = false;
            for (int i = 0; i < result.Count; i++)
            {
                if (Vector2.Distance(point, result[i]) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                result.Add(point);
            }
        }

        return result;
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
