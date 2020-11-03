using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour {


    private SwordMan swordMan;
    private BowMan bowMan;
    private Zombie zombie;




    private void Start() {
        swordMan = transform.parent.GetComponent<SwordMan>();
        bowMan = transform.parent.GetComponent<BowMan>();
        zombie = transform.parent.GetComponent<Zombie>();
    }












    private void SetAttackFalse() {

        if(transform.parent.name == "SwordMan") {
            swordMan.attacking = false;
            swordMan.sword.GetComponent<BoxCollider>().enabled = false;
        } else if(transform.parent.name == "BowMan") {
            bowMan.attacking = false;
        }        
    }


    private void ResetStag() {

        if(transform.parent.name == "SwordMan") {
            swordMan.inStag = false;
            swordMan.FreezeConstraints(true);
        } else if(transform.parent.name == "BowMan") {
            bowMan.inStag = false;
            bowMan.FreezeConstraints(true);
        }

        

    }




    // ============== SwordMan ===============================


    private void SetSwordManAttackFalse() {
        swordMan.attacking = false;
        swordMan.sword.GetComponent<BoxCollider>().enabled = false;
    }

    private void ResetSwordManStag() {
        swordMan.inStag = false;
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






    // ============== Zombie ===============================



    private void ResetZombieAttack() {
        zombie.attacking = false;        
    }


    private void TurnOfColliders() {
        zombie.LHand.GetComponent<BoxCollider>().enabled = false;
        zombie.RHand.GetComponent<BoxCollider>().enabled = false;
    }

    private void BecomeVulnerable() {
        zombie.isVulnerable = true;
    }

    






}
