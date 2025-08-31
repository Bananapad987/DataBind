using UnityEngine;
using UnityEngine.Analytics;

[RequireComponent(typeof(LineRenderer))]
public class VirusBinder : MonoBehaviour
{
    LineRenderer line_renderer;

    public enum TYPE
    {
        white,
        blue,
        red,
        yellow,
    }

    VirusBlock target_1;
    VirusBlock target_2;

    public float initial_velocity = 10;
    public float raycast_length = 1000;

    public void Bind(VirusBlock t_1, VirusBlock t_2, TYPE t)
    {
        target_1 = t_1;
        target_2 = t_2;

        target_1.death.AddListener(Rebind);
        target_2.death.AddListener(Rebind);

        Color c = Color.white;

        switch (t)
        {
            case TYPE.red:
                c = Color.red;
                break;
            case TYPE.yellow:
                c = Color.yellow;
                break;
            case TYPE.blue:
                c = Color.blue;
                break;
        }

        target_1.GetComponent<SpriteRenderer>().color = c;
        target_2.GetComponent<SpriteRenderer>().color = c;

        line_renderer = gameObject.GetComponent<LineRenderer>();

        c.a = 0.2f;
        line_renderer.startColor = c;
        line_renderer.endColor = c;

        Vector3 p_1 = target_1.transform.position;
        Vector3 p_2 = target_2.transform.position;

        line_renderer.SetPosition(0, target_1.transform.position);
        line_renderer.SetPosition(1, target_2.transform.position);

        transform.position = (p_1 + p_2) / 2;

        target_1.GetComponent<Rigidbody2D>().linearVelocity = (transform.position - p_1).normalized * initial_velocity;
        target_2.GetComponent<Rigidbody2D>().linearVelocity = (transform.position - p_2).normalized * initial_velocity;

        target_1.GetComponent<Collider2D>().enabled = true;
        target_2.GetComponent<Collider2D>().enabled = true;

    }

    void Rebind()
    {
        if (target_1 == null)
        {
            target_1 = target_2;
        }
        else
        {
            target_2 = null;
        }

        if (target_1 == null && target_2 == null)
        {
            Destroy(gameObject);
        }

        RaycastHit2D hit = Physics2D.Raycast(target_1.transform.position, target_1.GetComponent<Rigidbody2D>().linearVelocity.normalized, raycast_length, 1 << 7);

        if (hit.collider != null)
        {
            transform.position = hit.point;
            line_renderer.SetPosition(0, target_1.transform.position);
            line_renderer.SetPosition(1, target_2.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target_1 == null || target_1.gameObject == null)
        {
            return;
        }

        Vector3 p_1 = target_1.transform.position;

        line_renderer.SetPosition(0, target_1.transform.position);

        if (target_2 != null && target_2.gameObject != null)
        {
            line_renderer.SetPosition(1, target_2.transform.position);   
        }

        if ((p_1 - transform.position).sqrMagnitude < 1)
        {
            Destroy(gameObject);
        }
    }
}
