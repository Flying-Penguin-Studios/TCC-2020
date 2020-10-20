using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Arena : MonoBehaviour
{
    public List<Wave> Waves = new List<Wave>();

    GameObject Walls;
    GameObject VFX;

    List<PlayerController> l_players = new List<PlayerController>();

    [HideInInspector]
    public int EnemyCount = 0;

    void Start()
    {
        Walls = transform.GetChild(0).gameObject;
        VFX = transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            StartArena();
            //Destroy(gameObject, 10f);
        }
    }

    void StartArena()
    {
        StartCoroutine(IeArena());
    }

    IEnumerator IeArena()
    {
        EnemyCount = 0;
        Waves[Waves.Count - 1].Duration = 0;

        Destroy(GetComponent<SphereCollider>());

        Walls.SetActive(true);
        VFX.SetActive(true);

        yield return new WaitForSeconds(1f);

        foreach (Wave _wave in Waves)
        {
            foreach (GameObject Inimigo in _wave.Enemys)
            {
                RaycastHit RayHit;
                if (Physics.Raycast(transform.position, Vector3.forward, out RayHit, 15))
                {
                    //if (RayHit.transform.CompareTag("ArenaWall"))
                    //{
                    //float Area = RayHit.distance;     
                    //Area = float.Parse(Area.ToString("0.00"));

                    //float AreaX = UnityEngine.Random.Range(-Area + 3, Area - 3);
                    //float AreaZ = UnityEngine.Random.Range(-Area + 3, Area - 3);
                    float AreaX = UnityEngine.Random.Range(-11, 11);
                    float AreaZ = UnityEngine.Random.Range(-11, 11);
                    Vector3 Pos = new Vector3(AreaX, transform.position.y, AreaZ);
                    GameObject i_Inimigo = Instantiate(Inimigo, transform.position + Pos, Quaternion.identity, transform);
                    i_Inimigo.transform.localScale *= 0.1f;
                    EnemyCount++;
                    i_Inimigo.AddComponent<ArenaMob>().Arena = this;
                    //}
                }
            }
            yield return new WaitForSeconds(_wave.Duration);
            print("PROoooXIMO!");
        }

        yield return new WaitUntil(() => EnemyCount == 0);
        print("cabo");
        ArenaController.Singleton.ArenaCompleted(gameObject.name);
        Destroy(gameObject);
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
            StartArena();
        }

    }
}

[Serializable]
public class Wave
{
    public int Duration;
    public List<GameObject> Enemys = new List<GameObject>();
}
