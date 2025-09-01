using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public GameUI gui;
    public List<Sprite> sprites;

    public float speed = 10;
    public int max_health = 5;
    public int curr_health = 5;

    public Vector2 direction = Vector2.zero;
    Rigidbody2D rb;

    public float dash_velocity = 60F;
    public float dash_duration = 0.07F;

    public float max_dash_cooldown = 3;
    public float dash_cooldown = 0;
    public bool can_dash = true;
    Coroutine curr_dash = null;
    Coroutine has_iframes = null;
    float iframe_duration = 1;
    Coroutine is_slowed = null;

    DashBar dash_bar;

    public UnityEvent death = new UnityEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // gui.ReloadHeartUI();
        dash_bar = GetComponentInChildren<DashBar>();

        gui = GetComponentInChildren<GameUI>();
    }

    void FixedUpdate()
    {
        if (curr_dash == null)
        {
            direction = InputSystem.actions.FindAction("Move").ReadValue<Vector2>().normalized;
            rb.linearVelocity = direction * speed;
        }
    }

    void DashHandler()
    {
        if (can_dash && InputSystem.actions.FindAction("Dash").WasPressedThisFrame())
            {
            Debug.Log("Dash");
            if (direction != Vector2.zero) // Use dash
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
                dash_bar.frac = dash_cooldown / max_dash_cooldown;
                dash_cooldown -= Time.deltaTime;
            }
            else
            {
                dash_bar.SetVisible(false);
                can_dash = true;
            }
        }
    }

    void SpriteHandler()
    {
        int value = 0;

        if (is_slowed != null)
        {
            value += 1;
        }

        if (!can_dash)
        {
            value += 2;
        }

        GetComponent<SpriteRenderer>().sprite = sprites[value];
    }

    void Update()
    {
        DashHandler();
        SpriteHandler();
    }

    IEnumerator Dash()
    {
        can_dash = false;
        rb.linearVelocity = direction * dash_velocity;
        yield return new WaitForSeconds(dash_duration);

        dash_cooldown = max_dash_cooldown;
        dash_bar.frac = dash_cooldown / max_dash_cooldown;
        dash_bar.SetVisible(true);

        rb.linearVelocity = Vector2.zero;
        curr_dash = null;
    }

    public void Slow(float slow_percentage, float slow_duration)
    {
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

    public IEnumerator HitFlash()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.material.SetFloat("_IsFlash", 1);
        yield return new WaitForSeconds(.25F);
        sr.material.SetFloat("_IsFlash", 0);

        float duration_left = iframe_duration;
        float flash_interval = .1F;
        while (duration_left >= 2 * flash_interval)
        {
            Debug.Log("HITFLASH");

            yield return new WaitForSeconds(flash_interval);
            sr.enabled = false;

            yield return new WaitForSeconds(flash_interval);
            sr.enabled = true;

            duration_left -= 2*flash_interval;
        }

    }

    public void TakeDamage()
    {
        if (has_iframes != null)
        {
            return;
        }

        StartCoroutine(HitFlash());
        gui.ChangeHealth(-1);
        curr_health--;
        GameMaster.sound_manager.PlaySFX(SoundManager.SFX.hit, transform.position);

        if (curr_health == 0)
        {
            death.Invoke();
        }

        has_iframes = StartCoroutine(Iframes(iframe_duration));
    }

    void Heal()
    {
        if (curr_health != max_health)
        {
            curr_health++;
        }
    }

    void Died()
    {
        //Do something here
        return;
    }
    

}
