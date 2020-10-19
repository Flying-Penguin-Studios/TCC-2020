using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public abstract class Boss : EnemyController
{
    //protected HUD_Player HUD;

    [SerializeField] protected float speed;

    [Space(20)]

    [SerializeField] protected float AttackCD;
    [SerializeField] protected float Skill1CD;
    [SerializeField] protected float Skill2CD;

    [Space(20)]

    [SerializeField] protected int AttackDamage;
    [SerializeField] protected int Skill1Damage;
    [SerializeField] protected int Skill2Damage;

    [Space(20)]

    [SerializeField] protected float LifeToNextPhase;

    [Space(20)]

    [SerializeField] protected float TimeBetweenThunders;
    [SerializeField] protected float ThunderDamage;

    float PercentToChangeTarget;

    protected GameObject Target;
    protected PlayerController Player1;
    protected PlayerController Player2;

    [SerializeField] protected HitBoss HitArea;
    [SerializeField] protected HUD_Boss HUD;

    float AggroP1;
    float AggroP2;

    //protected abstract void StartPhaseX();

    bool CanAttack = true;

    void Start()
    {
        Init(speed);
        rb.angularDrag = 999;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Player1 = GameController.Singleton.ScenePlayer1.GetComponent<PlayerController>();
        Player2 = GameController.Singleton.ScenePlayer2.GetComponent<PlayerController>();
        Target = SortTarget();

        HUD.SetLife(enemy.HP);

        TimeDistance = Time.time;
    }

    GameObject SortTarget()
    {
        PlayerController[] Players = { Player1, Player2 };
        return Players[Random.Range(0, 2)].gameObject;
    }

    public void WalkOnAttack()
    {
        rb.drag = 0;
        Vector3 Dir = transform.forward.normalized * 10;
        rb.AddForce(Dir, ForceMode.Impulse);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            NormalAttack();
        }

        if (!anim.GetBool("OnAttack"))
        {
            Seek();
        }
    }

    float DistanceTarget = 0;
    float TimePassed;
    float TimeDistance;

    void Seek()
    {
        if (Target == null)
        {
            Wait();
        }

        DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceTarget < 1)
        {
            if (CanAttack)
            {
                NormalAttack();
            }
            else
            {
                Wait();
            }
        }
        else
        {
            TimePassed = Time.time - TimeDistance;
            if (TimePassed > 10)
            {
                //Ataca o alvo com o dash
                TimeDistance = Time.time;
                return;
            }

            Vector3 moveDir = Vector3.Normalize(Target.transform.position - transform.position);
            moveDir *= speed;
            anim.SetFloat("Speed", 1);
            rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
            transform.LookAt(new Vector3(Target.transform.position.x, 0, Target.transform.position.z));
        }
    }

    void Wait()
    {
        rb.velocity = Vector3.zero;
        anim.SetFloat("Speed", 0);
        return;
    }

    void NormalAttack()
    {
        HitArea.setNewDamage(AttackDamage);
        rb.velocity = Vector3.zero;
        transform.LookAt(new Vector3(Target.transform.position.x, 0, Target.transform.position.z));
        anim.SetTrigger("Attack");
        StartCoroutine(CountAttack());
    }

    IEnumerator CountAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(AttackCD);
        CanAttack = true;
        yield return null;
    }

    public override void TakeDamage(int damage, string player)
    {    
        enemy.HP -= damage;
        HUD.Life(enemy.HP);
    }
}
