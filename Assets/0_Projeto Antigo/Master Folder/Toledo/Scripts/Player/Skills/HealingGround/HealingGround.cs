using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingGround : MonoBehaviour {


    public GameObject HealingParticle;
    public float HealingGroundCooldown;
    private float NextHealingGround;



    private void Start() {
        NextHealingGround = 0;
    }


    private void Update() {

        if(Input.GetButton("P1_B") && (Time.time >= NextHealingGround)) {
            Instantiate(HealingParticle, new Vector3(this.transform.position.x, 0, this.transform.position.z), this.transform.rotation);
            NextHealingGround = Time.time + HealingGroundCooldown;
        }

    }




}
