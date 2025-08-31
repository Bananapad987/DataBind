using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class VirusBlock : MonoBehaviour
{
    SpriteRenderer sprite_renderer;
    public GameObject virus_field_prefab;
    public GameObject virus_projectile_prefab;

    public UnityEvent death;


    //red
    public float field_duration = 5f;

    //yellow
    public float projectile_speed = 3f;
    public GameObject player_object;

    //blue
    public float slow_percentage = 0.5f;
    public float slow_duration = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Color c = sprite_renderer.color;

        if (collision.gameObject.TryGetComponent<Player>(out var p))
        {
            if (c == Color.blue)
            {
                p.Slow(slow_percentage, slow_duration);
            }
            else
            {
                p.TakeDamage();
            }
        }
        else
        {
            if (c == Color.red)
            {
                GameObject vf_object = Instantiate(virus_field_prefab.gameObject);
                vf_object.transform.position = transform.position;
                vf_object.GetComponent<VirusField>().duration = field_duration;
            }
            else if (c == Color.yellow)
            {
                GameObject vp_object = Instantiate(virus_projectile_prefab);
                vp_object.transform.position = transform.position;
                vp_object.GetComponent<Rigidbody2D>().linearVelocity = (player_object.transform.position - transform.position).normalized * projectile_speed;
            }
        }

        death.Invoke();
        Destroy(gameObject);
    }
}
