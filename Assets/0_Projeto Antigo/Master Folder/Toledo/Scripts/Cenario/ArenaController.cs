using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour {


    public GameObject Archer;
    public GameObject Guard;
    public GameObject Zombie;
    

    private bool P1Inside;
    private bool P2Inside;
    private bool ArenaCompleted;
    private bool ArenaStart;
    private int Wave;


    private void Start() {
        ArenaCompleted = GameController.Singleton.Arena1Completed;
        ArenaStart = false;
        Wave = 1;
    }


    private void Update() {        
        StartArena();
        ArenaComplete();
    }




    private void OnTriggerStay(Collider other) {

        if((other.gameObject == GameController.Singleton.ScenePlayer1)) {
            P1Inside = true;
        }

        if((other.gameObject == GameController.Singleton.ScenePlayer2)) {
            P2Inside = true;
        }
    }


    private void OnTriggerExit(Collider other) {

        if(other.CompareTag("Player") && (other.gameObject.name == "Player 1")) {
            P1Inside = false;
        }

        if(other.CompareTag("Player") && (other.gameObject.name == "Player 2")) {
            P2Inside = false;
        }
    }


    private void StartArena() {

        if(!GameController.Singleton.ScenePlayer1.GetComponent<PlayerController>().ToVivo) { P1Inside = true; }
        if(!GameController.Singleton.ScenePlayer2.GetComponent<PlayerController>().ToVivo) { P2Inside = true; }


        if(P1Inside && P2Inside && !ArenaStart && !ArenaCompleted) {
            CloseArena();
            StartCoroutine(WaveSpawn());
        }       
    }


    private void CloseArena() {

        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(1).gameObject.SetActive(true);

    }



    private void ArenaComplete() {
        if(ArenaStart && GameController.Singleton.ArenaEnemyCount == 0) {
            GameController.Singleton.ArenaCompleted = true;
            Destroy(this.gameObject, 2);
        }
    }


    
    private IEnumerator WaveSpawn() {

        GameController.Singleton.ArenaEnemyCount = 28;
        float WaveDuration = 15;

        ArenaStart = true;



        Vector3 Spawn1 = new Vector3(45.5f, 1, 238);
        Vector3 Spawn2 = new Vector3(39, 1, 235);
        Vector3 Spawn3 = new Vector3(52, 1, 235);
        Vector3 Spawn4 = new Vector3(36, 1, 230);
        Vector3 Spawn5 = new Vector3(45.5f, 1, 230);
        Vector3 Spawn6 = new Vector3(55, 1, 230);
        Quaternion Rotation = Quaternion.Euler(0, 180, 0);


        yield return new WaitForSeconds(0);

        //Wave 1
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn2, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn3, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn4, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn6, Rotation)));
        yield return new WaitForSeconds(WaveDuration - 5);

        //Wave 2
        StartCoroutine(ArenaenemyConfig(Instantiate(Archer, Spawn2, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn3, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn4, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn5, Rotation)));
        yield return new WaitForSeconds(WaveDuration - 5);

        //Wave 3
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn2, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn3, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn4, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn6, Rotation)));
        yield return new WaitForSeconds(WaveDuration - 3);

        //Wave 4
        StartCoroutine(ArenaenemyConfig(Instantiate(Archer, Spawn1, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn2, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn3, Rotation)));
        yield return new WaitForSeconds(WaveDuration);

        //Wave 5
        StartCoroutine(ArenaenemyConfig(Instantiate(Zombie, Spawn1, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Archer, Spawn2, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn5, Rotation)));
        yield return new WaitForSeconds(WaveDuration);

        //Wave 6
        StartCoroutine(ArenaenemyConfig(Instantiate(Zombie, Spawn1, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn2, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn3, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn4, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Archer, Spawn6, Rotation)));
        yield return new WaitForSeconds(WaveDuration);

        //Wave 7
        StartCoroutine(ArenaenemyConfig(Instantiate(Archer, Spawn1, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn2, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn3, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Guard, Spawn4, Rotation)));
        StartCoroutine(ArenaenemyConfig(Instantiate(Zombie, Spawn6, Rotation)));

    }



    IEnumerator ArenaenemyConfig(GameObject enemy) {        

        yield return new WaitForSeconds(0.01f);

        enemy.GetComponent<EnemyController_Old>().SetRandomTarget();
        enemy.GetComponent<EnemyController_Old>().RangedAttackDistance = 20f;
        enemy.GetComponent<EnemyController_Old>().IsArenaEnemy = true;

    }







}
