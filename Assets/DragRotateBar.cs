using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragRotateBar : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public float maxAngle = 18f;       // 좌우 최대 회전각
    public float sensitivity = 0.5f;   // 드래그 민감도
    public float returnSpeed = 5f;     // 중앙 복귀 속도

    private float currentAngle = 0f;
    private bool isDragging = false;
    private float dragStartX;

    [Header("게이지 설정")]
    public float gaugeIncreaseRate = 0.4f;   // 회전할 때 게이지 상승 속도
    public float gaugeDecreaseRate = 0.2f;   // 멈춰 있을 때 게이지 감소 속도
    public Slider gaugeSlider;               // 게이지용 슬라이더 (UI)
    public Image gaugeFill;

    private float previousAngle = 0f;
    private float gauge = 0f;

    void Update()
    {
        // 드래그 중이 아닐 때 중앙으로 복귀
        if (!isDragging)
        {
            currentAngle = Mathf.Lerp(currentAngle, 0f, Time.deltaTime * returnSpeed);
            transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
        }
        float rotationDelta = Mathf.Abs(currentAngle - previousAngle);
        previousAngle = currentAngle;

        // 드래그 중이 아닐 때 막대 중앙 복귀
        
        // 게이지 업데이트
        if (rotationDelta > 0.05f) // 움직임이 있을 때
        {
            gauge += gaugeIncreaseRate * Time.deltaTime;
        }
        else // 가만히 있을 때
        {
            gauge -= gaugeDecreaseRate * Time.deltaTime;
        }
        gauge = Mathf.Clamp01(gauge);
        if (gaugeSlider) gaugeSlider.value = gauge;
        if (gaugeFill) gaugeFill.fillAmount = gauge;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        dragStartX = eventData.position.x;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaX = eventData.position.x - dragStartX;
        float targetAngle = Mathf.Clamp(deltaX * sensitivity, -maxAngle, maxAngle);

        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * 10f);
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}
