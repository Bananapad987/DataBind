using UnityEngine;

public class DashBar : MonoBehaviour
{

    GameObject dash_mask;

    public float frac = 1;

    bool is_visible;

    SpriteRenderer bar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dash_mask = GetComponentInChildren<SpriteMask>().gameObject;
        bar = GetComponent<SpriteRenderer>();
        SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (is_visible)
        {
            UpdateBar();
        }
    }

    void UpdateBar()
    {
        Vector2 pos = dash_mask.transform.localPosition;
        pos.x = -0.28F * Mathf.Round(frac * 9) / 9;
        dash_mask.transform.localPosition = pos;  
    }

    public void SetVisible(bool b)
    {
        bar.enabled = b;
        is_visible = b;

        if (!is_visible)
        {
            frac = 1;
            UpdateBar();
        }
    }
}
