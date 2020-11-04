using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] BossAttackClips;

    [SerializeField]
    private AudioClip[] BossDashClips;

    [SerializeField]
    private AudioClip[] BossMagicScreamClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PokiAttack()
    {
        PlayRandomClip(BossAttackClips);
    }

    private void PokiDash()
    {
        PlayRandomClip(BossDashClips);
    }

    private void PokiMagicScream()
    {
        PlayRandomClip(BossMagicScreamClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}
