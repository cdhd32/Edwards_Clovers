using UnityEngine;
using UnityEngine.EventSystems;

public class ECTiltPourHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Smooth Settings")]
    public float rotateSmoothSpeed = 8f; //각도 움직이는 속도

    [Header("References")]
    public RectTransform handleRect;    
    public RectTransform pivotRect;     
    public ECScienceTube testTube;            
    public ECScienceFlask flask;          

    [Header("Pour Settings")]
    private float minAngle = 0f;       
    private float maxAngle = -90f;       
    private float pourThreshold = 21f;    
    public float basePourRate = 0.5f;     // 붓는 속도
    public float pourCurveMultiplier = 1.5f; // 붓는 가속도

    public RectTransform maxTube;
    public RectTransform minTube;

    public ECLiquidSpawner liquidSpanwer;

    private Canvas parentCanvas;
    private float currentAngle = 0f;

    private float targetAngle = 0f; 
    public int answerCount;

    void Awake()
    {
        if (handleRect == null)
            handleRect = GetComponent<RectTransform>();

        parentCanvas = GetComponentInParent<Canvas>();
        minAngle = pivotRect.rotation.eulerAngles.z;
        maxAngle = maxTube.rotation.eulerAngles.z;
    }

    void Update()
    {
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotateSmoothSpeed);
        pivotRect.localEulerAngles = new Vector3(0, 0, currentAngle);
        if (currentAngle > pourThreshold)
        {
            float amountThisFrame = CalculatePourAmount(Time.deltaTime);
            float removed = testTube.RemoveECLiquid(amountThisFrame);
            Debug.Log(removed);
            int val = (int)(removed * 500);
            Debug.Log(val);
            liquidSpanwer.SpawnTea(val);
            if (removed > 0f)
                flask.AddECLiquid(removed);
        }

        if(currentAngle == minAngle)
        {
            if(liquidSpanwer.Count == answerCount)
            {
                EndGame();
            }
        }
    }

    private void EndGame()
    {

    }

    float CalculatePourAmount(float deltaTime)
    {
        float angleExcess = Mathf.Abs(currentAngle) - Mathf.Abs(pourThreshold);
        angleExcess = Mathf.Max(0f, angleExcess);

        float rate = basePourRate + angleExcess * pourCurveMultiplier * 0.01f;
        return rate * deltaTime;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        Camera cam = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            pivotRect, eventData.position, cam, out localPoint))
        {
            float angle = Mathf.Atan2(localPoint.y, localPoint.x) * Mathf.Rad2Deg;
            float desiredAngle = Mathf.Clamp(angle, minAngle, maxAngle);
            targetAngle = desiredAngle;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        targetAngle = minAngle;
    }
}
