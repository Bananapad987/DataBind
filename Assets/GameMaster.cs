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
        player.Start();

        cam.transform.parent = player.transform;
        sound_manager.transform.parent = player.transform;
        attacker = Instantiate(attacker_prefab).GetComponent<Attacker>();

        player.gui.quit.onClick.AddListener(EndGame);
        player.gui.pause.onClick.AddListener(PauseGame);
        player.gui.unpause.onClick.AddListener(UnpauseGame);

        
    }

    void PauseGame()
    {
        sound_manager.PlaySFX(SoundManager.SFX.pause_button, Vector3.zero);
        Time.timeScale = 0;
    }

    void UnpauseGame()
    {
        Time.timeScale = 1;
    }
    

    void EndGame()
    {
        Time.timeScale = 1;
        sound_manager.transform.parent = transform;
        cam.transform.parent = transform;
        Destroy(player.gameObject);
        Destroy(attacker.gameObject);

        title.gameObject.SetActive(true);
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (player != null)
    //     {
    //         sound_manager.transform.position = player.transform.position;
    //     }
    // }
}
