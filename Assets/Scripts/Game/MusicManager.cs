using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private AudioSource music;
    public static MusicManager Instance;
    public AudioClip menu, game;

    // Use this for initialization
    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        music = GetComponent<AudioSource>();
        music.loop = true;
        PlayMenuMusic();
    }

    public void PlayMenuMusic()
    {
        if (music.clip != menu)
        {
            music.Pause();
            music.clip = menu;
            music.Play();
        }
    }

    public void PlayGameMusic()
    {
        if (music.clip != game)
        {
            music.Pause();
            music.clip = game;
            music.Play();
        }
    }
}