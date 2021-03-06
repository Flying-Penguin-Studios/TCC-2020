﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMan : Mob
{



    public GameObject sword;
    public GameObject swordTrail;




    protected override void ChaseTarget()
    {

        LookToTarget();

        if (EnemyHasGroundToMove() && !jumping)
        {

            acceleration = !attacking;
            Accelerate();

        }
        else
        {

            if (EnemyShouldJump() && !jumping && (Time.time >= nextJump))
            {
                Jump();
            }

            if (!jumping)
            {
                acceleration = false;
                Accelerate();
            }

        }


    }


    protected override void Combat()
    {
        patrulheiro = false;
        bool Shield = false;
        Target = SetTarget();

        RaycastHit r_Shield;

        if (Physics.Raycast(transform.position, transform.forward, out r_Shield, 1.5f))
        {
            if (r_Shield.transform.CompareTag("Shield"))
            {
                Shield = true;
            }
        }

        if ((DistanceToTarget() <= minDistanceToPlayer && !attacking && !inStag && !jumping) || Shield)
        {
            acceleration = false;
            Attack();
        }
        else if (inStag)
        {
            FreezeConstraints(false);

        }
        else
        {
            ChaseTarget();
        }

    }


    protected override void Attack()
    {
        StartCoroutine(WaitBeforeAttack());
    }


    IEnumerator WaitBeforeAttack()
    {

        float time = Random.Range(0, 0.5f);

        attacking = true;

        yield return new WaitForSeconds(time);
        sword.GetComponent<BoxCollider>().enabled = true;
        anim.SetTrigger("Attack");

        GameObject trail = Instantiate(swordTrail, sword.transform.position, sword.transform.rotation);
        trail.transform.SetParent(sword.transform);
        trail.GetComponent<ParticleSystem>().Play();
        Destroy(trail, 2);

        yield return null;

    }



    public override void TakeDamage(int damage, string player)
    {

        if (!isVulnerable)
        {
            return;
        }

        if (damage > 0) { Stag(); }


        base.TakeDamage(damage, player);
    }


    private void Stag()
    {

        if ((Time.time >= nextStag))
        {
            inStag = true;
            anim.SetTrigger("Stag");
            FreezeConstraints(false);
            rb.velocity = Vector3.zero;
            rb.AddForce(-transform.forward * 1.5f, ForceMode.Impulse);
            nextStag = Time.time + stagRate;
        }
    }


    public void KnockBack(Vector3 punchPosition)
    {

        inStag = true;
        anim.SetTrigger("Stag");
        FreezeConstraints(false);


        Vector3 direction = (transform.position - punchPosition).normalized;
        Vector3 impulse = direction * 5 + (Vector3.up * 6);

        rb.velocity = impulse;
    }

}
