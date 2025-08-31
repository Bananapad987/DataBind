using System.Collections;
using System.Linq;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public GameObject virus_binder_prefab;
    public GameObject virus_block_prefab;
    public GameObject player_object;

    public float attack_cooldown = 3;
    public float attack_timer = 0;

    /// <summary>
    /// time between the first block spawning and the second block spawning
    /// </summary>
    public float burst_time = 0.5f;

    /// <summary>
    /// time between the second block spawning and squares actually binding
    /// </summary>
    public float bind_time = 0.5f;

    /// <summary>
    /// Amount of virus blocks summoned each attack
    /// </summary>
    public float bursts = 1;


    /// <summary>
    /// Amount of time between summoning virus blocks
    /// </summary>
    public float burst_spacing = 0.2f;

    /// <summary>
    /// Everytime the timer has increase by this amount, the brust will increase
    /// </summary>
    public float burst_increase_interval = 30;

    /// <summary>
    /// If the bursts have increased by this amount, then the attack_cooldown will decrease
    /// </summary>
    public float attack_cooldown_decrease_interval = 2;
    public float attack_cooldown_decrease_amount = 0.5f;

    /// <summary>
    /// Blocks will spawn between the inner and outer radius centered at the player
    /// </summary>
    public float inner_radius = 10;
    public float outer_radius = 30;

    /// <summary>
    /// Keeps track of when to increase the difficulty
    /// </summary>
    public float difficulty_timer = 0;

    /// <summary>
    /// Amount of times the burst increased since the attack_cooldown has decreased
    /// </summary>
    public float burst_increase_count = 0;

    /// <summary>
    /// Score
    /// </summary>
    public float stopwatch = 0;

    public int[] weights = { 3, 0, 0, 0 };

    int level = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        difficulty_timer = burst_increase_interval;
        attack_timer = attack_cooldown;
    }

    void UpdateDifficulty()
    {
        if (difficulty_timer <= 0)
        {
            level += 1;
            if (burst_increase_count == attack_cooldown_decrease_interval)
            {
                attack_cooldown = Mathf.Max(attack_cooldown - attack_cooldown_decrease_amount, 1);
                burst_increase_count = 0;
            }
            else
            {
                bursts += 1;
                burst_increase_count += 1;
            }

            difficulty_timer = burst_increase_interval;
            weights[(level % 4) + 1] += 1;
        }
    }

    VirusBinder.TYPE GenerateType()
    {
        int total = weights.Sum(); 

        int temp = Random.Range(0, total) + 1;

        for (int i = 0; i < weights.Length; i++)
        {
            temp -= weights[i];

            if (temp <= 0)
            {
                return (VirusBinder.TYPE)i;
            }
        }

        return VirusBinder.TYPE.white;
    }

    IEnumerator SummonBlocks()
    {
        Vector2 direction = new Vector2(Random.Range(-1f, 1), Random.Range(-1f, 1)).normalized;
        float distance_1 = Random.Range(inner_radius, outer_radius);
        float distance_2 = Random.Range(inner_radius, outer_radius);

        GameObject virus_block_object_1 = Instantiate(virus_block_prefab);
        virus_block_object_1.transform.position = (Vector2)player_object.transform.position + direction * distance_1;

        yield return new WaitForSeconds(burst_time);

        GameObject virus_block_object_2 = Instantiate(virus_block_prefab);
        virus_block_object_2.transform.position = (Vector2)player_object.transform.position - direction * distance_2;

        VirusBinder vb = Instantiate(virus_binder_prefab).GetComponent<VirusBinder>();

        VirusBlock virus_block_1 = virus_block_object_1.GetComponent<VirusBlock>();
        VirusBlock virus_block_2 = virus_block_object_2.GetComponent<VirusBlock>();

        virus_block_1.player_object = player_object;
        virus_block_2.player_object = player_object;

        VirusBinder.TYPE type = GenerateType();

        yield return new WaitForSeconds(bind_time);

        vb.Bind(virus_block_1, virus_block_2, type);
    }

    IEnumerator Attack()
    {
        for (int i = 0; i < bursts; i++)
        {
            StartCoroutine(SummonBlocks());
            yield return new WaitForSeconds(burst_spacing);
        }

        attacking = null;
        attack_timer = attack_cooldown;
    }



    Coroutine attacking = null;

    // Update is called once per frame
    void Update()
    {
        stopwatch += Time.deltaTime;
        difficulty_timer -= Time.deltaTime;
        attack_timer -= Time.deltaTime;

        UpdateDifficulty();

        if (attack_timer <= 0 && attacking == null)
        {
            attacking = StartCoroutine(Attack());
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(player_object.transform.position, inner_radius);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(player_object.transform.position, outer_radius);
    }
}
