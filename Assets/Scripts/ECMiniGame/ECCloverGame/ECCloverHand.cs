using Unity.Burst.Intrinsics;
using UnityEngine;

public class ECCloverHand : MonoBehaviour
{
    public CanvasGroup cg;
    private RectTransform rt;
    public ECMiniGameTimer timer;

    public RectTransform handGuideRect;
    public RectTransform handImage;
    public RectTransform bodyImage;
    private bool isStart;
    public float smoothSpeed = 20f;
    private Camera mainCamera;
    public RectTransform arm;
    public Canvas canvas;


    private void Awake()
    {
        mainCamera = Camera.main;
         rt = GetComponent<RectTransform>();
        isStart = true;
    }
    private void Update()
    {
        if(isStart)
        {
            Vector2 pos = Input.mousePosition;
            RotateArm();
            handGuideRect.position = Vector2.Lerp(handGuideRect.position, pos, Time.deltaTime * smoothSpeed);
            if (!timer.IsStart)
            {
                isStart = false;
                cg.alpha = 0;
            }
        }
    }

    private void RotateArm()
    {
        if (arm == null || mainCamera == null)
            return;
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out mousePos
        );

        Vector2 shoulderPos = arm.anchoredPosition;

        // 3️⃣ 방향 계산
        Vector2 dir = mousePos - shoulderPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 120;

        // 4️⃣ 팔 회전 (부드럽게)
        Quaternion targetRot = Quaternion.Euler(0, 0, angle);
        arm.localRotation = Quaternion.Lerp(arm.localRotation, targetRot, Time.deltaTime * smoothSpeed);
    }
}
