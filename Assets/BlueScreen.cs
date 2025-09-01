using TMPro;
using UnityEngine;

public class BlueScreen : MonoBehaviour
{
    public TMP_Text text_box;
    public UnityEngine.UI.Button restart;
    public UnityEngine.UI.Button quit;

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
}
