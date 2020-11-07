using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour
{
    [SerializeField] private LayerMask l_Mask;

    private Collider[] l_Player;

    [SerializeField] private int Power = 10;
    [SerializeField] private int Duration = 5;
    [SerializeField] private float TickTime = 1f;

    private PlayerController Player;

    void Start()
    {
        StartCoroutine(Heal());
    }

    public void SetPlayer(PlayerController n_Player)
    {
        Player = n_Player;
    }

    IEnumerator Heal()
    {
        ParticleSystem[] Particles = {transform.GetChild(0).GetComponent<ParticleSystem>(),
                                      transform.GetChild(1).GetComponent<ParticleSystem>(),
                                      transform.GetChild(2).GetComponent<ParticleSystem>(),
                                      transform.GetChild(3).GetComponent<ParticleSystem>()};

        yield return new WaitForSeconds(.75f);

        float TimeDuration = Time.time + Duration;

        while (TimeDuration >= Time.time)
        {
            l_Player = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius, l_Mask);

            foreach (Collider Player in l_Player)
            {
                Player.GetComponent<PlayerController>().Heal(Power);
            }

            yield return new WaitForSeconds(TickTime);
        }


        //for (float i = 1; i > 0; i -= Time.deltaTime)
        //{
        //    Particles[0].gameObject.GetComponent<Material>().color = Alpha()
        //}

        //foreach (ParticleSystem Particle in Particles)
        //{
        //    Particle
        //}

        Player.GetComponent<Heal>().CountCD();
        Destroy(gameObject);
        yield return null;
    }

    private Color Alpha(Material Mat, float i)
    {
        Color Color = Mat.color;
        Color.a = i;
        return Color;
    }
}
