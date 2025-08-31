using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int speed = 10;

    int max_health = 5;
    int hearts;

    Vector2 direction = Vector2.zero;
    Rigidbody2D rb;

    public float dash_velocity = 60F;
    public float dash_duration = 0.07F;

    float max_dash_cooldown = 3;
    float dash_cooldown = 0;
    bool can_dash = true;
    Coroutine curr_dash = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hearts = max_health;
        Debug.Log("Start");

    }
    void FixedUpdate()
    {
        if (curr_dash == null)
        {
            direction = InputSystem.actions.FindAction("Move").ReadValue<Vector2>().normalized;
            rb.linearVelocity = direction * speed;
        }
    }

    void Update()
    {
        if (can_dash && InputSystem.actions.FindAction("Dash").WasPressedThisFrame())
        {
            Debug.Log("Dash");
            if (direction != Vector2.zero)
            {
                curr_dash = StartCoroutine("Dash");
            }
        }
        if (!can_dash && curr_dash == null)
        {
            if (dash_cooldown >= 0)
            {
                Debug.Log($"{dash_cooldown}, {curr_dash}");
                dash_cooldown -= Time.deltaTime;
            }
            else
            {
                can_dash = true;
            }
        }
    }

    IEnumerator Dash()
    {
        can_dash = false;
        rb.linearVelocity = direction * dash_velocity;
        yield return new WaitForSeconds(dash_duration);

        dash_cooldown = max_dash_cooldown;
        rb.linearVelocity = Vector2.zero;
        curr_dash = null;
    }

    void TakeDamage()
    {
        hearts--;
        if (hearts == 0)
        {
            Died();
        }
    }

    void Heal()
    {
        if (hearts != max_health)
        {
            hearts++;
        }
    }

    void Died()
    {
        //Do something here
        return;
    }
}
