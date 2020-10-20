using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour {


    public float speed;
    public int dano;



    private void Start() {
        Destroy(this.gameObject, 2);
    }


    private void FixedUpdate() {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }



    private void OnTriggerEnter(Collider other) {

        if(other.CompareTag("Player")) {

            if((other.name == "Player1") || other.name == "Player2") {
                other.GetComponent<PlayerController>().TakeDamage(dano);
                Destroy(this.gameObject);
            }


        }

    }



}
