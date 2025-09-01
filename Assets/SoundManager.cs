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

    public float global_volume;
    public float music_volume;
    public float sfx_volume;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        music = gameObject.GetComponent<AudioSource>();
        music.volume = global_volume;
        PlaySFX(SFX.open_game, Vector3.zero);
    }

    public void SetSFX(float newValue)
    {
        sfx_volume = newValue;
    }
    public void SetMusic(float newValue)
    {
        music_volume = newValue;
        music.volume = global_volume * music_volume;

    }
    public void PlaySFX(SFX s, Vector3 pos)
    {
        float volume_adjustment = 1;
        AudioClip sfx = open_game_sfx;
        switch (s)
        {
            case SFX.menu_button:
                sfx = menu_button_sfx;
                volume_adjustment = 0.4F;
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
            pos = transform.position;
            AudioSource.PlayClipAtPoint(sfx, pos, global_volume * sfx_volume * volume_adjustment);
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
