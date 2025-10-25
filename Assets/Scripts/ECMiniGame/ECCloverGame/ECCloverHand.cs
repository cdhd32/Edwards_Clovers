using Unity.VisualScripting;
using UnityEngine;

public class ECCloverHand : MonoBehaviour
{
    public CanvasGroup cg;
    private RectTransform rt;
    public ECMiniGameTimer timer;
    private bool isStart;
    public float smoothSpeed = 10f;

    private void Awake()
    {
         rt = GetComponent<RectTransform>();
        isStart = true;
    }
    private void Update()
    {
        if(isStart)
        {
            Vector2 pos = Input.mousePosition;
            rt.position = Vector2.Lerp(rt.position, pos, Time.deltaTime * smoothSpeed);
            if(!timer.IsStart)
            {
                isStart = false;
                cg.alpha = 0;
            }
        }
    }
}
