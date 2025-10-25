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

    private void Awake()
    {
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
            randNum = Random.Range(0, 1);
            //spawnPoint = randNum == 0 ? transform.position : SpawnPoint.position;
            spawnPoint = SpawnPoint.position;

            tea = Instantiate(Tea, spawnPoint, Quaternion.identity);
            tea.transform.SetParent(transform);
            tea.gameObject.SetActive(false);
            circles.Add(tea);
        }

    }

    public void SpawnTea(int val)
    {
        if (count + val > circles.Count - 1) return;
        for (int i = 0; i < val; i++)
        {
            circles[count].gameObject.SetActive(true);
            count++;
        }
        //Debug.Log(count);
    }

    public EResultState ReturnGameResult()
    {
        EResultState result = EResultState.Count;
        if (count >= 160 && count <= 180)
        {
            result = EResultState.Perfect;
        }
        else if ((count <= 190 && count > 180) || (count < 160 && count > 150))
        {
            result = EResultState.Great;
        }
        else if ((count < 200 && count > 190) || (count < 150 && count > 140))
        {
            result = EResultState.Good;
        }
        else
        {
            result = EResultState.Bad;
        }
        return result;
    }

    public void SpawnTea()
    {
        if (count > circles.Count - 1) return;
        for (int i = 0; i < 2; i++)
        {
            circles[count].gameObject.SetActive(true);
            count++;
        }



        if (count.Equals(90))
        {
            Debug.Log("Á¤´ä");
            //Smoke.Play();
        }

    }

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
