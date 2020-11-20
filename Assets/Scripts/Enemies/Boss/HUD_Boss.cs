using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Boss : MonoBehaviour
{
    public Image Vida1;
    public Image Vida2;
    public Image Vida3;

    private Image ActualVida;
    private float LifeBar;


    public void SetNewLife(int Count, float LifeBar)
    {
        this.LifeBar = LifeBar;

        switch (Count)
        {
            case 1:
                ActualVida = Vida1;
                break;
            case 2:
                Vida1.transform.parent.gameObject.SetActive(false);
                Vida2.transform.parent.gameObject.SetActive(true);
                ActualVida = Vida2;
                break;
            case 3:
                Vida2.transform.parent.gameObject.SetActive(false);
                Vida3.transform.parent.gameObject.SetActive(true);
                ActualVida = Vida3;
                break;
        }

        StartCoroutine(Life());
    }


    IEnumerator Life()
    {
        while (ActualVida.fillAmount < 1)
        {
            ActualVida.fillAmount += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return null;
    }

    public void SetLife(float actualLife)
    {
        StopCoroutine(Life());
        ActualVida.fillAmount = Mathf.Clamp(actualLife / LifeBar, 0, 1);
    }

    public void BackMenu()
    {
        GameController.Singleton.LoadScene("1_Menu");
    }
}
