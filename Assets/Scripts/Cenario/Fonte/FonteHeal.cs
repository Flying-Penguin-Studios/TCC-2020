using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FonteHeal : MonoBehaviour {




    private float fireRate = 1;
    private float nextFire = 0;




    private void OnTriggerStay(Collider other) {
        
        if(other.CompareTag("Player")) {

            if(Time.time >= nextFire) {

                other.GetComponent<PlayerController>().Heal(1);
                nextFire = Time.time + fireRate;
            }
        }

    }




}
