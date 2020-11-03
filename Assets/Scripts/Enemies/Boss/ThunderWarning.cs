using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWarning : MonoBehaviour
{
    [SerializeField] float TimeWarning;
    //[SerializeField] float minTimeWarning;
    //[SerializeField] float maxTimeWarning;

    public GameObject Thunder;

    int ThunderDamage;

    void Start()
    {
        Invoke("SummonThunder", TimeWarning);
    }

    public void SetThunderDamage(int D)
    {
        ThunderDamage = D;
    }

    void SummonThunder()
    {
        GameObject T = Instantiate(Thunder, transform.position, Quaternion.identity);
        T.GetComponent<Thunder>().SetDamage(ThunderDamage);
        Destroy(gameObject);
    }
}
