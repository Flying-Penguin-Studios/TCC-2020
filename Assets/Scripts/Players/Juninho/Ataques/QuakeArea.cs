using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeArea : PlayerHit
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

    protected override void DamageInteraction(GameObject n_gameObject)
    {
        Rigidbody rb = n_gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.back * 5, ForceMode.Impulse);
    }

    IEnumerator Expand()
    {
        Vector3 sr = transform.localScale * MaxLenth;

        while (transform.localScale.x < sr.x)
        {
            Vector3 s = Vector3.one;
            s.y = 0;
            transform.localScale += s * ScaleSpeed;
            yield return null;
        }

        Player.GetComponent<QuakePunch>().CountCD();
        Destroy(gameObject);
        yield return null;
    }
}
