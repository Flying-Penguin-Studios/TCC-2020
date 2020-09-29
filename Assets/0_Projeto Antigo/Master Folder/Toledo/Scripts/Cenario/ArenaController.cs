using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : MonoBehaviour {


    public GameObject Walls;
    public GameObject Archer;
    public GameObject Guard;
    public GameObject Zombie;
    public GameObject LimitadoresCerca;

    private bool P1Inside;
    private bool P2Inside;
    private bool ArenaCompleted;
    private bool ArenaStart;
    private int Wave;


    private void Start() {
        ArenaCompleted = GameController_OLD.Singleton.ArenaCompleted;
        ArenaStart = false;
        Wave = 1;
    }


    private void Update() {        
        StartArena();
        ArenaComplete();
    }




    private void OnTriggerStay(Collider other) {

        if(other.CompareTag("Player") && (other.gameObject.name == "Player 1")) {
            P1Inside = true;
        }

        if(other.CompareTag("Player") && (other.gameObject.name == "Player 2")) {
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

        if(!GameController_OLD.Singleton.InstantiatedPlayer1.GetComponent<PlayerController>().ToVivo) { P1Inside = true; }
        if(!GameController_OLD.Singleton.InstantiatedPlayer2.GetComponent<PlayerController>().ToVivo) { P2Inside = true; }

        if(P1Inside && P2Inside && !ArenaStart && !ArenaCompleted) {
            Walls.SetActive(true);
            StartCoroutine(WallsUp());
            StartCoroutine(WaveSpawn());
        }       
    }


    private IEnumerator WallsUp() {
        for(float i = -3; i <= 3; i = i + Time.deltaTime * 1.5f) {            
            Walls.transform.localPosition = new Vector3(Walls.transform.localPosition.x, i, Walls.transform.localPosition.z);
            yield return null;
        }
    }


    private IEnumerator WallsDown() {
        for(float i = 0; i >= -4; i = i - Time.deltaTime * 2) {
            if(Walls) { Walls.transform.localPosition = new Vector3(Walls.transform.localPosition.x, i, Walls.transform.localPosition.z); }            
            yield return null;
        }
        Destroy(Walls, 1);
    }


    private void ArenaComplete() {
        if(ArenaStart && GameController_OLD.Singleton.cenarioController.GetComponent<CenarioController>().ArenaEnemyCount == 0) {
            StartCoroutine(WallsDown());
            DestroiLimitadores();
            GameController_OLD.Singleton.ArenaCompleted = true;
        }
    }


    
    private IEnumerator WaveSpawn() {

        GameController_OLD.Singleton.cenarioController.GetComponent<CenarioController>().ArenaEnemyCount = 28;
        float WaveDuration = 15;

        ArenaStart = true;



        Vector3 Spawn1 = new Vector3(45.5f, 1, 238);
        Vector3 Spawn2 = new Vector3(39, 1, 235);
        Vector3 Spawn3 = new Vector3(52, 1, 235);
        Vector3 Spawn4 = new Vector3(36, 1, 230);
        Vector3 Spawn5 = new Vector3(45.5f, 1, 230);
        Vector3 Spawn6 = new Vector3(55, 1, 230);
        Quaternion Rotation = Quaternion.Euler(0, 180, 0);


        yield return new WaitForSeconds(2);

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





    private void DestroiLimitadores() {
        Destroy(LimitadoresCerca, 1);
    }



}
