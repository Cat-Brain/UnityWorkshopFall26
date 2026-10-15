using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Rigidbody2D rb;

    public float speed;

    Vector2 dir;

    void Start()
    {
        dir = transform.right;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = dir * speed;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!enabled)
            return;

        Destroy(gameObject);
        enabled = false;
    }
}
