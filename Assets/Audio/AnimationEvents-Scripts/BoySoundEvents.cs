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
    private AudioClip[] ShieldCrystalClips;

    [SerializeField]
    private AudioClip[] GroundPunchClips;

    [SerializeField]
    private AudioClip[] GroundPunchImpactClips;

    [SerializeField]
    private AudioClip[] GrowlClips;

    [SerializeField]
    private AudioClip[] DashClips;

    [SerializeField]
    private AudioClip[] PunchClips;

    [SerializeField]
    private AudioClip[] MaleFootstepsClips;

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

    private void BoyShieldCrystal()
    {
        PlayRandomClip(ShieldCrystalClips);
    }

    private void BoyGroundPunch()
    {
        PlayRandomClip(GroundPunchClips);
    }

    private void BoyGroundPunchImpact()
    {
        PlayRandomClip(GroundPunchImpactClips);
    }

    private void BoyGrowl()
    {
        PlayRandomClip(GrowlClips);
    }

    private void BoyDash()
    {
        PlayRandomClip(DashClips);
    }

    private void BoyPunch()
    {
        PlayRandomClip(PunchClips);
    }

    private void BoyFootsteps()
    {
        PlayRandomClip(MaleFootstepsClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}
