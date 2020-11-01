using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Phase1 : BossPhase
{
    public Boss_Phase1()
    {
        speed = 2.5f;
        AttackCD = 5;
        Skill1CD = 5;
        Skill2CD = 5;

        AttackDamage = 20;
        Skill1Damage = 30;
        Skill2Damage = 30;

        PercentToChangePhase = 70;
        DurationChangePhase = 20;

        TimeBetweenThunders = 5;

        MaxRaioCD = 5;
        MinRaioCD = 2;

        ThunderDamage = 15;

        AninSpeed = 1;
    }
}
