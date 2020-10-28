using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoySoundEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] JumpClips;

    [SerializeField]
    private AudioClip[] ShieldClips;

    [SerializeField]
    private AudioClip[] GroundPunchClips;

    [SerializeField]
    private AudioClip[] DashClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void BoyJump()
    {
        PlayRandomClip(JumpClips);
    }

    private void BoyShield()
    {
        PlayRandomClip(ShieldClips);
    }

    private void BoyGroundPunch()
    {
        PlayRandomClip(GroundPunchClips);
    }

    private void BoyDash()
    {
        PlayRandomClip(DashClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}
