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
        black
    }

    public VirusBlock target_1;
    public VirusBlock target_2;

    public Sprite[] sprites;
    public Color[] colors = {Color.white, Color.blue, Color.red, Color.yellow, Color.black};

    public float initial_velocity = 10;
    public float raycast_length = 1000;

    bool binded = false;

    public void Bind(VirusBlock t_1, VirusBlock t_2, TYPE t)
    {
        target_1 = t_1;
        target_2 = t_2;

        target_1.death.AddListener(Rebind);
        target_2.death.AddListener(Rebind);

        if (target_1 == null || target_2 == null)
        {
            Rebind();
            return;
        }

        target_1.GetComponent<VirusBlock>().type = t;
        target_2.GetComponent<VirusBlock>().type = t;

        target_1.GetComponent<SpriteRenderer>().sprite = sprites[(int)t];
        target_2.GetComponent<SpriteRenderer>().sprite = sprites[(int)t];

        line_renderer = gameObject.GetComponent<LineRenderer>();

        Color c = colors[(int)t];
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

        binded = true;

    }

    void Rebind()
    {
        if (target_1 == null)
        {
            if (target_2 == null)
            {
                Destroy(gameObject);
            }
            else
            {
                target_1 = target_2;
            }
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
        if (!binded)
        {
            return;
        }

        if (target_1 == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 p_1 = target_1.transform.position;

        line_renderer.SetPosition(0, target_1.transform.position);

        if (target_2 != null)
        {
            line_renderer.SetPosition(1, target_2.transform.position);   
        }

        if ((p_1 - transform.position).sqrMagnitude < 1)
        {
            Destroy(gameObject);
        }
    }
}
