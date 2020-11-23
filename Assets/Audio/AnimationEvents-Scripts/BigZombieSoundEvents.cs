using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigZombieSoundEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] GolemAttackClips;

    [SerializeField]
    private AudioClip[] GolemPreRageClips;

    [SerializeField]
    private AudioClip[] GolemRageClips;

    [SerializeField]
    private AudioClip[] GolemDamageClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void GolemAttack()
    {
        PlayRandomClip(GolemAttackClips);
    }

    private void GolemPreRage()
    {
        PlayRandomClip(GolemPreRageClips);
    }

    private void GolemRage()
    {
        PlayRandomClip(GolemRageClips);
    }

    private void GolemDamage()
    {
        PlayRandomClip(GolemDamageClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}
