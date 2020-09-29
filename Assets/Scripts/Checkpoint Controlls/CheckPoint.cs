using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Transform Pos1;
    public Transform Pos2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            if (CheckPointController.Singleton)
            {
                CheckPointController.Singleton.SetPositons(Pos1, Pos2);
            }
        }
    }
}
