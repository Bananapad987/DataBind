using UnityEngine;

public class Title : MonoBehaviour
{
    public UnityEngine.UI.Button start;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
