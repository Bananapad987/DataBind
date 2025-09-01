using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public UnityEngine.UI.Button start;

    public Slider sfx_slider;
    public Slider music_slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sfx_slider.onValueChanged.AddListener(GameMaster.sound_manager.SetSFX);
        music_slider.onValueChanged.AddListener(GameMaster.sound_manager.SetMusic);
    }

    // Update is called once per frame
    void Update()
    {
        sfx_slider.value = GameMaster.sound_manager.sfx_volume;
        music_slider.value = GameMaster.sound_manager.music_volume;
    }

    public void SoundPlay()
    {
        GameMaster.sound_manager.PlaySFX(SoundManager.SFX.menu_button, Vector3.zero);
    }

    public void Quit() 
    {
        Debug.Log("APP QUIT");
        Application.Quit();
    }
}
