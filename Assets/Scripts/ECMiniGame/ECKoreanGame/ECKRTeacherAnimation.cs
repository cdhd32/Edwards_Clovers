using DG.Tweening;
using UnityEngine;

public class ECKRTeacherAnimation : MonoBehaviour
{
    public RectTransform answerGoal;
    public RectTransform originTrans;
    public RectTransform edwardHand;
    public RectTransform teacherEffect;
    private Vector3 originPos;
    private Vector3 originRot;
    private float edwardHandOriginPos = -910;
    private float animDuration = 0.5f;

    private void Awake()
    {
        originPos = originTrans.position;
        originRot = originTrans.rotation.eulerAngles;
        //MoveStick(true);
        teacherEffect.transform
            .DORotate(new Vector3(0, 0, -24), 1, RotateMode.Fast)
            .SetEase(Ease.Linear)           
            .SetLoops(-1, LoopType.Yoyo);
    }
    public void MoveStick(bool isClickAnswer)
    {
        if(isClickAnswer)
        {
            originTrans.DORotate(answerGoal.rotation.eulerAngles, animDuration);
            originTrans.DOMove(answerGoal.transform.position, animDuration);
            edwardHand.DOLocalMoveY(-2, animDuration);
        }
        else
        {
            originTrans.DORotate(originRot, animDuration);
            originTrans.DOMove(originPos, animDuration);
            edwardHand.DOLocalMoveY(edwardHandOriginPos, animDuration);
        }
    }
}
