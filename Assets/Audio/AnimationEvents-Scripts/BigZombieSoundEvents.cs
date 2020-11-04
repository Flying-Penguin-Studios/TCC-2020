using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigZombieSoundEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] GolemAttackClips;

    [SerializeField]
    private AudioClip[] GolemRageClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void GolemAttack()
    {
        PlayRandomClip(GolemAttackClips);
    }

    private void GolemRage()
    {
        PlayRandomClip(GolemRageClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}
