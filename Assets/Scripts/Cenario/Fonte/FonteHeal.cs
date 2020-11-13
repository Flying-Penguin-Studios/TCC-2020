using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FonteHeal : MonoBehaviour {



    public int healPerSecond = 1;
    private float fireRate = 1;
    private float nextFire = 0;




    private void OnTriggerStay(Collider other) {
        
        if(other.CompareTag("Player")) {

            if(Time.time >= nextFire) {

                other.GetComponent<PlayerController>().Heal(healPerSecond);
                nextFire = Time.time + fireRate;
            }
        }

    }




}
