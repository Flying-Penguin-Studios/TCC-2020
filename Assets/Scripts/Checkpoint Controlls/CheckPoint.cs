using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Transform Pos1;
    public Transform Pos2;
    public Transform PosCam;

    public GameObject VFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            if (CheckPointController.Singleton)
            {
                CheckPointController.Singleton.SetPositons(Pos1, Pos2, PosCam);
                GetComponent<Collider>().enabled = false;

                GameObject _VFX = Instantiate(VFX, transform);

                if (!GameController.Singleton.P1.ToVivo)
                {
                    GameController.Singleton.P1.transform.position = Pos1.position;
                    GameController.Singleton.P1._Revive();
                }

                if (!GameController.Singleton.P2.ToVivo)
                {
                    GameController.Singleton.P2.transform.position = Pos2.position;
                    GameController.Singleton.P2._Revive();
                }

                GameController.Singleton.P1.HealFull();
                GameController.Singleton.P2.HealFull();

                Destroy(_VFX, 1.3f);
                Destroy(this, 1.3f);
            }
        }
    }
}
