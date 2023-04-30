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

    [SerializeField] private AudioSource m_audioSource1;
    [SerializeField] private AudioSource m_audioSource2;
    [SerializeField] private AudioClip m_mainMenuMusicClip;
    [SerializeField] private AudioClip m_transitionClip;
    [SerializeField] private AudioClip m_levelMusicClip;

    private void Awake()
    {
        if (!_sInstance)
        {
            _sInstance = this;
        }
        else
        {
            DestroyImmediate(this);
            return;
        }

        if (m_audioSource1 && m_audioSource2)
        {
            m_audioSource1.playOnAwake = false;
            m_audioSource1.loop = true;

            //// For main menu first, then level
            //m_audioSource1.playOnAwake = false;
            //m_audioSource1.loop = true;

            //// For transition
            //m_audioSource2.playOnAwake = false;
            //m_audioSource2.loop = false;

            //PlayMainMenuMusic();
        }
        else
        {
            Debug.LogError("No audio source attached to: " + this.name);
        }
    }

    private void PlayMainMenuMusic()
    {
        if (m_mainMenuMusicClip)
        {
            // Set up AS1 to play main menu clip
            if (m_audioSource1.isPlaying)
            {
                m_audioSource1.Stop();
            }
            m_audioSource1.clip = m_mainMenuMusicClip;
            m_audioSource1.Play();

            // Set up AS2 to load up transition clip
            m_audioSource2.clip = m_transitionClip;
        }
        else
        {
            Debug.LogError("Missing main menu clip ref");
        }
    }

    public void TransitionToLevelMusic()
    {
        if (m_levelMusicClip && m_transitionClip)
        {
            // Play transition music
            m_audioSource2.Play();
            m_audioSource1.Stop();

            StartCoroutine(WaitForTransitionEnd());

            // Set up for next level music with AS1
            m_audioSource1.clip = m_levelMusicClip;
            m_audioSource1.loop = true;
        }
        else
        {
            Debug.LogError("Missing level clip ref");
        }
    }

    private IEnumerator WaitForTransitionEnd()
    {
        // while the transition clip is still playing
        while (m_audioSource2.isPlaying)
        {
            yield return null;
        }
        // Transition clip is now done, stop AS2
        // Play level loop on AS1
        m_audioSource1.Play();
        m_audioSource2.Stop();
    }
}
