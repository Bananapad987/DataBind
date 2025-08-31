using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class VirusBlock : MonoBehaviour
{
    SpriteRenderer sprite_renderer;
    public GameObject virus_field_prefab;
    public GameObject virus_projectile_prefab;

    public UnityEvent<VirusBlock> death;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Color c = sprite_renderer.color;

        if (c == Color.red)
        {
            GameObject virus_field_obj = Instantiate(virus_field_prefab.gameObject);
            virus_field_obj.transform.position = transform.position;
        }
        else if (c == Color.yellow)
        {

        }
        else if (c == Color.blue)
        {

        }

        Debug.Log("Collided");

        death.Invoke(this);
        Destroy(gameObject);
    }
}
