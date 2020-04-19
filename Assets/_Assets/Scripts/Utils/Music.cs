using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public List<AudioClip> musicQueue = new List<AudioClip>();
    public AudioSource audioSource;
    int index = 1;

    void Update()
    {
        if (!audioSource.isPlaying) PlayNextSong();
    }

    void PlayNextSong()
    {
        index++;
        if (index > musicQueue.Count - 1) index = 0;
        audioSource.clip = musicQueue[index];
        audioSource.Play();
    }
}
