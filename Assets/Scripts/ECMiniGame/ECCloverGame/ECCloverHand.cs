using Unity.VisualScripting;
using UnityEngine;

public class ECCloverHand : MonoBehaviour
{
    private RectTransform rt;

    private void Awake()
    {
         rt = GetComponent<RectTransform>();    
    }
    private void Update()
    {
        Vector2 pos = Input.mousePosition;
        rt.position = pos;
    }
}
