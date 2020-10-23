using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoss : MonoBehaviour
{
    protected int ActualDamage = 0;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        player.TakeDamage(ActualDamage);
    }

    public void setNewDamage(int Dano)
    {
        ActualDamage = Dano;
    }
}
