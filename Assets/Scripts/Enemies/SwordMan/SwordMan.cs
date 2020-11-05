using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMan : Mob {



    public GameObject sword;












    protected override void ChaseTarget() {
        
        LookToTarget();        

        if(EnemyHasGroundToMove()) {

            acceleration = !attacking;
            Accelerate();

        } else {
            
            if(EnemyShouldJump() && !jumping && (Time.time >= nextJump)) {
                Jump();
            }

            if(!jumping) {
                acceleration = false;
                Accelerate();
            }

        }

    }



    protected override void Combat() {

        Target = SetTarget();

        if(DistanceToTarget() <= minDistanceToPlayer && !attacking && !inStag && !jumping) {
            acceleration = false;
            Attack();
        } else if(inStag) {
            FreezeConstraints(false);

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
