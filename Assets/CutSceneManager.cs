using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CutSceneManager : MonoBehaviour, IPointerDownHandler
{
    public List<GameObject> cutsceneObjects;
    public List<int> groupSizeList;

    private int groupNumIndex = 0;  // 현재 그룹 인덱스
    private int groupClickCount = 0; // 현재 그룹 내 클릭 횟수
    private int globalIndex = 0;     // 전체 컷씬 인덱스 (cutsceneObjects 기준)

    public void OnPointerDown(PointerEventData eventData)
    {
        // 모든 컷씬이 끝났다면 종료
        if (groupNumIndex >= groupSizeList.Count || globalIndex >= cutsceneObjects.Count)
        {
            gameObject.SetActive(false);
            return;
        }

        int groupSize = groupSizeList[groupNumIndex];

        // "끄는 단계" (그룹 내 마지막 컷씬까지 보여준 뒤 한 번 더 클릭)
        if (groupClickCount == groupSize)
        {
            // 현재 그룹 컷씬 비활성화
            for (int i = globalIndex - groupSize; i < globalIndex; i++)
            {
                if (i >= 0 && i < cutsceneObjects.Count)
                    cutsceneObjects[i].SetActive(false);
            }

            // 다음 그룹으로 이동
            groupNumIndex++;
            groupClickCount = 0;

            // 만약 모든 그룹을 끝냈다면 종료
            if (groupNumIndex >= groupSizeList.Count)
            {
                gameObject.SetActive(false);
                return;
            }

            return; // 끄기 단계 클릭은 컷씬 활성화 안 함
        }

        // 현재 그룹에서 켜야 할 컷씬 활성화
        if (globalIndex < cutsceneObjects.Count)
        {
            cutsceneObjects[globalIndex].SetActive(true);
            globalIndex++;
            groupClickCount++;
        }
    }
}
