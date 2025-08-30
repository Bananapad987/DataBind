using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 400.0F;
    Vector2 direction = Vector2.zero;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    void FixedUpdate()
    {
        Vector2 move_value = InputSystem.actions.FindAction("Move").ReadValue<Vector2>().normalized;
        rb.linearVelocity = move_value * speed * Time.deltaTime;
    }
}
