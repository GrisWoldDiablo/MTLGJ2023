using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSFXManager : MonoBehaviour
{
    private static WallSFXManager _sInstance;
    public static WallSFXManager Get()
    {
        return _sInstance;
    }

    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private List<AudioClip> m_wallClips;
    [SerializeField] private AudioClip m_heartClip;

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
}
