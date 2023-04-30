using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To attach to things that make sound effects for the character :)
/// </summary>
public class CharacterSFXManager : MonoBehaviour
{
    private static CharacterSFXManager _sInstance;

    public static CharacterSFXManager Get()
    {
        return _sInstance;
    }

    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private List<AudioClip> m_stepsClipLists;
    [SerializeField] private List<AudioClip> m_jumpClipLists;
    [SerializeField] private List<AudioClip> m_slideClipLists;
    [SerializeField] private List<AudioClip> m_hurtClipLists;
    [SerializeField] private List<AudioClip> m_dieClipLists;
    [SerializeField] private AudioClip m_fallClip;

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

        if (m_audioSource)
        {
            m_audioSource.playOnAwake = false;
            m_audioSource.loop = false;
            m_audioSource.clip = null;
        }
        else
        {
            Debug.LogError("No audio source attached to: " + this.name);
        }
    }

    public void PlayStepSFX()
    {
        m_audioSource.PlayOneShot(m_stepsClipLists[Random.Range(0, m_stepsClipLists.Count)]);
    }

    public void PlayJumpSFX()
    {
        m_audioSource.PlayOneShot(m_jumpClipLists[Random.Range(0, m_jumpClipLists.Count)]);
    }

    public void PlaySlideSFX()
    {
        m_audioSource.PlayOneShot(m_slideClipLists[Random.Range(0, m_slideClipLists.Count)]);
    }

    public void PlayHurtSFX()
    {
        m_audioSource.PlayOneShot(m_hurtClipLists[Random.Range(0, m_hurtClipLists.Count)]);
    }

    public void PlayDieSFX()
    {
        m_audioSource.PlayOneShot(m_dieClipLists[Random.Range(0, m_dieClipLists.Count)]);
        m_audioSource.clip = m_fallClip;
        m_audioSource.PlayDelayed(0.15f);
    }
}
