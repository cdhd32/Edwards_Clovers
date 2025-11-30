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
        perfect = new int[2] { 60, 100 };
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
        spawnPoint = SpawnPoint.position;
        for (int i = 0; i < totalSize; i++)
        {
            tea = Instantiate(Tea, spawnPoint, Quaternion.identity, transform);
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
        Debug.Log("°á°ú" + count);
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

}
