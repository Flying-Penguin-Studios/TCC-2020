using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : HitBoss
{
    public int Damage;
    private BoxCollider Collider;


    private void Start()
    {
        Collider = GetComponent<BoxCollider>();
        Invoke("Enable", 0.6f);
        Destroy(gameObject, 3.2f);
    }

    void Enable()
    {
        Collider.enabled = true;
        Invoke("Desable", 0.4f);
    }

    void Desable()
    {
        Collider.enabled = false;
    }

    public void SetDamage(int D)
    {
        ActualDamage = D;
    }
}
