using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : Mob {


    public GameObject Bow;
    public GameObject projetil;



    protected override void Combat() {
        Target = SetTarget();
        Attack();
    }


    protected override void Attack() {
        if(!attacking) {
            transform.LookAt(Target.transform.position);
            attacking = true;
            anim.SetTrigger("Attack");
        }        
    }



}
