using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Biud_Handle : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;



    private void Start() {
        Player1 = GameController.Singleton.InstantiatedPlayer1;
        Player2 = GameController.Singleton.InstantiatedPlayer2;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("P1_RB") || Input.GetButtonDown("P1_LB"))
        {
            //Player1.gameObject.SetActive(!Player1.gameObject.activeSelf);
            //Player2.gameObject.SetActive(!Player2.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("P1_Select")) {

            //if (Player1.gameObject.activeSelf)
            SceneManager.LoadScene(0);
            //else
            //    SceneManager.LoadScene(Player2.gameObject.scene.name);

        }


    }
}
