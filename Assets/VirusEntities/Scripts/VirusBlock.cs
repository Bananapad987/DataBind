using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class VirusBlock : MonoBehaviour
{
    public GameObject virus_field_prefab;
    public GameObject virus_projectile_prefab;

    public UnityEvent death;

    public VirusBinder.TYPE type;
    //red
    public float field_duration = 5f;

    //yellow
    public float projectile_speed = 3f;
    public GameObject player_object;

    //blue
    public float slow_percentage = 0.5f;
    public float slow_duration = 3;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out var p))
        {
            if (type == VirusBinder.TYPE.blue)
            {
                p.Slow(slow_percentage, slow_duration);
            }

            p.TakeDamage();
        }
        else
        {
            switch (type)
            {
                case VirusBinder.TYPE.red:
                    GameObject vf_object = Instantiate(virus_field_prefab.gameObject);
                    vf_object.transform.position = transform.position;
                    vf_object.GetComponent<VirusField>().duration = field_duration;
                    break;
                case VirusBinder.TYPE.yellow:
                    GameObject vp_object = Instantiate(virus_projectile_prefab);
                    vp_object.transform.position = transform.position;
                    vp_object.GetComponent<Rigidbody2D>().linearVelocity = (player_object.transform.position - transform.position).normalized * projectile_speed;
                    break;
                case VirusBinder.TYPE.black:
                    int layermask = ~(1 << 7 | 1 << 8);
                    gameObject.GetComponent<Collider2D>().excludeLayers = layermask;
                    return;
            }
        }

        death.Invoke();
        Destroy(gameObject);
    }
}
