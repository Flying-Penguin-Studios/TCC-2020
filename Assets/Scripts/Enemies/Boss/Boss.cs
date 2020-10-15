using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public abstract class Boss : EnemyController
{
    [SerializeField] protected float speed;

    [Space(20)]

    [SerializeField] protected float AttackCD;
    [SerializeField] protected float Skill1CD;
    [SerializeField] protected float Skill2CD;

    [Space(20)]

    [SerializeField] protected float AttackDamage;
    [SerializeField] protected float Skill1Damage;
    [SerializeField] protected float Skill2Damage;

    [Space(20)]

    [SerializeField] protected float LifeToNextPhase;

    [Space(20)]

    [SerializeField] protected float TimeBetweenThunders;
    [SerializeField] protected float ThunderDamage;

    float PercentToChangeTarget;

    protected GameObject Target;
    protected Juninho Juninho;
    protected Angie Angie;


    //protected abstract void StartPhaseX();

    void Start()
    {
        Init(speed);
        Juninho = FindObjectOfType<Juninho>();
        Angie = FindObjectOfType<Angie>();
    }

    void Update()
    {
        Seek();
    }

    void Seek()
    {

    }
}
