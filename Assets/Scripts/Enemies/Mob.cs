using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : EnemyController {


    public float patrolSpeed;
    public float combatSpeed;
    public float aggroRange;




    private void Start() {
        Init(patrolSpeed);
    }





    /// <summary>
    /// Regras ao tomar dano. Ex: Stag, verificação de HP, se morreu, etc...
    /// </summary>
    public virtual void TakeDamage() {
        Die();
    }



    public void Die() {

    }














}
