using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {


    public GameObject Archer;
    public GameObject Guard;
    public GameObject Zombie;
    public GameObject EnemyParent;

    private GameObject InstantiatedEnemy;
    private int LastCheckPoint;



    private void Start() {
        Invoke("DelaySpawn", 0.1f);
    }






    private void DelaySpawn() {
        LastCheckPoint = GameController.Singleton.LastCheckPoint;
        SpawnEnemy();
    }



    private void SpawnEnemy() {

        //Primeiro Encontro
        InstantiateEnemy(Guard, new Vector3(27, 0.5f, 37), new Vector3(0, 180, 0));
        InstantiateEnemy(Guard, new Vector3(23, 0.5f, 32), new Vector3(0, 190, 0));
        InstantiateEnemy(Guard, new Vector3(31, 0.5f, 29), new Vector3(0, 237, 0));


        //Segundo Encontro
        InstantiateEnemy(Guard, new Vector3(34, 1.7f, 65), new Vector3(0, 180, 0));
        InstantiateEnemy(Guard, new Vector3(34, 1.2f, 56), new Vector3(0, 180, 0));
        InstantiateEnemy(Guard, new Vector3(30, 1.2f, 60), new Vector3(0, 180, 0));
        InstantiateEnemy(Archer, new Vector3(20.5f, 1.8f, 68.5f), new Vector3(0, 180, 0));

        //Terceiro Encontro
        InstantiateEnemy(Guard, new Vector3(21, 2f, 140), new Vector3(0, 180, 0));
        InstantiateEnemy(Guard, new Vector3(28, 2f, 140), new Vector3(0, 180, 0));
        InstantiateEnemy(Guard, new Vector3(32, 2f, 144), new Vector3(0, 180, 0));
        InstantiateEnemy(Guard, new Vector3(16, 2f, 144), new Vector3(0, 180, 0));
        InstantiateEnemy(Zombie, new Vector3(24, 2.5f, 143), new Vector3(0, 180, 0));

        //Quarto Encontro
        InstantiateEnemy(Archer, new Vector3(-22.5f, 1f, 145f), new Vector3(0, 180, 0));
        InstantiateEnemy(Guard, new Vector3(-48, 0.5f, 154), new Vector3(0, 90, 0));
        InstantiateEnemy(Guard, new Vector3(-50, 0.7f, 148), new Vector3(0, 90, 0));
        InstantiateEnemy(Guard, new Vector3(-48, 0.3f, 140), new Vector3(0, 90, 0));
        InstantiateEnemy(Guard, new Vector3(-35, 1f, 145), new Vector3(0, -200, 0));

    }



    private void InstantiateEnemy(GameObject enemy, Vector3 EnemyPosition, Vector3 EnemyRotation, bool patrol = false) {
        InstantiatedEnemy = Instantiate(enemy, EnemyPosition, Quaternion.Euler(EnemyRotation));
        InstantiatedEnemy.transform.SetParent(EnemyParent.transform);

        if(patrol) {
            InstantiatedEnemy.GetComponent<EnemyController_Old>().HasPatrol = true;
        }

    }


}
