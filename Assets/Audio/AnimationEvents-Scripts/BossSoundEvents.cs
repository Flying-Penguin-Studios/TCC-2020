using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] BossAttackVoiceClips;

    [SerializeField]
    private AudioClip[] BossAttackSFXClips;

    [SerializeField]
    private AudioClip[] BossDashVoiceClips;

    [SerializeField]
    private AudioClip[] BossDashSFXClips;

    [SerializeField]
    private AudioClip[] BossMagicScreamClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PokiAttack()
    {
        PlayRandomClip(BossAttackVoiceClips);
        PlayRandomClip(BossAttackSFXClips);
    }

    private void PokiDash()
    {
        PlayRandomClip(BossDashVoiceClips);
        PlayRandomClip(BossDashSFXClips);
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
