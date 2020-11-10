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

        patrulheiro = false;
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


    public override void TakeDamage(int damage, string player) {

        if(!isVulnerable) {
            return;
        }

        Stag();

        base.TakeDamage(damage, player);
    }


    private void Stag() {

        if((Time.time >= nextStag)) {
            inStag = true;
            anim.SetTrigger("Stag");
            FreezeConstraints(false);
            rb.velocity = Vector3.zero;
            rb.AddForce(-transform.forward * 1.5f, ForceMode.Impulse);
            nextStag = Time.time + stagRate;
        }        
    }


    public void KnockBack(Vector3 punchPosition)    {

        inStag = true;
        anim.SetTrigger("Stag");
        FreezeConstraints(false);


        Vector3 direction = (transform.position - punchPosition).normalized;
        Vector3 impulse = direction * 5 + (Vector3.up * 6);

        rb.velocity = impulse;
    }



}
