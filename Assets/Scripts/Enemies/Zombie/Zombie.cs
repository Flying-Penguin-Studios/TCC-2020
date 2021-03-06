﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Mob
{


    [Tooltip("Porcentagem (%) de vida para entrar em Berserker Mode"), Header("Zombie"), Space(10)]
    public float LifeToBerserkerMode;
    public bool berserkerModeOn = false;
    public bool inTransformation = false;

    public GameObject LHand;
    public GameObject RHand;

    private int attackType;




    protected override void Combat()
    {

        Target = SetTarget();

        if (DistanceToTarget() <= minDistanceToPlayer && !attacking)
        {
            acceleration = false;
            Attack();
        }
        else if (inTransformation)
        {
            FreezeConstraints(true);
        }
        else
        {
            ChaseTarget();
        }
    }




    protected override void Attack()
    {
        attacking = true;
        attackType = SortAttack();

        if (attackType == 1)
        {
            RHand.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            LHand.GetComponent<BoxCollider>().enabled = true;
        }

        anim.SetTrigger("Attack");
        anim.SetInteger("AttackType", attackType);
    }





    private int SortAttack()
    {
        return Random.Range(1, 3);
    }







    private void EnterBerserkerMode()
    {
        inTransformation = true;
        StartCoroutine(GetBigger());
        isVulnerable = false;
        berserkerModeOn = true;
        anim.SetFloat("BerserkerModeOn", 1);
        anim.SetTrigger("ActivateBerserker");
        anim.SetFloat("AttackSpeedMultiplier", 1.5f);
        combatSpeed = 3;
    }





    public override void TakeDamage(int damage, string player)
    {

        //Executa as regras da classe pai.
        base.TakeDamage(damage, player);

        if (enemy.HP <= (enemy.HP_Max * (LifeToBerserkerMode / 100)) && !berserkerModeOn)
        {
            EnterBerserkerMode();
        }
    }


    IEnumerator GetBigger()
    {

        for (float i = 1; i <= 1.5f; i = i + 0.15f * Time.fixedDeltaTime)
        {
            transform.localScale = new Vector3(i, i, i);
            yield return null;
        }
    }
    protected override void Accelerate()
    {
        base.Accelerate();

        if (!berserkerModeOn)
        {
            Collider[] Vortex = Physics.OverlapSphere(transform.position, 3, LayerMask.GetMask("Vortex_Zone"));

            if (Vortex.Length > 0)
                rb.velocity /= 2;
        }
    }
}
