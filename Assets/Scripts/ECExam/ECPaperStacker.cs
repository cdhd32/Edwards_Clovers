using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ECPaperStacker : MonoBehaviour
{
    [Header("References")]
    public RectTransform container;      // 종이가 쌓일 부모 RectTransform (Canvas 내부)
    public Image paperPrefab;       // 종이 프리팹 (UI Image)

    [Header("Stack Settings")]
    public int paperCount = 10;          // 생성할 종이 개수
    public Vector2 stackStartPos = new Vector2(0, 0); // container 로컬 좌표 기준 스택 시작점 (anchoredPosition)
    public Vector2 stackOffset = new Vector2(0, -6f); // 각 종이마다 아래로 얼마나 오프셋할지
    public float dropHeight = 600f;      // 처음 생성 위치(위)에서 얼마나 떨어뜨려 생성할지 (스크린 단위)
    public float spawnInterval = 0.08f;  // 종이 한장씩 생성될 때 딜레이

    public Sprite[] sprites;

    [Header("Animation")]
    public float dropDuration = 0.35f;   // 낙하 시간
    public Ease dropEase = Ease.OutBack; // 낙하 easing
    public float settleDuration = 0.12f; // 작은 흔들림/정리 시간
    public float scaleOnDrop = 1.02f;    // 떨어질 때 살짝 커지기 비율
    public Vector2 randomRotationRange = new Vector2(-6f, 6f); // 각도 랜덤 범위

    [Header("Options")]
    public bool randomizeRotation = true;
    public bool setSiblingIncreasing = true; // 먼저 생성한게 아래로 가게 하려면 true
    public CanvasGroup cg;

    private List<GameObject> spawned = new List<GameObject>();
    int count = 0;

    void Start()
    {
        // 예시로 시작 시 자동 쌓기
        StartStacking();
    }

    public void StartStacking()
    {
        StopAllCoroutines();
        foreach (var go in spawned) if (go) Destroy(go);
        spawned.Clear();
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < paperCount; i++)
        {
            SpawnOne(i);
            yield return new WaitForSeconds(spawnInterval);
        }
        cg.DOFade(0, 0.5f);
    }

    void SpawnOne(int index)
    {
        // 1) 인스턴스화
        Image go = Instantiate(paperPrefab, container);
        if(count == 0)
        {
            go.sprite = sprites[Random.Range(0, 4)];
            go.color = Color.white;
        }
        count++;
        RectTransform rt = go.GetComponent<RectTransform>();

        // 2) 타겟 로컬 포지션 계산 (container의 anchoredPosition 기준)
        Vector2 targetPos = stackStartPos + stackOffset * index;

        // 3) 초기 위치는 targetPos + dropHeight 위 (local 좌표)
        Vector2 initialPos = targetPos + new Vector2(0, dropHeight);
        rt.anchoredPosition = initialPos;

        // 4) 초기 회전/스케일
        float randomRot = randomizeRotation ? Random.Range(randomRotationRange.x, randomRotationRange.y) : 0f;
        rt.localEulerAngles = new Vector3(0, 0, randomRot);
        rt.localScale = Vector3.one * scaleOnDrop;

        // 5) sibling index 조정 (겹침 순서 제어)
        if (setSiblingIncreasing)
        {
            // 먼저 생성한게 아래에 있도록
            rt.SetSiblingIndex(0);
        }
        else
        {
            rt.SetAsLastSibling();
        }

        spawned.Add(go.gameObject);

        // 6) 애니메이션: 낙하 -> 약간 튕김/정리 -> 최종 스케일/회전
        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOAnchorPos(targetPos, dropDuration).SetEase(dropEase));
        seq.Join(rt.DOLocalRotate(new Vector3(0, 0, randomRot), dropDuration).SetEase(Ease.Linear));
        seq.Join(rt.DOScale(Vector3.one, dropDuration * 0.7f).SetEase(Ease.OutQuad));
        // 정돈 효과 (작은 흔들림)
        seq.AppendInterval(0f);
        seq.Append(rt.DOLocalRotate(new Vector3(0, 0, randomRot * 0.5f), settleDuration).SetEase(Ease.OutSine));
        seq.Append(rt.DOLocalRotate(new Vector3(0, 0, randomRot), settleDuration).SetEase(Ease.InOutSine));
        // 완료 콜백(선택적으로 사운드 등)
        seq.OnComplete(() => {
        });
    }

    private void DisablePapers()
    {
        gameObject.SetActive(false);
    }

    // 선택적으로: 모든 종이를 한꺼번에 정리(예: 쌓인 상태에서 한 장씩 제거)
    public void ClearStack()
    {
        foreach (var go in spawned) if (go) Destroy(go);
        spawned.Clear();
    }
}
