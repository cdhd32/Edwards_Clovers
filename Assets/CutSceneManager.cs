
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CutSceneManager : MonoBehaviour, IPointerDownHandler
{
    public List<GameObject> cutsceneObjects;  
    public List<int> groupSizeList;
    private int groupNumIndex = 0;
    private int clickCount = 0;
   

    public void OnPointerDown(PointerEventData eventData)
    {
        int total = cutsceneObjects.Count;
        int groupIndex = clickCount / (groupSizeList[groupNumIndex] + 1); 
        int groupStart = groupIndex * groupSizeList[groupNumIndex];
        int groupEnd = Mathf.Min(groupStart + groupSizeList[groupNumIndex], total);
        if(clickCount==total)gameObject.SetActive(false);
        // 현재 클릭이 "모두 끄는 단계"라면
        if ((clickCount + 1) % (groupSizeList[groupNumIndex] + 1) == 0)
        {
            // 전부 비활성화
            for (int i = groupStart; i < groupEnd; i++)
            {
                if (i < total)
                    cutsceneObjects[i].SetActive(false);
            }
            groupNumIndex++;
        }
        else
        {
            // 활성화할 인덱스 계산
            int activeIndex = groupStart + (clickCount % (groupSizeList[groupNumIndex] + 1));

            if (activeIndex < total)
                cutsceneObjects[activeIndex].SetActive(true);
        }

        clickCount++;
    }
}
