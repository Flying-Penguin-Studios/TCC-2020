using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    private GameObject Player1;
    private GameObject Player2;
    private float MaxDistanceBtwPlayers = 15;

    [Header("Velocidade"), Tooltip("Velocidade com que a câmera se afasta ou se apróxima dos Players no eixo Z.")]
    public float ZSpeed;
    
    [Header("Multi-Player")]
    //[Range(1, 8), Tooltip("Altura mínima da Cam. Quando os 2 players se encontram colados um no outro.")]
    //public float MultMinYDistance;

    [Range(0, 15), Tooltip("Afastamento da Câmera em Y")]  
    public float MultYDistance = 6;

    [Range(9, 15), Tooltip("Altura máxima da Cam. Quando os 2 players se encontram completamente afastados.")]
    public float MultMaxYDistance;
    [Range(5, 25), Tooltip("Afastamento da Câmera em Z. Define o quão longe um player pode se afastar do outro no eixo X.")]
    public float MultZDistance;
    [Tooltip("Rotação Mínima em X quando os players estão próximos.")]
    public float MinXRotation;
    [Tooltip("Rotação Máxima em X quando os players estão Afastados.")]
    public float MaxXRotation;


    [Header("Single Player"), Space(10)]
    [Range(0, 15), Tooltip("Afastamento da Câmera em Y")]
    public float SingleMaxYDistance = 6;
    [Range(0, 20), Tooltip("Altura da Câmera.")]
    public float SingleMaxZDistance = 8;
    [Range(20, 75), Tooltip("Rotação da Câmera em X.")]
    public float SingleRotation;

    private bool P1IsAlive;
    private bool P2IsAlive;
    private float dist;
    private float CamY;
    private Vector3 CamPosition;
    private Vector3 CamBasePosition;
    private Vector2 CamDistance;
    private float PlayerDistance;
    private float PlayerZDistance;
    private float CameraXRotation;
    private Quaternion CameraFinalRotation;


    // TEMPORARIO - APAGAR DEPOIS QUE CORRIGIR O GAMECONTROLLER
    public GameObject P1;
    public GameObject P2;


    private void Start() {
        //Player1 = GameController.Singleton.InstantiatedPlayer1;
        //Player2 = GameController.Singleton.InstantiatedPlayer2;

        Player1 = P1;
        Player2 = P2;
    }


    private void Update() {
        CheckPlayerDistance();
        SetCameraDistance();
        SetCameraRotation();
    }




    //Ajusta posição da câmera de agordo com a posição dos Jogadores;
    private void SetCameraDistance() {

        CamBasePosition = this.transform.parent.transform.localPosition;
        //P1IsAlive = Player1.GetComponent<PlayerController>().ToVivo;
        //P2IsAlive = Player2.GetComponent<PlayerController>().ToVivo;

        P1IsAlive = P1.GetComponent<PlayerController>().ToVivo;
        P2IsAlive = P2.GetComponent<PlayerController>().ToVivo;


        if(P1IsAlive && P2IsAlive) {

            CamDistance = new Vector2(CamBasePosition.y + MultYDistance, -PlayerDistance + 2);

            //Limita a elevação da câmera entre um mínimo e um máximo.
            //CamDistance.x = Mathf.Clamp(CamDistance.x, MultMinYDistance, MultMaxYDistance);



            //No eixo Z, mantem a camera sempre antes do player com menor valor de Z.
            PlayerZDistance = Player1.transform.position.z - Player2.transform.position.z;

            //Se a distância Z entre os jogadores for negativa, significa que Player1 está antes do Player2.
            if(PlayerZDistance < 0) {
                CamDistance.y = Player1.transform.position.z - MultZDistance;
            } else {
                CamDistance.y = Player2.transform.position.z - MultZDistance;
            }



            //Posição final da câmera
            CamPosition = new Vector3(this.transform.position.x, CamDistance.x, CamDistance.y);
            //Fator de velocidade de movimento da câmera baseado na distância que a câmera tá de seu ponto final.. Quanto mais lónge, mais rápido ela se move.
            dist = Vector3.Distance(this.transform.position, CamPosition);



            //Move a Câmera de forma mais suave.
            this.transform.position = Vector3.MoveTowards(this.transform.position, CamPosition, ZSpeed * 0.001f * dist);

        } else {

            CamPosition = new Vector3(this.transform.position.x, CamBasePosition.y + SingleMaxYDistance, CamBasePosition.z - SingleMaxZDistance);
            dist = Vector3.Distance(this.transform.position, CamPosition);

            this.transform.position = Vector3.MoveTowards(this.transform.position, CamPosition, ZSpeed * 0.001f * dist);           

        } 

    }



    //Ajusta rotação da câmera de agordo com a posição dos Jogadores;
    private void SetCameraRotation() {

        if(P1IsAlive && P2IsAlive) {

            float LimitedPlayerDistance = Mathf.Clamp(PlayerDistance, 0, MaxDistanceBtwPlayers);

            //Relação entre a distancia entre os jogadores, e a rotação da Câmera...      Foórmula original:   Slider2 = (((Slider1 * 100) / 51) / 100) * 10) + 30; 
            CameraXRotation = (((LimitedPlayerDistance * 100) / MaxDistanceBtwPlayers) / 100) * (MaxXRotation - MinXRotation) + MinXRotation;


            CameraFinalRotation = Quaternion.Euler(CameraXRotation, this.transform.rotation.y, this.transform.rotation.z);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, CameraFinalRotation, Time.deltaTime * 0.3f);            

        } else {

            CameraFinalRotation = Quaternion.Euler(SingleRotation, this.transform.rotation.y, this.transform.rotation.z);

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, CameraFinalRotation, Time.deltaTime * 0.3f);

        }
    }


    private void CheckPlayerDistance() {
        PlayerDistance = Vector3.Distance(Player1.transform.position, Player2.transform.position);
    }




}