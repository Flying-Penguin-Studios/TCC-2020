using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMan : Mob {



    public GameObject sword;




    protected override void Combat() {

        Target = SetTarget();

        if(DistanceToTarget() <= minDistanceToPlayer && !attacking) {
            acceleration = false;
            Attack();
        } else {
            ChaseTarget();
        }

    }


    protected override void Attack() {
        attacking = true;
        sword.GetComponent<BoxCollider>().enabled = true;
        anim.SetTrigger("Attack");
    }



}
