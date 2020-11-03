using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : HitBoss
{
    public int Damage;

    private void Start()
    {
        //ActualDamage = Damage;
        Destroy(gameObject, 1f);
    }

    public void SetDamage(int D)
    {
        ActualDamage = D;
    }

    void Update()
    {
        transform.Translate(transform.up * 14 * Time.deltaTime);
    }
}
