using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To attach to things that make sound effects :)
/// </summary>
public class CharacterSFXManager : MonoBehaviour
{
    private static CharacterSFXManager _sInstance;

    public static CharacterSFXManager Get()
    {
        return _sInstance;
    }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> stepsClipLists;
    [SerializeField] private List<AudioClip> jumpClipLists;
    [SerializeField] private List<AudioClip> hurtClipLists;
    [SerializeField] private List<AudioClip> dieClipLists;

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

        if(audioSource)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
        else
        {
            Debug.LogError("No audio source attached to: " + this.name);
        }
    }

    void Start()
    {

    }
}
