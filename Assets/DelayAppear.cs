using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DelayAppear : MonoBehaviour
{
    public float dealyTime;
    private void OnEnable()
    {
        StartCoroutine(Appear());
    }
    IEnumerator Appear()
    {
        yield return new WaitForSeconds(dealyTime);
        gameObject.GetComponent<Image>().color = new Vector4(255, 255, 255, 255);
    }
}
