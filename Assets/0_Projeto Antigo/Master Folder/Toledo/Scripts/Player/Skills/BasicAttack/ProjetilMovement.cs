using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilMovement : MonoBehaviour {


    public GameObject Player;
    public int PlayerAttackPower;
    public int ProjetilAttackPower;
    public float ProjetilSpeed;


    private void Start() {
        Destroy(this.gameObject, 3);
        
    }


    private void Update() {
        this.transform.position += this.transform.forward.normalized * Time.deltaTime * ProjetilSpeed;
    }



    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")) {
            other.GetComponent<EnemyController_Old>().Enemy.TakeDamage(PlayerAttackPower + ProjetilAttackPower);
            other.GetComponent<EnemyController_Old>().Enemy.GenerateThreat(Player.name, PlayerAttackPower + ProjetilAttackPower);
            Destroy(this.gameObject);
        }
    }

}
