using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private LayerMask l_Mask;

    private Collider[] l_Player;

    [SerializeField]
    private int Power = 10;
    [SerializeField]
    private int Duration = 5;
    [SerializeField]
    private float TickTime = 1f;

    private Material material;
    private float WaveRate = 1.25f;
    private float NextWave;

    private PlayerController Player;

    void Start()
    {
        //StartCoroutine("Heal");
        material = this.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material;
        StartCoroutine("OpeningEffect");
        NextWave = Time.time + 1.75f;
        rb = GetComponent<Rigidbody>();
    }

    public void SetPlayer(PlayerController n_Player)
    {
        Player = n_Player;
    }

    private void FixedUpdate()
    {
        if (Time.time >= NextWave)
        {
            StartCoroutine("Wave");
            NextWave = Time.time + WaveRate;
        }

        FakeGravity();

    }

    IEnumerator Heal()
    {
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

        StartCoroutine("FadeOut");
        Player.GetComponent<Heal>().CountCD();
        yield return null;
    }

    IEnumerator OpeningEffect()
    {
        float t = 1;

        for (float i = 1; i >= 0.5f; i -= 0.01f * 3)
        {
            t -= i / 600;
            material.SetFloat("_Raio", i);
            material.SetFloat("_BorderRadius", t);

            yield return null;
        }

        StartCoroutine("Heal");
        yield return null;
    }

    IEnumerator Wave()
    {
        for (float i = 1.1f; i >= 0.4f; i -= 0.015f)
        {
            material.SetFloat("_RaioOnda", i);
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        for (float i = 1f; i >= 0; i -= 0.025f)
        {
            material.SetFloat("_Alpha", i);

            float t = i - (i * i);

            if (t <= 0)
            {
                t = 0;
                material.SetFloat("_EmissionPower", t);
            }

            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }


    private void FakeGravity() {
        rb.velocity += Vector3.down;
    }

}
