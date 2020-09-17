using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Skill
{
    public GameObject Escudinho;
    private GameObject InstShield;

    public int Duration = 5;

    protected override void Effect()
    {
        Vector3 n = Player.transform.position + Escudinho.transform.position;
        //InstShield = Instantiate(Escudinho, n, Player.transform.localRotation);
        InstShield = Instantiate(Escudinho, Player.transform);
        InstShield.transform.parent = null;

        Collider[] Inimigos = Physics.OverlapSphere(InstShield.transform.position, 5);

        foreach (Collider item in Inimigos)
        {
            if (item.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                double t = Vector3.Distance(item.transform.position, InstShield.transform.position);

                if (t < 5)
                {
                    Rigidbody rb = item.GetComponent<Rigidbody>();

                    if (rb)
                    {
                        Vector3 Dir = (item.transform.position - InstShield.transform.position).normalized;
                        Dir.y = 0;
                        rb.AddForce(Dir * 5, ForceMode.Impulse);
                    }
                }
            }
        }

    }

    public override void Play()
    {
        Avaliable = false;
        HUD.WaitCD();
        Effect();
    }

    public override void CountCD()
    {
        DestroyShield();
        StartCoroutine("Count_CD");
    }

    public void DestroyShield()
    {
        Destroy(InstShield);
    }

    IEnumerator Count_CD()
    {
        float ReturnTime = Time.time + CD;
        float currentTime = 0;

        while (ReturnTime >= Time.time)
        {
            currentTime = ReturnTime - Time.time;
            HUD.setCD(currentTime, CD);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        HUD.setCD(0);
        Avaliable = true;
        yield return null;
    }

}
