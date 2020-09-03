using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour {


    private GameObject Zombie;
    private int Damage;
    public int NormalDamage;
    public int BerserkerDamage;
    private float AttackRate = 0.5f;
    private float P1NextAttack;
    private float P2NextAttack;


    private void Start() {
        Zombie = this.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject;
        P1NextAttack = 0;
    }


    private void OnTriggerEnter(Collider other) {

        Damage = (Zombie.GetComponent<EnemyController>().BerserkerModeOn == true) ? BerserkerDamage : NormalDamage;

        if(other.CompareTag("Player")) {

            if((other.name == "Player 1") && (Time.time >= P1NextAttack)) {
                other.GetComponent<PlayerController>().TakeDamage(Damage);
                P1NextAttack = Time.time + AttackRate;
            }

            if((other.name == "Player 2") && (Time.time >= P2NextAttack)) {
                other.GetComponent<PlayerController>().TakeDamage(Damage);
                P2NextAttack = Time.time + AttackRate;
            }

        }

    }






}
