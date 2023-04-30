using System.Collections.Generic;
using UnityEngine;


public class EruptionSFXManager : MonoBehaviour
{
    private static EruptionSFXManager _sInstance;
    public static EruptionSFXManager Get()
    {
        return _sInstance;
    }

    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private List<AudioClip> m_whooshClips;
    [SerializeField] private List<AudioClip> m_crashClips;

    void Awake()
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
        m_audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX()
    {
        AudioClip clip = m_whooshClips[Random.Range(0, m_whooshClips.Count)];
        m_audioSource.PlayOneShot(clip);
        m_audioSource.clip = m_crashClips[Random.Range(0, m_crashClips.Count)];
        m_audioSource.PlayScheduled(clip.length - 0.1);
    }
}
