using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTutorial : MonoBehaviour
{
    public AudioClip[] musicClips;
    private AudioSource audioSource;

    private int currentClipIndex = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayNextClip();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextClip();
        }
    }

    private void PlayNextClip()
    {
        audioSource.clip = musicClips[currentClipIndex];

        audioSource.Play();

        currentClipIndex = (currentClipIndex + 1) % musicClips.Length;
    }
}
