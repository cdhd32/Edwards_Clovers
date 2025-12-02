using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ECTiltPourHandle : ECMiniGameBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Smooth Settings")]
    public float rotateSmoothSpeed = 8f; //각도 움직이는 속도

    [Header("References")]
    public RectTransform handleRect;    
    public RectTransform pivotRect;     
    public ECScienceTube testTube;            
    public ECScienceFlask flask;
    //public Image handleImage;

    [Header("Pour Settings")]
    private float minAngle = 0f;       
    private float maxAngle = -90f;       
    private float pourThreshold = 6f;    
    public float basePourRate = 0.5f;     // 붓는 속도
    public float pourCurveMultiplier = 1.5f; // 붓는 가속도

    public RectTransform maxTube;
    public RectTransform minTube;

    public ECLiquidSpawner liquidSpawner;

    private Canvas parentCanvas;
    private float currentAngle = 0f;

    private float targetAngle = 0f;
    public int answerCount = 170;
    private bool isDragEnd = false;
    private bool isPlaying = true;

    public GameObject clickPanel;

    void Awake()
    {
        if (handleRect == null)
            handleRect = GetComponent<RectTransform>();

        parentCanvas = GetComponentInParent<Canvas>();
        minAngle = pivotRect.rotation.eulerAngles.z;
        maxAngle = maxTube.rotation.eulerAngles.z;
    }

    void FixedUpdate()
    {
        if (isPlaying)
        {
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotateSmoothSpeed);
            pivotRect.localEulerAngles = new Vector3(0, 0, currentAngle);
            if (currentAngle > pourThreshold)
            {
                int amountThisFrame = CalculatePourAmount(Time.deltaTime);
                int removed = testTube.RemoveECLiquid(amountThisFrame);
               // Debug.Log(removed);
                liquidSpawner.SpawnTea(removed);
                if (removed > 0f)
                    flask.AddECLiquid(removed);
            }
        }
    }

    private void EndGame()
    {
        EResultState state = liquidSpawner.ReturnGameResult();
        Debug.Log("State" + state);
        timer.EndTimer(state);
    }
    int CalculatePourAmount(float deltaTime)
    {
        float angleExcess = Mathf.Abs(currentAngle) - Mathf.Abs(pourThreshold);
        angleExcess = Mathf.Max(0f, angleExcess);

        float rate = basePourRate + angleExcess * pourCurveMultiplier;
        int intRate = (int)rate;
        return intRate;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        clickPanel.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragEnd) return;
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
        //handleImage.color = Color.gray;
        isDragEnd = true;
        Invoke("EndEvent", 5f);
    }

    private void EndEvent()
    {
        EndGame();
        isPlaying = false;
    }
}
