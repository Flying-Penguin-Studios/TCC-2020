using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldWall : MonoBehaviour
{
    public void Parede()
    {
        //Collider[] Inimigos = Physics.OverlapSphere(transform.position, 5);

        //foreach (Collider item in Inimigos)
        //{
        //    if (item.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //    {
        //        double t = Vector3.Distance(item.transform.position, transform.position);

        //        if (t < 5)
        //        {
        //            Rigidbody rb = item.GetComponent<Rigidbody>();

        //            if (rb)
        //            {
        //                Vector3 Dir = (item.transform.position - transform.position).normalized;
        //                Dir.y = 0;
        //                rb.AddForce(Dir * 5, ForceMode.Impulse);
        //            }
        //        }
        //    }
        //}

        //GetComponentInChildren<Collider>().enabled = true;
    }
}
