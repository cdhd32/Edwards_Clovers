using System.Collections.Generic;
using UnityEngine;

public class ECLiquidSpawner : MonoBehaviour
{
    public ECLiquid Tea;
    public Transform SpawnPoint;

    private List<ECLiquid> circles;
    private int randNum;
    private int count;
    public int Count => count;
    private Vector3 spawnPoint;
    private ECLiquid tea;
    public int totalSize = 100;
    public Transform tubeEdgeCollider;
    public Transform rotTransform;
    private bool isCheck;
    private int[] perfect;
    private int[] great;
    private int[] good;

    private void Awake()
    {
        perfect = new int[2] { 70, 110 };
        great = new int[2] { 40, 140 };
        good = new int[2] { 10, 170 };
        circles = new List<ECLiquid>();
        count = 0;
        MakeTea();
    }

    private void Update()
    {
        tubeEdgeCollider.rotation = rotTransform.rotation;
    }
    private void MakeTea()
    {
        for (int i = 0; i < totalSize; i++)
        {
            spawnPoint = SpawnPoint.position;
            tea = Instantiate(Tea, spawnPoint, Quaternion.identity);
            tea.transform.SetParent(transform);
            tea.gameObject.SetActive(false);
            circles.Add(tea);
        }

    }

    public void SpawnTea(int val)
    {
        if (count + val > circles.Count - 1)
        {
            if (!isCheck)
            {
                val = circles.Count - count;
                isCheck = true;
            }
            else
            {
                return;
            }
        }

        for (int i = 0; i < val; i++)
        {
            circles[count].gameObject.SetActive(true);
            count++;
        }
    }

    public EResultState ReturnGameResult()
    {
        EResultState result = EResultState.Count;
        Debug.Log("결과" + count);
        if (count >= perfect[0] && count <= perfect[1])
        {
            result = EResultState.Perfect;
        }
        else if ((count <= great[1] && count > perfect[1]) || (count < perfect[0] && count > great[0]))
        {
            result = EResultState.Great;
        }
        else if ((count < good[1] && count > great[1]) || (count < great[0] && count > good[0]))
        {
            result = EResultState.Good;
        }
        else
        {
            result = EResultState.Bad;
        }
        return result;
    }

    //public EResultState ReturnGameResult()
    //{
    //    EResultState result = EResultState.Count;
    //    Debug.Log("결과" + count);
    //    if (count >= 140 && count <= 190)
    //    {
    //        result = EResultState.Perfect;
    //    }
    //    else if ((count <= 220 && count > 190) || (count < 140 && count > 120))
    //    {
    //        result = EResultState.Great;
    //    }
    //    else if ((count < 280 && count > 220) || (count < 120 && count > 80))
    //    {
    //        result = EResultState.Good;
    //    }
    //    else
    //    {
    //        result = EResultState.Bad;
    //    }
    //    return result;
    //}


    public void ResetTea()
    {
        count = 0;
        foreach (ECLiquid liquid in circles)
        {
            randNum = Random.Range(0, 1);
            spawnPoint = randNum == 0 ? transform.position : SpawnPoint.position;
            liquid.transform.position = spawnPoint;
            liquid.gameObject.SetActive(false);
        }
    }
}
