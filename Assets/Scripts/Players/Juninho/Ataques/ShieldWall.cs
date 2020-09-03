using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldWall : MonoBehaviour
{
    public void Parede()
    {
        GetComponentInChildren<Collider>().enabled = true;
    }
}
