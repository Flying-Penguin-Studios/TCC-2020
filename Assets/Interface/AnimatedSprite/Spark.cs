using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour
{
    private int cont;
    void Start()
    {
        StartCoroutine("TrocarSprite");
    }

    private IEnumerator TrocarSprite()
    {
        cont = 0;
        while (cont < 15)
        {
            if (cont <= 1)
            {
                this.GetComponent<Renderer>().material.SetFloat("_ColunaDesejada", cont);
                this.GetComponent<Renderer>().material.SetFloat("_LinhaDesejada", 2.0f);

                yield return new WaitForSeconds(0.05f);

                cont++;
            }

            if (cont == 2)
            {
                this.GetComponent<Renderer>().material.SetFloat("_ColunaDesejada", cont);
                this.GetComponent<Renderer>().material.SetFloat("_LinhaDesejada", 2.0f);

                yield return new WaitForSeconds(0.03f);

                cont++;
            }

            if ((cont > 2) && (cont <= 5))
            {
                this.GetComponent<Renderer>().material.SetFloat("_ColunaDesejada", cont - 3);
                this.GetComponent<Renderer>().material.SetFloat("_LinhaDesejada", 1.0f);

                yield return new WaitForSeconds(0.02f);

                cont++;
            }

            if ((cont > 5) && (cont <= 8))
            {
                this.GetComponent<Renderer>().material.SetFloat("_ColunaDesejada", cont - 6);
                this.GetComponent<Renderer>().material.SetFloat("_LinhaDesejada", 0.0f);

                yield return new WaitForSeconds(0.02f);

                cont++;
            }

            if (cont == 9)
            {
                yield return new WaitForSeconds(0.01f);

                this.GetComponent<Renderer>().material.SetFloat("_ColunaDesejada", 3.0f);
                this.GetComponent<Renderer>().material.SetFloat("_LinhaDesejada", 2.0f);

                yield return new WaitForSeconds(0.05f);

                cont++;
            }

            if (cont == 10)
            {
                this.GetComponent<Renderer>().material.SetFloat("_ColunaDesejada", 3.0f);
                this.GetComponent<Renderer>().material.SetFloat("_LinhaDesejada", 1.0f);

                yield return new WaitForSeconds(0.07f);

                cont++;
            }

            if (cont >= 11)
            {
                this.GetComponent<Renderer>().material.SetFloat("_ColunaDesejada", 3.0f);
                this.GetComponent<Renderer>().material.SetFloat("_LinhaDesejada", 0.0f);

                yield return new WaitForSeconds(0.04f);

                cont++;
            }
        }

        yield return new WaitForSeconds(5f);
        Destroy(gameObject);

    }
}
