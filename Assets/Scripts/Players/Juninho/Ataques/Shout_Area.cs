using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shout_Area : PlayerHit
{
    [SerializeField]
    private float ScaleSpeed = 4;

    [SerializeField]
    private float MaxLenth = 5;

    void Start()
    {
        //Player = FindObjectOfType<Angie>();
        StartCoroutine("Expand");
    }

    IEnumerator Expand()
    {
        Vector3 sr = transform.localScale * MaxLenth;

        while (transform.localScale.x < sr.x)
        {
            transform.localScale += Vector3.one * ScaleSpeed;
            yield return null;
        }

        Player.GetComponent<Shout>().CountCD();
        Destroy(gameObject);
        yield return null;
    }

    protected override void DamageInteraction(GameObject n_gameObject)
    {
        Target = n_gameObject.GetComponent<EnemyController_Old>();
        Target.ShoutThreat();
    }
}
