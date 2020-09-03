using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : PlayerHit
{
    //void Start()
    //{
    //    SetPlayer(GetComponentInParent<Juninho>());
    //}

    protected override void EnterDamage(GameObject n_gameObject)
    {
        CalcDamage(n_gameObject);
        DamageInteraction(n_gameObject);
    }

    protected override void DamageInteraction(GameObject n_gameObject)
    {
        Rigidbody rb = n_gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        Vector3 Dir = (n_gameObject.transform.position - Player.transform.position).normalized;
        rb.AddForce(Dir * 15, ForceMode.Impulse);
    }

}
