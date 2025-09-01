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


    public GameObject blue_screen_prefab;
    public static BlueScreen blue_screen;
    public static Camera cam;


    public int highscore = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sound_manager = Instantiate(sound_manager_prefab).GetComponent<SoundManager>();
        title = Instantiate(title_prefab).GetComponent<Title>();

        blue_screen = Instantiate(blue_screen_prefab).GetComponent<BlueScreen>();
        blue_screen.restart.onClick.AddListener(StartGame);
        
        blue_screen.quit.onClick.AddListener(BackToTitle);
        blue_screen.gameObject.SetActive(false);

        title.start.onClick.AddListener(StartGame);
        cam = GetComponentInChildren<Camera>();

        grid_obj = Instantiate(grid_prefab);
    }

    void StartGame()
    {
        blue_screen.gameObject.SetActive(false);

        title.gameObject.SetActive(false);
        player = Instantiate(player_prefab).GetComponent<Player>();
        player.Start();
        player.death.AddListener(EndGame);

        cam.transform.parent = player.transform;
        cam.transform.position = Vector2.zero;
        sound_manager.transform.parent = player.transform;
        sound_manager.transform.position = Vector2.zero;


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


    public void EndGame()
    {
        int curr_score = (int)Mathf.Floor(attacker.stopwatch);

        Time.timeScale = 1;
        sound_manager.transform.parent = transform;
        sound_manager.transform.position = Vector2.zero;
        cam.transform.parent = transform;
        cam.transform.position = Vector2.zero;
        Destroy(player.gameObject);
        Destroy(attacker.gameObject);

        blue_screen.gameObject.SetActive(true);
        blue_screen.text_box.text = $"Score: {curr_score}";
        if (curr_score > highscore)
        {
            highscore = curr_score;
        }
    }

    void BackToTitle()
    {
        blue_screen.gameObject.SetActive(false);

        if (highscore != 0)
        {
            title.highscore.text = $"Highscore: {highscore}";
        }
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
