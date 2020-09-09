using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour { 




    public void ResetAttack() {
        this.transform.parent.GetComponent<EnemyController_Old>().AttackReset();
    }








    // ------- Archer -----------------------------------




    // ------- Guard -----------------------------------
    private void GuardTurnOnCollider() {
        this.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = true;
    }

    private void GuardTurnOffCollider() {
        this.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = false;
    }


    // ------- Zombie -----------------------------------

    public void FinishBerserkerAnimation() {
        this.transform.parent.GetComponent<EnemyController_Old>().ActivatingBerserker = false;
    }

    private void ZombieTurnOnCollider() {
        this.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = true;
    }

    private void ZombieTurnOffCollider() {
        this.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = false;
    }
    







    // ------- Soldier -----------------------------------

    private void SoldierTurnOnCollider() {
        this.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = true;
    }

    private void SoldierTurnOffCollider() {
        this.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = false;
    }


}
