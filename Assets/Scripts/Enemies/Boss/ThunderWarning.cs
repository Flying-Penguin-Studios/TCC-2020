using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWarning : MonoBehaviour
{
    [SerializeField] float TimeWarning;
    //[SerializeField] float minTimeWarning;
    //[SerializeField] float maxTimeWarning;

    public GameObject Thunder;

    void Start()
    {
        Invoke("SummonThunder", TimeWarning);
    }

    void SummonThunder()
    {
        Instantiate(Thunder, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
