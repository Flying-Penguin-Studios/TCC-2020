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

            l_collider = Physics.OverlapSphere(transform.position, 6, l_Mask);

            foreach (Collider obj in l_collider)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();

                if (obj.gameObject.GetComponent<Zombie>())
                {
                    if (!obj.gameObject.GetComponent<Zombie>().berserkerModeOn)
                        rb.velocity /= 2;
                }
                else
                {
                    Vector3 Dir = (rb.transform.position - transform.position).normalized * -1;

                    float Dist = Vector3.Distance(transform.position, rb.transform.position);
                    Dist = Mathf.Pow(Dist, 1.5f);

                    float PowerForce = Power / Dist * 50;
                    PowerForce = Mathf.Clamp(PowerForce, 0.01f, Mathf.Pow(10, 3));
                    Dir.y = 0;

                    rb.AddForce(Dir * PowerForce, ForceMode.VelocityChange);
                }
            }

            yield return null;
        }

        GetComponent<Collider>().enabled = true;
        StartCoroutine("Vortex");
        yield return null;
    }

    IEnumerator Vortex()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        float TimeDuration = Time.time + Duration;

        while (TimeDuration >= Time.time)
        {
            l_collider = Physics.OverlapSphere(transform.position, 6, l_Mask);

            foreach (Collider obj in l_collider)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();

                if (obj.name.ToUpper().Contains("ZOMBI"))
                {
                    if (!obj.GetComponent<EnemyController_Old>().BerserkerModeOn)
                        rb.velocity /= 2;
                }
                else
                {
                    Vector3 Direction = (rb.transform.position - transform.position).normalized * -1;

                    float Distance = Vector3.Distance(transform.position, rb.transform.position);
                    Distance = Mathf.Pow(Distance, 1.5f);

                    float PowerForce = Power / Distance * 50;
                    PowerForce = Mathf.Clamp(PowerForce, 0.01f, Mathf.Pow(10, 3));
                    Direction.y = 0;

                    rb.AddForce(Direction * PowerForce, ForceMode.VelocityChange);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        l_collider = Physics.OverlapSphere(transform.position, 4, l_Mask);

        foreach (Collider obj in l_collider)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }

        Player.GetComponent<Vortex>().CountCD();
        Destroy(gameObject);
        yield return null;
    }
}
