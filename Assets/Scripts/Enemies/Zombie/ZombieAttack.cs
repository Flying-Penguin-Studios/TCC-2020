using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour {


    


    public int danoMin;
    public int danoMax;

    private float AttackRate = 0.5f;
    private float P1NextAttack = 0;
    private float P2NextAttack = 0;





    private void OnTriggerEnter(Collider other) {

        if(other.CompareTag("Player")) {

            if((other.name == "Player1") && (Time.time >= P1NextAttack)) {
                other.GetComponent<PlayerController>().TakeDamage(Random.Range(danoMin, danoMax));
                P1NextAttack = Time.time + AttackRate;
            }

            if((other.name == "Player2") && (Time.time >= P2NextAttack)) {
                other.GetComponent<PlayerController>().TakeDamage(Random.Range(danoMin, danoMax));
                P2NextAttack = Time.time + AttackRate;
            }

        }

    }




}
