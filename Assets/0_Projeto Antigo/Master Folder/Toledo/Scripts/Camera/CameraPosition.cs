using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour {

    public float speed, dist;

    private GameObject Player1;
    private GameObject Player2;
    private bool P1IsAlive;
    private bool P2IsAlive;
    private Vector3 Player1Position;
    private Vector3 Player2Position;
    private Vector3 CamBasePosition;


    // TEMPORARIO - APAGAR DEPOIS QUE CORRIGIR O GAMECONTROLLER
    public GameObject P1;
    public GameObject P2;
    private Vector3 P1StartPosition;


    private void Start() {
        Invoke("firstcameraposition", 0.09f);
    }


    private void Update() {
        SetCameraPosition();
        P1StartPosition = P1.transform.position;
    }





    private void firstcameraposition() {
        //this.transform.position = GameController.Singleton.Player1StartPosition;
        this.transform.position = P1StartPosition;
    }


    private void SetCameraPosition() {

        //Player1 = GameController.Singleton.InstantiatedPlayer1;
        //Player2 = GameController.Singleton.InstantiatedPlayer2;

        Player1 = P1;
        Player2 = P2;

        P1IsAlive = Player1.GetComponent<PlayerController>().ToVivo;
        P2IsAlive = Player2.GetComponent<PlayerController>().ToVivo;

        if(P1IsAlive && P2IsAlive) {

            Player1Position = Player1.transform.position;
            Player2Position = Player2.transform.position;


            //Posição final do Objeto base da Câmera (O Cubo)
            CamBasePosition.x = (Player1Position.x + Player2Position.x) / 2;
            CamBasePosition.y = (Player1Position.y + Player2Position.y) / 2;
            CamBasePosition.z = (Player1Position.z + Player2Position.z) / 2;


            dist = Vector3.Distance(this.transform.position, CamBasePosition);

            //Move o Objeto base de forma mais suave.
            this.transform.position = Vector3.MoveTowards(this.transform.position, CamBasePosition, speed * 0.0007f * dist);

        } else if(P1IsAlive && !P2IsAlive) {

            dist = Vector3.Distance(this.transform.position, Player1.transform.position);
            this.transform.position = Vector3.MoveTowards(this.transform.position, Player1.transform.position, speed * 0.0015f * dist);

        } else if(!P1IsAlive && P2IsAlive) {

            dist = Vector3.Distance(this.transform.position, Player2.transform.position);
            this.transform.position = Vector3.MoveTowards(this.transform.position, Player2.transform.position, speed * 0.0015f * dist);

        }
                

    }


}
