using UnityEngine;

public class GirlSoundEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] JumpClips;

    [SerializeField]
    private AudioClip[] ExplosionClips;

    [SerializeField]
    private AudioClip[] DashClips;

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

    private void GirlDash()
    {
        PlayRandomClip(DashClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}