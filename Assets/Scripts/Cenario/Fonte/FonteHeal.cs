using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FonteHeal : MonoBehaviour
{
    public GameObject healVFX;

    private float fireRate = 1;
    private float nextFire = 0;

    private bool P1Inside = false;
    private bool P2Inside = false;
    private bool vfxExist = false;

    GameObject instantiatedHealVFX;

    ParticleSystem vfx1;
    ParticleSystem vfx2;

    private void FixedUpdate()
    {
        if (P1Inside || P2Inside)
        {
            if (!vfxExist)
            {
                instantiatedHealVFX = Instantiate(healVFX, this.transform.position, this.transform.rotation);
                vfx1 = instantiatedHealVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                vfx2 = instantiatedHealVFX.transform.GetChild(1).GetComponent<ParticleSystem>();
                vfxExist = true;
            }

            instantiatedHealVFX.SetActive(true);
        }
        else
        {
            if (instantiatedHealVFX)
            {
                vfx1.Stop();
                vfx2.Stop();
                Destroy(instantiatedHealVFX, 5);
                vfxExist = false;
            }
        }
    }

    bool Healing = false;

    IEnumerator IHeal()
    {
        Healing = true;

        while (true)
        {
            foreach (PlayerController Player in l_Player)
            {
                Player.Heal(1);
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    private List<PlayerController> l_Player = new List<PlayerController>();

    private void OnTriggerEnter(Collider other)
    {
        PlayerController Player;
        if (Player = other.GetComponent<PlayerController>())
            l_Player.Add(Player);

        if (!Healing)
        {
            StartCoroutine(IHeal());
        }

        if (other.CompareTag("Player"))
        {
            if (other.name == "Player1") { P1Inside = true; }
            if (other.name == "Player2") { P2Inside = true; }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController Player;
        if (Player = other.GetComponent<PlayerController>())
            l_Player.Remove(Player);

        if (l_Player.Count == 0 && Healing)
        {
            StopCoroutine(IHeal());
            Healing = false;
        }

        if (other.CompareTag("Player"))
        {
            if (other.name == "Player1") { P1Inside = false; }
            if (other.name == "Player2") { P2Inside = false; }
        }
    }
}
