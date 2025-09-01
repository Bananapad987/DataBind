using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject sound_manager_prefab;
    public static SoundManager sound_manager;

    public GameObject attacker_prefab;
    public static Attacker attacker;

    public GameObject title_prefab;
    public static Title title;

    public GameObject player_prefab;
    public static Player player;

    public GameObject grid_prefab;
    public static GameObject grid_obj;
    public static Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sound_manager = Instantiate(sound_manager_prefab).GetComponent<SoundManager>();
        title = Instantiate(title_prefab).GetComponent<Title>();
        title.start.onClick.AddListener(StartGame);
        cam = GetComponentInChildren<Camera>();
    }

    void StartGame()
    {
        grid_obj = Instantiate(grid_prefab);
        title.gameObject.SetActive(false);
        player = Instantiate(player_prefab).GetComponent<Player>();
        cam.transform.parent = player.transform;
        attacker = Instantiate(attacker_prefab).GetComponent<Attacker>();
    }

    void EndGame()
    {
        sound_manager.transform.position = Vector3.zero;
        cam.transform.parent = transform;
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
