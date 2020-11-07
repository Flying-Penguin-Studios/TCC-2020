using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldWall : MonoBehaviour
{
    IEnumerator Wall()
    {
        SphereCollider Sphere = GetComponent<SphereCollider>();

        while (Sphere.radius < 3)
        {
            Sphere.radius += Time.deltaTime * 5;
            yield return null;
        }

        yield return null;

    }
    private void Start()
    {
        StartCoroutine(Wall());
    }
}
