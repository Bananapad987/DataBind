using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    AudioSource music;

    public enum SFX {
        open_game,
        menu_button,
        pause_button,
        block_break,
        hit,
    }

    public AudioClip open_game_sfx;
    public AudioClip menu_button_sfx;
    public AudioClip pause_sfx;
    public AudioClip block_break_sfx;
    public AudioClip hit_sfx;

    public float global_volume = 0.5f;
    public float music_volume = 1;
    public float sfx_volume = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        music = gameObject.GetComponent<AudioSource>();
        music.volume = global_volume * music_volume;
        PlaySFX(SFX.open_game, Vector3.zero);
    }

    public void PlaySFX(SFX s, Vector3 pos)
    {
        AudioClip sfx = open_game_sfx;
        switch (s)
        {
            case SFX.menu_button:
                sfx = menu_button_sfx;
                break;
            case SFX.pause_button:
                sfx = pause_sfx;
                break;
            case SFX.block_break:
                sfx = block_break_sfx;
                break;
            case SFX.hit:
                sfx = hit_sfx;
                break;
        }

        if (sfx == block_break_sfx)
        {
            StartCoroutine(BlockBreak(pos));
        }
        else
        {
            AudioSource.PlayClipAtPoint(sfx, pos, global_volume * sfx_volume);   
        }
    }

    IEnumerator BlockBreak(Vector3 pos)
    {
        GameObject temp = new();
        temp.transform.position = pos;
        AudioSource a = temp.AddComponent<AudioSource>();
        a.volume = global_volume * sfx_volume;
        a.resource = block_break_sfx;
        a.pitch = Random.Range(-.2f, .2f) + 1;
        a.Play();
        yield return new WaitForSeconds(block_break_sfx.length);
        Destroy(temp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
