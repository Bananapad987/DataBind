using System.Collections;
using UnityEngine;

public class VirusField : MonoBehaviour
{
    public float duration = -1;

    void Update()
    {
        if (duration > 0)
        {
            StartCoroutine(Activate(duration));
            duration = -1;
        }
    }
    
    public IEnumerator Activate(float d)
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(d);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out var p))
        {
            p.TakeDamage();
            Destroy(gameObject);
        }
    }
}
