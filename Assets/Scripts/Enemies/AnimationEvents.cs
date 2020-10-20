using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour {


    private Mob mob;
    



    private void Start() {
        mob = transform.parent.GetComponent<Mob>();
    }



    private void SetAttackFalse() {
        mob.attacking = false;
        mob.sword.GetComponent<BoxCollider>().enabled = false;
    }



    






}
