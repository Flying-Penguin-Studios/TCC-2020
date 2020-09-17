using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCheckPoint : MonoBehaviour
{

    [HideInInspector]
    public Vector3 checkPointSpawner
    {
        get { return CheckPointSpawner.position; }
    }

    public Transform CheckPointSpawner;

    //private void OnCollisionEnter(Collider other)
    //{
    //    PlayerController Player = other.GetComponent<PlayerController>();

    //    if (Player)
    //    {
    //        Player.SetIslandPoint(this);
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController Player;

        if (Player = collision.gameObject.GetComponent<PlayerController>())
        {
            Player.SetIslandPoint(this);
            print("O INICIO DO SONHO");
        }
    }
}
