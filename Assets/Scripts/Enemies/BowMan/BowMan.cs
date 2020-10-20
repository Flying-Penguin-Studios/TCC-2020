using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : Mob {


    public GameObject Bow;
    public GameObject projetil;



    protected override void Combat() {
        Target = SetTarget();
        transform.LookAt(Target.transform.position);
        Attack();
    }


    protected override void Attack() {
        if(!attacking) {
            attacking = true;
            anim.SetTrigger("Attack");
        }        
    }



}
