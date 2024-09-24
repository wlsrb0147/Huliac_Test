using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource _audioSource;
    public AudioClip audioClip;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        _audioSource.PlayOneShot(audioClip);
    }
}
