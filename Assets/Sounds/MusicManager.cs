using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will hold music sound clips and play them, *not* for sound effects
/// </summary>
public class MusicManager : MonoBehaviour
{
    private static MusicManager _sInstance;

    public static MusicManager Get()
    {
        return _sInstance;
    }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip MainMenuMusicClip;
    [SerializeField] private AudioClip LevelMusicClip;

    private void Awake()
    {
        if (!_sInstance)
        {
            _sInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(this);
            return;
        }

        if(!MainMenuMusicClip || !LevelMusicClip)
        {
            //Debug.LogError("Missing an audio clips ref");
        }

        if (audioSource)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = true;
        }
        else
        {
            Debug.LogError("No audio source attached to: " + this.name);
        }

    }

    public void PlayMainMenuMusic()
    {

    }

    public void PlayLevelMusic()
    {

    }
}
