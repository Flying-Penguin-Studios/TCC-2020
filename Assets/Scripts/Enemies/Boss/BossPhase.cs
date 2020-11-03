using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class BossPhase
{
    public float speed;

    [Space(20)]

    public float AttackCD;
    public float Skill1CD;
    public float Skill2CD;

    [Space(20)]

    public int AttackDamage;
    public int Skill1Damage;
    public int Skill2Damage;

    [Space(20)]

    public float PercentToChangePhase;
    public float DurationChangePhase;

    [Space(20)]

    public float TimeBetweenThunders;
    public int ThunderDamage;

    [Space(20)]
   
    public float MinRaioCD;
    public float MaxRaioCD;

    [Space(20)]

    public float AninSpeed;
}
