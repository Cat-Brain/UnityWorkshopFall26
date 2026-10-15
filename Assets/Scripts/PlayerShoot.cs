using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;

    public float range;

    public InputActionReference shootAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootAction.action.started += TryShoot;
    }

    void TryShoot(InputAction.CallbackContext _)
    {
        Vector2 aim = AimDirection();
        Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)AimDirection() * range);
    }

    Vector2 AimDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint((Vector3)Mouse.current.position.value);
        Vector2 mouseOffset = mousePos - (Vector2)transform.position;
        return mouseOffset.normalized;
    }
}
