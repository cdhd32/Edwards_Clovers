using DG.Tweening;
using UnityEngine;

public class ECLiquid : MonoBehaviour
{
    public Rigidbody2D rigid;
    private CircleCollider2D circleCollider;
    public Transform imageTransform;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();

        //circleCollider.isTrigger = true;

    }

    private void OnEnable()
    {

        //Invoke("TriggerSet", 2f);
        //circleCollider.isTrigger = false;
    }

    public void AddForce()
    {

        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.gravityScale = 1.5f;
        rigid.AddForce(new Vector2(-2,-1) * 0.05f, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Utils.tagName))
        {
            TriggerSet();   
        }
    }

    private void TriggerSet()
    {
        imageTransform.DOScale(1, 1);
        circleCollider.isTrigger = false;
    }

    private void OnDisable()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
        circleCollider.isTrigger = true;
    }
}
