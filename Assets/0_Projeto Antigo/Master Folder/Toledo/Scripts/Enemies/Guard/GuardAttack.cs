using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAttack : MonoBehaviour {


    private float AttackRate = 0.5f;
    private float P1NextAttack;
    private float P2NextAttack;


    private void Start() {
        P1NextAttack = 0;
    }


    private void OnTriggerEnter(Collider other) {

        if(other.CompareTag("Player")) {

            if((other.name == "Player 1") && (Time.time >= P1NextAttack)) {
                other.GetComponent<PlayerController>().TakeDamage(10);
                P1NextAttack = Time.time + AttackRate;
            }

            if((other.name == "Player 2") && (Time.time >= P2NextAttack)) {
                other.GetComponent<PlayerController>().TakeDamage(10);
                P2NextAttack = Time.time + AttackRate;
            }

        }

    }




}
