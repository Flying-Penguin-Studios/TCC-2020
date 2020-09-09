using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController_teste : MonoBehaviour
{
    #region Singleton
    public static GameController_teste Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    #endregion

    public playerStats Juninho;
    public playerStats Angie;

    void Start()
    {
        Angie = GameObject.Find(StaticVariables.Names.Angie).GetComponent<PlayerController>().getStats();
        print("Vida da Angie: " + Angie.currentLife);
    }

    // Update is called once per frame
    void Update()
    {
        //print("Vida do Juninho: " + Juninho.returnStats().currentLife);
        //print("Vida da Angie: " + Angie.returnStats().currentLife);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Angie = GameObject.Find(StaticVariables.Names.Angie).GetComponent<PlayerController>().getStats();
            SceneManager.LoadScene("Master_Room_Teste");
        }
    }
}
