using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManSoundEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] SwordClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void SwordManSword()
    {
        PlayRandomClip(SwordClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}
