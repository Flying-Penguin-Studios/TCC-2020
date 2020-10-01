using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaMob : MonoBehaviour
{
    [HideInInspector]
    public Arena Arena;

    private void OnDestroy()
    {
        Arena.EnemyCount--;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            Destroy(gameObject);
            //Destroy(gameObject, 10f);
        }
    }
}
