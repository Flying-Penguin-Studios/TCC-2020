using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenarioController : MonoBehaviour {


    // ----- Player -----------------
    public GameObject PlayerParent;
    [Space(20)]
    public Vector3 Player1StartPosition;
    public Vector3 Player2StartPosition;

    [Space(20)]
    // ----- Check Points -----------
    public GameObject LastCheckPoint;


    // ----- Check Points -----------
    [HideInInspector]
    public bool Arena1Complete;

    // ----- Arena ------------------
    public int ArenaEnemyCount;




    private void Start() {
        GameController.Singleton.Player1StartPosition = Player1StartPosition;
        GameController.Singleton.Player2StartPosition = Player2StartPosition;
        GameController.Singleton.playerParent = PlayerParent;
        GameController.Singleton.cenarioController = this.gameObject;
        GameController.Singleton.LastCheckPoint = 0;
        ArenaEnemyCount = 0;
    }






    /// <summary>
    /// Salva no GameController o último Check Point alvançado.
    /// </summary>
    public void SetCheckPoint() {
        GameController.Singleton.LastCheckPoint = LastCheckPoint.GetComponent<CheckPoint>().CheckPointNumber;
        GameController.Singleton.CheckPointPosition = LastCheckPoint.transform.position;
    }



    /// <summary>
    /// Retorna a distancia do Terreno abaixo do player, caso haja um terereno abaixo.
    /// </summary>
    /// <returns></returns>
    public (bool HasGround, float YPosition) TerrainPossition(GameObject player) {

        RaycastHit terrain;
        if(Physics.Raycast(player.transform.position + Vector3.up, Vector3.down, out terrain, 15)) {

            if(terrain.collider.CompareTag("Terrain")) {
                return (true, terrain.collider.transform.position.y);
            }
        }

        ///Em caso do jogador pular plataformas e n ter chão abaixo.
        return (false, 0);
    }


}
