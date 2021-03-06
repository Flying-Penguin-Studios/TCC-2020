﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{


    public float speed;
    public int dano;
    public GameObject impactVFX;



    private void Start()
    {
        Destroy(this.gameObject, 0.75f);
    }


    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(dano);
            Instantiate(impactVFX, other.transform.position + Vector3.up, impactVFX.transform.rotation);
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }
}
