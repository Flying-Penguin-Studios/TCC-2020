using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowManAttack : MonoBehaviour {




    public int dano;
    private float fireRate = 1;
    private float nextFire = 0;




    private void FixedUpdate() {


        if(Time.time >= nextFire) {



            nextFire = Time.time + fireRate;
        }


    }








}
