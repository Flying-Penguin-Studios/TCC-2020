using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexZone : MonoBehaviour
{
    [SerializeField]
    private LayerMask l_Mask;

    private Collider[] l_collider;

    [SerializeField]
    private float Power = 1.5f;
    [SerializeField]
    private int Duration = 5;

    private PlayerController Player;

    void Start()
    {
        StartCoroutine("Move");
        GetComponent<Collider>().enabled = false;
    }

    public void SetPlayer(PlayerController n_Player)
    {
        Player = n_Player;
    }

    IEnumerator Move()
    {
        Vector3 Direction = transform.TransformDirection(Vector3.forward).normalized;
        Vector3 Destiny = transform.position + (Direction * 10);
        float Distance = Vector3.Distance(Direction, Destiny);        

        while (!(Distance >= 0 && Distance <= 1))
        {
            transform.position += Direction * (Time.deltaTime * 10);
            Distance = Vector3.Distance(transform.position, Destiny);
            yield return null;
        }

        GetComponent<Collider>().enabled = true;
        StartCoroutine("Vortex");
        yield return null;
    }

    IEnumerator Vortex()
    {
        float TimeDuration = Time.time + Duration;

        while (TimeDuration >= Time.time)
        {
            l_collider = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius, l_Mask);

            foreach (Collider obj in l_collider)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();

                if (obj.name.ToUpper().Contains("ZOMBI"))
                {
                    if (!obj.GetComponent<EnemyController>().BerserkerModeOn)
                        rb.velocity /= 2;
                }
                else
                {
                    Vector3 Direction = (transform.position - rb.transform.position).normalized;

                    float Distance = Vector3.Distance(transform.position, rb.transform.position);
                    Distance = Mathf.Pow(Distance, 1.5f);

                    float PowerForce = (Power / Distance) * 100;
                    PowerForce = Mathf.Clamp(PowerForce, 0.01f, Mathf.Pow(10, 4));
                    Direction.y = 0;
                    rb.AddForce(Direction * PowerForce);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        Player.GetComponent<Vortex>().CountCD();
        Destroy(gameObject);
        yield return null;
    }
}
