using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCheckPoint : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        PlayerController Player;

        if (Player = collision.gameObject.GetComponent<PlayerController>())
        {
            Vector3 newPoint = collision.contacts[0].point;
            Player.SetIslandPoint(newPoint);
        }
    }
}
