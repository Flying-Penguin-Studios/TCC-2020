using UnityEngine;

public class AngieJump : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] AngieJumpClips;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void GirlJump()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return AngieJumpClips[Random.Range(0, AngieJumpClips.Length)];
    }
}