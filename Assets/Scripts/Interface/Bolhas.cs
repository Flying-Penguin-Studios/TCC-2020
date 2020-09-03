using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bolhas : MonoBehaviour
{
    public Material teste;

    public void Efeito()
    {
        float SpeedX = teste.GetFloat("_SpeedX");
        float SpeedY = teste.GetFloat("_SpeedY");

        teste.SetFloat("_SpeedX", SpeedX * 5);
        teste.SetFloat("_SpeedY", SpeedY * 5);
        teste.SetColor("_Color2", Color.grey/4);

    }
    public void VoltarEfeito()
    {
        float SpeedX = teste.GetFloat("_SpeedX");
        float SpeedY = teste.GetFloat("_SpeedY");
        teste.SetColor("_Color2", Color.black);

        teste.SetFloat("_SpeedX", SpeedX / 5);
        teste.SetFloat("_SpeedY", SpeedY / 5);
    }

}
