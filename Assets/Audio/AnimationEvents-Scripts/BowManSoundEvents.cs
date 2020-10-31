using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowManSoundEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] BowClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void BowManBow()
    {
        PlayRandomClip(BowClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}
