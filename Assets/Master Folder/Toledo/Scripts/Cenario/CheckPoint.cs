using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

        
    public int CheckPointNumber;
    private GameObject CenarioController;
    private bool Saved;
    private bool Activated;
    private bool StartParticle;




    private void Start() {
        Saved = false;
        StartParticle = false;
    }




    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            if(!Saved) {
                SavePoint();
                ReviveAliado();
                Saved = true;
            }
        }
    }
    

    /// <summary>
    /// Seta no CenarioController o último Check Point alcançado, e em seguida define o memso valor no GameController.
    /// </summary>
    private void SavePoint() {
        CenarioController = GameController.Singleton.cenarioController;
        CenarioController.GetComponent<CenarioController>().LastCheckPoint = this.gameObject;
        CenarioController.GetComponent<CenarioController>().SetCheckPoint();
        GameController.Singleton.SetCheckPoint();
        Destroy(this.gameObject, 1);
    }


    private void ReviveAliado() {
        if(GameController.Singleton.P1IsAlive) {
            GameController.Singleton.InstantiatedPlayer2.transform.position = this.transform.position;
            GameController.Singleton.Reviveplayer(GameController.Singleton.InstantiatedPlayer2);
        } else if(GameController.Singleton.P2IsAlive) {
            GameController.Singleton.InstantiatedPlayer1.transform.position = this.transform.position;
            GameController.Singleton.Reviveplayer(GameController.Singleton.InstantiatedPlayer1);
        }
    }


}
