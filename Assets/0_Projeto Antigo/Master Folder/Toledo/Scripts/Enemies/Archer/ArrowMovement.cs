using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour {


    public float ArrowSpeed;


    private void Start() {
        Destroy(this.gameObject, 1.0f);
    }




    private void Update() {

        this.transform.localPosition += this.transform.forward.normalized * ArrowSpeed * Time.deltaTime;

    }


    private void OnTriggerEnter(Collider other) {

        if(other.CompareTag("Player")) {
                other.GetComponent<PlayerController>().TakeDamage(10);
                Destroy(this.gameObject);
        }
        else if (other.CompareTag("Shield"))        {
            Destroy(this.gameObject);
        }
    }
}