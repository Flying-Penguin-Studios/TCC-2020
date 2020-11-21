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
    private AudioClip[] ChargePrepClips;

    [SerializeField]
    private AudioClip[] ChargeReleaseClips;

    [SerializeField]
    private AudioClip[] ChargePunchClips;

    [SerializeField]
    private AudioClip[] MaleFootstepsClips;

    [SerializeField]
    private AudioClip[] WoodMaleFootstepsClips;

    [SerializeField]
    private AudioClip[] MaleDamageClips;

    private AudioSource audioSource;

    RaycastHit terrain;

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

    private void BoyChargePrep()
    {
        PlayRandomClip(ChargePrepClips);
    }

    private void BoyChargeRelease()
    {
        PlayRandomClip(ChargeReleaseClips);
    }

    private void BoyChargePunch()
    {
        PlayRandomClip(ChargePunchClips);
    }

    private void BoyFootsteps()
    {
        if (Physics.Raycast(this.transform.position, Vector3.down, out terrain, 999))
        {
            if (terrain.transform.CompareTag("Terrain") || terrain.transform.CompareTag("StaticObject") || terrain.transform.CompareTag("ObjetosDeCena") || terrain.transform.CompareTag("Ponte"))
            {
                if (terrain.transform.CompareTag("Ponte"))
                {
                    PlayRandomClip(WoodMaleFootstepsClips);
                    //Aqui vc poe pra tocar o som de madeira
                }
                else
                {
                    PlayRandomClip(MaleFootstepsClips);
                    //Aqui vc poe pra tocar o som de granma
                }
            }
        }
    }

    public void BoyDamage()
    {
        PlayRandomClip(MaleDamageClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}
