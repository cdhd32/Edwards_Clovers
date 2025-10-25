using Unity.VisualScripting;
using UnityEngine;

public class ECCloverHand : MonoBehaviour
{
    public CanvasGroup cg;
    private RectTransform rt;
    public ECMiniGameTimer timer;
    private bool isStart;

    private void Awake()
    {
         rt = GetComponent<RectTransform>();    
    }
    private void Update()
    {
        if(isStart)
        {
            Vector2 pos = Input.mousePosition;
            rt.position = pos;
            if(!timer.IsStart)
            {
                isStart = false;
                cg.alpha = 0;
            }
        }
    }
}
