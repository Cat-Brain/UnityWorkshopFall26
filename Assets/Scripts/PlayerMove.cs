using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;

    public float speed, jumpForce;
    public float accel, decel;

    public float jumpDist, jumpRadius;

    public float cayoteTime, jumpBuffer, jumpCooldown;
    float cayoteTimer = 0, jumpBufferTimer = 0, jumpCooldownTimer = 0;

    public InputActionReference movementAction, jumpAction;

    public LayerMask jumpMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpAction.action.started += TryJump;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity,
            oldVelocity = velocity;

        float movementInput = movementAction.action.ReadValue<Vector2>().x;
        

        if (Mathf.Abs(movementInput) > 0.1f)
            velocity.x += accel * movementInput * Time.deltaTime;
        else
            velocity.x -= decel * Mathf.Sign(velocity.x) * Time.deltaTime;

        float effectiveSpeed = Mathf.Max(speed, Mathf.Abs(oldVelocity.x));
        if (Mathf.Abs(velocity.x) > effectiveSpeed)
            velocity.x = Mathf.Sign(velocity.x) * effectiveSpeed;



        if (CanJump())
            cayoteTimer = cayoteTime;

        if (cayoteTimer > 0 && jumpBufferTimer > 0 && jumpCooldownTimer <= 0)
        {
            velocity.y = jumpForce;
            jumpBufferTimer = 0;
            cayoteTimer = 0;
            jumpCooldownTimer = jumpCooldown;
        }
        cayoteTimer -= Time.deltaTime;
        jumpBufferTimer -= Time.deltaTime;
        jumpCooldownTimer -= Time.deltaTime;


        rb.linearVelocity = velocity;
    }

    void TryJump(InputAction.CallbackContext _)
    {
        jumpBufferTimer = jumpBuffer;
    }

    bool CanJump()
    {
        return Physics2D.CircleCast(transform.position, jumpRadius, Vector2.down, jumpDist, jumpMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = CanJump() ? Color.green : cayoteTimer > 0 ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(transform.position, jumpRadius);
        Gizmos.DrawWireSphere(transform.position - Vector3.up * jumpDist, jumpRadius);
    }
}
