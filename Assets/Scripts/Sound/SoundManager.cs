using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] private AudioClip pickSound;
    [SerializeField] private AudioClip dropSound;
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPickSound()
    {
        audioSource.clip = pickSound;
        audioSource.Play();
    }

    public void PlayDropSound()
    {
        audioSource.clip = dropSound;
        audioSource.Play();
    }
}
