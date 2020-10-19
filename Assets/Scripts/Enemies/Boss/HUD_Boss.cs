using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Boss : MonoBehaviour
{
    public Slider SliderTemp;
    public Text TextTemp;

    private int MaxLife;

    public void SetLife(int MaxLife)
    {
        this.MaxLife = MaxLife;
        Life(MaxLife);
    }

    public void Life(float Value)
    {
        SliderTemp.value = Value / MaxLife;
        TextTemp.text = Value + "/" + MaxLife;
    }
}
