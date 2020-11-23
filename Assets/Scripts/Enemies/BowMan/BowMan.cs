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


    public override void TakeDamage(int damage, string player) {

        if(!isVulnerable) {
            return;
        }

        if (damage > 0) { Stag(); }

        base.TakeDamage(damage, player);
    }


    public void KnockBack(Vector3 punchPosition) {

        inStag = true;
        anim.SetTrigger("Stag");
        FreezeConstraints(false);


        Vector3 direction = (transform.position - punchPosition).normalized;
        Vector3 impulse = direction * 5 + (Vector3.up * 6);

        rb.velocity = impulse;
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

}
