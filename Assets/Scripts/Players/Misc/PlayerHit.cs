using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : HitObject
{
    public PlayerController Player;
    protected EnemyController Target;

    public void SetPlayer(PlayerController n_Player)
    {
        Player = n_Player;
    }

    protected override void EnterDamage(GameObject n_gameObject)
    {
        CalcDamage(n_gameObject);
        DamageInteraction();
        DamageInteraction(n_gameObject);
    }

    protected override void DamageInteraction()
    {
        return;
    }

    protected virtual void DamageInteraction(GameObject n_gameObject)
    {
        return;
    }

    protected virtual void CalcDamage(GameObject n_gameObject)
    {
        Target = n_gameObject.GetComponent<EnemyController>();
        try
        {
            //Target.TakeDamage(RoundDamage(dano),Player.Name);              
        }
        catch (System.Exception e)
        {
            if (Target is null)
            {
                Debug.Log("Enemy Controller nao encontrado no inimigo : " + n_gameObject.name);
            }
            else
            {
                Debug.Log(e.Message);
                throw;
            }
        }
    }

    private int RoundDamage(float Dano)
    {
        return Mathf.RoundToInt(Dano);
    }
}
