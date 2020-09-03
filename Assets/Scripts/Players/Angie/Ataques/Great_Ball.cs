using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Great_Ball : Energy_Ball
{
    int tickTimes = 5;
    float DamageReducer;

    protected override void Start()
    {
        base.Start();
        DamageReducer = dano * 0.1f;
    }

    protected override void DamageInteraction()
    {
        if (tickTimes > 0)
        {
            dano -= DamageReducer;
            tickTimes--;
        }
        
        //print(dano + " - Da Esfera grande");
    }
}
