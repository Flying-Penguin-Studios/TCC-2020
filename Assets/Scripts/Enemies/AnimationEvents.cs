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










    // ============== SwordMan ===============================


    private void SetAttackFalse() {
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
