using UnityEngine;

public class ECLiquid : MonoBehaviour
{
    public Rigidbody2D rigid;
    private CircleCollider2D circleCollider;
    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        circleCollider.isTrigger = true;

    }

    private void OnEnable()
    {
        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.AddForce(new Vector2(-1,-1) * 0.1f, ForceMode2D.Impulse);
        //Invoke("TriggerSet", 2f);
        //circleCollider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Box"))
        {
            TriggerSet();   
        }
    }

    private void TriggerSet()
    {
        circleCollider.isTrigger = false;
    }

    private void OnDisable()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
        circleCollider.isTrigger = true;
    }
}
