using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTheWorldCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController Player;

        if (Player = other.GetComponent<PlayerController>())
        {
            Player.RespawnOnLastIsland();
            print("DEU TUDO CERTO");
        }

    }
}
