using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Phase3 : BossPhase
{
    public Boss_Phase3()
    {
        speed = 1.5f;
        AttackCD = 1;
        Skill1CD = 1;
        Skill2CD = 1;

        AttackDamage = 50;
        Skill1Damage = 60;
        Skill2Damage = 60;

        HP = 200;
        PercentToChangePhase = 0;
        DurationChangePhase = 0;

        TimeBetweenThunders = 1;

        MaxRaioCD = 1;
        MinRaioCD = 3;

        ThunderDamage = 25;

        AninSpeed = 1.2f;
    }
}
