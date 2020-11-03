using UnityEngine;

public class GirlSoundEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] JumpClips;

    [SerializeField]
    private AudioClip[] ExplosionClips;

    [SerializeField]
    private AudioClip[] ExplosionImpactClips;

    [SerializeField]
    private AudioClip[] VortexClips;

    [SerializeField]
    private AudioClip[] VortexScreamClips;

    [SerializeField]
    private AudioClip[] DashClips;

    [SerializeField]
    private AudioClip[] FootstepsClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void GirlJump()
    {
        PlayRandomClip(JumpClips);
    }

    private void GirlExplosion()
    {
        PlayRandomClip(ExplosionClips);
    }

    private void GirlExplosionImpact()
    {
        PlayRandomClip(ExplosionImpactClips);
    }

    private void GirlVortex()
    {
        PlayRandomClip(VortexClips);
    }

    private void GirlVortexScream()
    {
        PlayRandomClip(VortexScreamClips);
    }

    private void GirlDash()
    {
        PlayRandomClip(DashClips);
    }

    private void GirlFootsteps()
    {
        PlayRandomClip(FootstepsClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}