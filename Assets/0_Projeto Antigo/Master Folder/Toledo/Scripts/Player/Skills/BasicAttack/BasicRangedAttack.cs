using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedAttack : MonoBehaviour {

    
    public GameObject Projetil;
    public float FireRate;

    private float NextFire;
    private GameObject InstantiatedProjetil;
    private int PlayerAttackPower;
    private Vector3 SpawnPosition;
    private Transform FireOrigin;

    private Animator anim;

    string PlayerInput;



    private void Start() {
        anim = this.transform.GetChild(0).GetComponent<Animator>();
        FireOrigin = this.transform.GetChild(3);
        NextFire = 0;
        PlayerAttackPower = 4;
        SetPlayerInput();
    }


    private void FixedUpdate() {
        Attack();
    }



    private void Attack() {          

        if(Input.GetButton(PlayerInput) && (Time.time >= NextFire)) {
            //anim.SetInteger("Attack", 1);
            //Invoke("InstantiateProjetil", 0.35f);
            //Invoke("FinishAttackAnimation", 0.25f);
            InstantiateProjetil();
            NextFire = Time.time + FireRate;
        }
    }


    private void FinishAttackAnimation() {
        anim.SetInteger("Attack", 0);
    }


    private void SetPlayerInput() {
        if(this.gameObject.name == "Player 1") {
            PlayerInput = "P1_X";
        } else if(this.gameObject.name == "Player 2") {
            PlayerInput = "P2_X";
        }
    }

    private void InstantiateProjetil() { 
        //SpawnPosition = new Vector3(this.transform.position.x - 0.65f, this.transform.position.y + 1, this.transform.position.z + 0.45f) + this.transform.forward * 0.5f;
        SpawnPosition = FireOrigin.position + this.transform.forward * 0.5f;
        InstantiatedProjetil = Instantiate(Projetil, SpawnPosition, this.transform.rotation);
        InstantiatedProjetil.GetComponent<ProjetilMovement>().PlayerAttackPower = PlayerAttackPower;
        InstantiatedProjetil.GetComponent<ProjetilMovement>().Player = this.gameObject;
    }

}
