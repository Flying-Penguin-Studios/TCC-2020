using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : HitBoss
{
    public int Damage;

    private void Start()
    {
        ActualDamage = Damage;
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        transform.Translate(transform.up * 14 * Time.deltaTime);
    }
}
