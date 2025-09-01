using UnityEngine;

public class VirusProjectile : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out var p))
        {
            p.TakeDamage();
        }

        Destroy(gameObject);
    }
}
