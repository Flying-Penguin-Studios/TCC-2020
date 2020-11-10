using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointController : MonoBehaviour
{
    public static CheckPointController Singleton = null;
    private void Awake()
    {
        if (gameObject.scene.name.ToUpper().Contains("Menu".ToUpper()))
        {
            Singleton = null;
            Destroy(gameObject);
        }

        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //public Transform SpawnPoint1;
    //public Transform SpawnPoint2;

    public Vector3 Pos1;
    public Vector3 Pos2;
    public Vector3 CamPos;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Teste_Controllers1");
        }
    }

    public void SetPositons(Transform Pos1, Transform Pos2, Transform CamPos)
    {
        this.Pos1 = Pos1.position;
        this.Pos2 = Pos2.position;
        this.CamPos = CamPos.position;
    }

    public void SetPlayers(GameObject Player)
    {
        Player.transform.position = Pos1;
    }

    public void SetPlayers(GameObject Player1, GameObject Player2)
    {
        Player1.transform.position = Pos1;
        Player2.transform.position = Pos2;

        if (CamPos != Vector3.zero)
        {
            GameObject.FindObjectOfType<CameraPosition>().gameObject.transform.position = CamPos;
        }
    }
}
