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
    private AudioClip[] GrassFootstepsClips;

    [SerializeField]
    private AudioClip[] WoodFootstepsClips;

    [SerializeField]
    private AudioClip[] GirlDamageClips;

    private AudioSource audioSource;

    RaycastHit terrain;

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
        if (Physics.Raycast(this.transform.position, Vector3.down, out terrain, 999))
        {
            if (terrain.transform.CompareTag("Terrain") || terrain.transform.CompareTag("StaticObject") || terrain.transform.CompareTag("ObjetosDeCena") || terrain.transform.CompareTag("Ponte"))
            {
                if (terrain.transform.CompareTag("Ponte"))
                {
                    PlayRandomClip(WoodFootstepsClips);
                    //Aqui vc poe pra tocar o som de madeira
                }
                else
                {
                    PlayRandomClip(GrassFootstepsClips);
                    //Aqui vc poe pra tocar o som de granma
                }
            }
        }
    }

    public void GirlDamage()
    {
        PlayRandomClip(GirlDamageClips);
    }

    void PlayRandomClip(AudioClip[] Clips)
    {
        audioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
    }
}