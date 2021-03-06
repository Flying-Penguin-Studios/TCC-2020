﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Phase2 : BossPhase
{
    public Boss_Phase2()
    {
        speed = 2.5f;
        AttackCD = 5;
        Skill1CD = 5;
        Skill2CD = 5;

        AttackDamage = 30;
        Skill1Damage = 40;
        Skill2Damage = 40;

        HP = 400;
        PercentToChangePhase = 30;
        DurationChangePhase = 25;

        TimeBetweenThunders = 5;

        MaxRaioCD = 2;
        MinRaioCD = 1;

        ThunderDamage = 20;

        AninSpeed = 1.2f;
    }
}
