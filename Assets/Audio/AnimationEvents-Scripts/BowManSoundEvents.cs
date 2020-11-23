using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowManSoundEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] BowClips;

    [SerializeField]
    private AudioClip[] DamageClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void BowManBow()
    {
        PlayRandomClip(BowClips);
    }

    private void MobDamageSound()
    {
        PlayRandomClip(DamageClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}
