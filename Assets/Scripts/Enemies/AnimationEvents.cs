using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour {


    private SwordMan swordMan;
    private BowMan bowMan;




    private void Start() {
        swordMan = transform.parent.GetComponent<SwordMan>();
        bowMan = transform.parent.GetComponent<BowMan>();
    }










    // ============== SwordMan ===============================


    private void SetAttackFalse() {
        swordMan.attacking = false;
        swordMan.sword.GetComponent<BoxCollider>().enabled = false;
    }






    // ============== BowMan ===============================



    private void ResetBowManAttack() {
        Invoke("ResetBowManAttackAfterDelay", 2);
    }



    private void ResetBowManAttackAfterDelay() {
        bowMan.attacking = false;
    }


    private void Projetil() {
        
        Instantiate(bowMan.projetil, bowMan.Bow.transform.position , transform.rotation);
    }




}
