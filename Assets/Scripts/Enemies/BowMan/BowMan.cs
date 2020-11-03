using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : Mob {


    public GameObject Bow;
    public GameObject projetil;



    protected override void Combat() {
        Target = SetTarget();

        if(Vector3.Distance(transform.position, Target.transform.position) >= maxDistanceInCombat) {
            LeaveCombat();
            return;
        }

        LookToTarget();
        Attack();
    }


    protected override void Attack() {
        if(!attacking) {            
            attacking = true;
            anim.SetTrigger("Attack");
        }        
    }



}
