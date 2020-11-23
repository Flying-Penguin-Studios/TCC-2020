using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustOnRun : MonoBehaviour {

        
    private PlayerMovement PlayerMove;
    private bool Jumping;
    


    private void Start() {
        PlayerMove = this.GetComponent<PlayerMovement>();
    }

    private void FixedUpdate() {
        DustController();
    }



    private void DustController() {

        Jumping = this.GetComponent<PlayerMovement>().Jumping;

        if(!Jumping) {
            if((PlayerMove.moveAmout > 0.9f)) {
                Invoke("TurnDustOn", 0.25f);
            } else if((Input.GetAxis("P1_L_Joystick_Horizontal") == 0) && (Input.GetAxis("P1_L_Joystick_Vertical")) == 0) {
                Invoke("TurnDustOff", 0.25f);
            }
        } else {
            this.transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>().Stop();
        }

    }


    private void TurnDustOn() {
        this.transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>().Play();
    }

    private void TurnDustOff() {
        this.transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>().Stop();
        //this.transform.GetChild(2).gameObject.SetActive(false);
    }

}