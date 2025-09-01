using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Rendering;
using TMPro;
using System;

public class GameUI : MonoBehaviour
{

    //TEMPORARY VARIABLES FOR SPRITES
    public Sprite full_heart;
    public Sprite empty_heart;
    public GameObject heart_grid;

    int max_health = 0;
    int curr_health = 0;

    public Player player;
    public Attacker attacker;
    public TMP_Text text_box;

    // Heart template
    List<GameObject> hearts = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetHealth(player.max_health, player.curr_health);
    }

    public void SetHealth(int max_health_, int curr_health_)
    {
        if (max_health > max_health_)
        {
            DecreaseMaxHealth(max_health - max_health_);
        }
        else if (max_health < max_health_)
        {
            IncreaseMaxHealth(max_health_ - max_health);
        }

        curr_health = curr_health_;
        ReloadHeartUI();
    }

    /// <summary>
    /// Changes the current health by the amount specified (positive is healing, negative is damage)
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeHealth(int amount)
    {
        curr_health += amount;

        // Set curr to be within 0 and max
        curr_health = math.min(max_health, curr_health);
        curr_health = math.max(0, curr_health);

        ReloadHeartUI();
    }

    /// <summary>
    /// Removes a heart (removes empty ones first)
    /// </summary>
    /// <param name="amount"></param>
    public void DecreaseMaxHealth(int amount = 1)
    {
        // Lock max at 0
        if (max_health > amount)
        {
            max_health -= amount;
        }
        else
        {
            amount = max_health;
            max_health = 0;
        }

        // Decrease curr if we have to
        if (curr_health > max_health)
        {
            curr_health = max_health;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject h = hearts[hearts.Count - 1];
            hearts.RemoveAt(hearts.Count - 1);
            Destroy(h);
        }
    }

    /// <summary>
    /// Adds to max health. Also increases current health by "amount"
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseMaxHealth(int amount = 1)
    {
        max_health += amount;
        curr_health += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject heart_obj = new GameObject("Heart");
            Image img = heart_obj.AddComponent<Image>();
            img.sprite = full_heart;
            heart_obj.transform.SetParent(heart_grid.transform);
            hearts.Add(heart_obj);
        }
        ReloadHeartUI();
    }

    /// <summary>
    /// Sets the sprites of the hearts to the correct image (Has room for optimization)
    /// </summary>
    public void ReloadHeartUI()
    {
        for (int i = 0; i < max_health; i++)
        {
            Image img = hearts[i].GetComponent<Image>();
            if (i >= curr_health)
            {
                img.sprite = empty_heart;
            }
            else
            {
                img.sprite = full_heart;
            }
        }
    }

    void Update()
    {
        text_box.text = $"{Mathf.Floor(attacker.stopwatch)}";
    }
}
