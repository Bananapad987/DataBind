using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject sound_manager_prefab;
    public static SoundManager sound_manager;

    public GameObject attacker_prefab;
    public static Attacker attacker;

    public GameObject player_prefab;
    public static Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sound_manager = Instantiate(sound_manager_prefab).GetComponent<SoundManager>();
        StartGame();
    }

    void StartGame()
    {
        player = Instantiate(player_prefab).GetComponent<Player>();
        attacker = Instantiate(attacker_prefab).GetComponent<Attacker>();
    }

    void EndGame()
    {
        sound_manager.transform.position = Vector3.zero;
        Destroy(player.gameObject);
        Destroy(attacker.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            sound_manager.transform.position = player.transform.position;
        }
    }
}
