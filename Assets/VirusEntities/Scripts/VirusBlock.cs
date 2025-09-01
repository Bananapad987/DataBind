using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class VirusBlock : MonoBehaviour
{
    public GameObject virus_field_prefab;
    public GameObject virus_projectile_prefab;
    public UnityEvent death = new();

    public VirusBinder.TYPE type = VirusBinder.TYPE.white;
    //red
    readonly float field_duration = 5f;

    //yellow
    float projectile_speed = 7;

    //blue
    readonly float slow_percentage = 0.5f;
    readonly float slow_duration = 3;

    bool outside_wall = true;

    void Update()
    {
        if (type == VirusBinder.TYPE.black) {
            gameObject.GetComponent<Collider2D>().excludeLayers |= 1 << 6;
        }

        if (outside_wall && Mathf.Abs(transform.position.x) < 13 && Mathf.Abs(transform.position.y) < 8)
        {
            outside_wall = false;
        }

        if (outside_wall)
        {
            gameObject.GetComponent<Collider2D>().excludeLayers |= 1 << 7;
        }
        else
        {
            gameObject.GetComponent<Collider2D>().excludeLayers &= ~(1 << 7);
        }
    }

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
                    vp_object.GetComponent<Rigidbody2D>().linearVelocity = (GameMaster.player.gameObject.transform.position - transform.position).normalized * projectile_speed;
                    break;
            }

            GameMaster.sound_manager.PlaySFX(SoundManager.SFX.block_break, transform.position);
        }

        death.Invoke();
        Destroy(gameObject);
    }
}
