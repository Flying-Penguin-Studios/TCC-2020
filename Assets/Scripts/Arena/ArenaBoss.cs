using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBoss : MonoBehaviour
{
    GameObject Walls;
    GameObject VFX;

    List<PlayerController> l_players = new List<PlayerController>();

    public GameObject Boss;

    public GameObject MainCan;
    public GameObject SubCan;

    void Start()
    {
        Walls = transform.GetChild(0).gameObject;
        VFX = transform.GetChild(1).gameObject;
    }

    void StartBoss()
    {
        Destroy(GetComponent<SphereCollider>());

        Walls.SetActive(true);
        VFX.SetActive(true);

        MainCan.SetActive(false);
        SubCan.SetActive(true);

        Boss.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezeAll;
        Boss.GetComponent<Boss>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController Player;
        if (Player = other.GetComponent<PlayerController>())
            AddPlayer(Player);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController Player;
        if (Player = other.GetComponent<PlayerController>())
            l_players.Remove(Player);
    }

    void AddPlayer(PlayerController Player)
    {
        int CountPlayer = 0;

        if (GameController.Singleton.P1.ToVivo)
        {
            CountPlayer++;
        }

        if (GameController.Singleton.P2.ToVivo)
        {
            CountPlayer++;
        }

        l_players.Add(Player);

        if (l_players.Count == CountPlayer)
        {
            StartBoss();
        }
    }
}
