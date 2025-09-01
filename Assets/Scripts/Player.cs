using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 10;

    int max_health = 5;
    int hearts;

    public Vector2 direction = Vector2.zero;
    Rigidbody2D rb;

    public float dash_velocity = 60F;
    public float dash_duration = 0.07F;

    float max_dash_cooldown = 3;
    float dash_cooldown = 0;
    bool can_dash = true;
    Coroutine curr_dash = null;

    Coroutine has_iframes = null;
    float iframe_duration = 1;

    Coroutine is_slowed = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hearts = max_health;
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
                if (has_iframes != null)
                {
                    StopCoroutine(has_iframes);
                }

                has_iframes = StartCoroutine(Iframes(dash_duration));
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

    public void Slow(float slow_percentage, float slow_duration) {
        if (is_slowed != null)
        {
            StopCoroutine(is_slowed);
        }
        else
        {
            is_slowed = StartCoroutine(Slowed(slow_percentage, slow_duration));
        }
    }

    public IEnumerator Slowed(float slow_percentage, float slow_duration)
    {
        float amt = speed * slow_percentage;
        speed -= amt;
        yield return new WaitForSeconds(slow_duration);
        speed += amt;

        is_slowed = null;
    }

    IEnumerator Iframes(float duration)
    {
        yield return new WaitForSeconds(duration);
        has_iframes = null;
    }

    public void TakeDamage()
    {
        if (has_iframes != null)
        {
            return;
        }

        hearts--;
        GameMaster.sound_manager.PlaySFX(SoundManager.SFX.hit, Vector3.zero);

        if (hearts == 0)
        {
            Died();
        }

        has_iframes = StartCoroutine(Iframes(iframe_duration));
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
