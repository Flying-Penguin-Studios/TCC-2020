using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedAttackP2 : MonoBehaviour {


    public GameObject Projetil;
    public float FireRate;

    private float NextFire;
    private GameObject InstantiatedProjetil;
    private int PlayerAttackPower;
    private Vector3 SpawnPosition;



    private void Start() {
        NextFire = 0;
        //PlayerAttackPower = this.GetComponent<PlayerController>().PlayerAttackPower.
        PlayerAttackPower = 4;
    }


    private void FixedUpdate() {
        Attack();
    }



    private void Attack() {

        if(Input.GetKey(KeyCode.Space) && (Time.time >= NextFire)) {

            SpawnPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z) + this.transform.forward * 0.5f;
            InstantiatedProjetil = Instantiate(Projetil, SpawnPosition, this.transform.rotation);
            InstantiatedProjetil.GetComponent<ProjetilMovement>().PlayerAttackPower = PlayerAttackPower;
            InstantiatedProjetil.GetComponent<ProjetilMovement>().Player = this.gameObject;
            NextFire = Time.time + FireRate;

        }
    }




}
