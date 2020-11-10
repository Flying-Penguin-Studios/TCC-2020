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
            newPoint = (newPoint - Player.transform.forward.normalized) * .5f;
            Player.SetIslandPoint(newPoint);
        }
    }
}
