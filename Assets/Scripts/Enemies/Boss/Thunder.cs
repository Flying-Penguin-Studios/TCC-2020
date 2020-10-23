using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : HitBoss
{
    public int Damage;

    private void Start()
    {
        ActualDamage = Damage;
    }

    void Update()
    {
        transform.Translate(transform.up * 5 * Time.deltaTime);
    }
}
