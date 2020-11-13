using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour {


    public float speed;
    public int dano;
    public GameObject impactVFX;



    private void Start() {
        Destroy(this.gameObject, 0.75f);
    }


    private void FixedUpdate() {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }



    private void OnTriggerEnter(Collider other) {

        if(other.CompareTag("Player")) {

            if((other.name == "Player1") || other.name == "Player2") {
                other.GetComponent<PlayerController>().TakeDamage(dano);
                GameObject arrow =  Instantiate(impactVFX, other.transform.position + Vector3.up, impactVFX.transform.rotation);
                Destroy(arrow, 2);
                Destroy(this.gameObject);
            }
        }
    }



}
