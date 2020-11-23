using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint_OLD : MonoBehaviour {

        
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
        CenarioController = GameController_OLD.Singleton.cenarioController;
        CenarioController.GetComponent<CenarioController>().LastCheckPoint = this.gameObject;
        CenarioController.GetComponent<CenarioController>().SetCheckPoint();
        GameController_OLD.Singleton.SetCheckPoint();
        Destroy(this.gameObject, 1);
    }


    private void ReviveAliado() {
        if(GameController_OLD.Singleton.P1IsAlive) {
            GameController_OLD.Singleton.InstantiatedPlayer2.transform.position = this.transform.position;
            GameController_OLD.Singleton.Reviveplayer(GameController_OLD.Singleton.InstantiatedPlayer2);
        } else if(GameController_OLD.Singleton.P2IsAlive) {
            GameController_OLD.Singleton.InstantiatedPlayer1.transform.position = this.transform.position;
            GameController_OLD.Singleton.Reviveplayer(GameController_OLD.Singleton.InstantiatedPlayer1);
        }
    }


}
