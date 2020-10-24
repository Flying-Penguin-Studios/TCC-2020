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
    [SerializeField] protected float DurationChangePhase;

    [Space(20)]

    [SerializeField] protected float TimeBetweenThunders;
    [SerializeField] protected float ThunderDamage;

    float PercentToChangeTarget;

    [SerializeField] protected GameObject Target;
    protected PlayerController Player1;
    protected PlayerController Player2;

    [Space(20)]

    [SerializeField] protected HitBoss HitArea;
    [SerializeField] protected HUD_Boss HUD;

    [Space(20)]
    [SerializeField] protected GameObject Raio;
    [SerializeField] protected float MinRaioCD;
    [SerializeField] protected float MaxRaioCD;



    [Space(20)]

    [SerializeField] float AggroP1;
    [SerializeField] float AggroP2;

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
        StartCoroutine(AggroControl());

        HUD.SetLife(enemy.HP);

        TimeDistance = Time.time;
    }

    GameObject SortTarget()
    {
        PlayerController[] Players = { Player1, Player2 };
        PlayerController P = Players[Random.Range(0, 2)];
        GenerateAggro(P);
        return P.gameObject;
    }

    public void WalkOnAttack()
    {
        rb.drag = 0;
        Vector3 Dir = transform.forward.normalized * 7;
        rb.AddForce(Dir, ForceMode.Impulse);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            NormalAttack();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            enemy.HP -= 10;
            HUD.Life(enemy.HP);
            if (enemy.HP <= LifeToNextPhase)
            {
                StartCoroutine(ChangePhase());
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GenerateAggro(Player1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GenerateAggro(Player2);
        }

        if (!anim.GetBool("OnAttack") || !anim.GetBool("OnChange"))
        {
            TakeDecision();
        }
    }

    float DistanceTarget = 0;
    float TimePassed;
    float TimeDistance;

    bool Waiting = false;

    void TakeDecision()
    {
        if (Target == null)
        {
            Target = GetNewTarget();
        }

        DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);

        if (!CanAttack)
        {
            if (DistanceTarget > 1 && DistanceTarget < 50)
            {
                if (!Waiting)
                {
                    if (Random.Range(0, 1) == 0)
                    {
                        StartCoroutine(Wait());
                    }
                    else
                    {
                        Seek();
                    }
                }
            }
            else
            {
                //Vagar por X segundos
            }

            return;
        }
        else
        {
            Attack();
        }
    }

    void Attack()
    {
        if (DistanceTarget > 50)
        {
            //Dash
        }
        else if (DistanceTarget < 1)
        {
            NormalAttack();
        }
        else
        {
            Seek();
        }
    }

    void Seek()
    {
        Vector3 moveDir = Vector3.Normalize(Target.transform.position - transform.position);
        moveDir *= speed;
        anim.SetFloat("Speed", 1);
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
        transform.LookAt(new Vector3(Target.transform.position.x, 0, Target.transform.position.z));
    }

    GameObject GetNewTarget()
    {
        if (Mathf.Abs(AggroP1 - AggroP2) > 20)//Primeira regra, ela bate quem tiver mais de 20% de aggro que outro player
        {
            return AggroP1 > AggroP2 ? Player1.gameObject : Player2.gameObject;
        }
        else
        {
            float DistanceP1 = Vector3.Distance(transform.position, Player1.transform.position);
            float DistanceP2 = Vector3.Distance(transform.position, Player2.transform.position);

            return DistanceP1 > DistanceP2 ? Player2.gameObject : Player1.gameObject;
        }
    }

    IEnumerator Wait()
    {
        Waiting = true;
        rb.velocity = Vector3.zero;
        anim.SetFloat("Speed", 0);
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        Target = GetNewTarget();
        Waiting = false;
    }

    void NormalAttack()
    {
        HitArea.setNewDamage(AttackDamage);
        rb.velocity = Vector3.zero;
        transform.LookAt(new Vector3(Target.transform.position.x, 0, Target.transform.position.z));
        anim.SetTrigger("Attack");
        StartCoroutine(CountAttack());
        Target = GetNewTarget();
    }

    IEnumerator CountAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(AttackCD);
        CanAttack = true;
        yield return null;
    }

    protected void GenerateAggro(PlayerController P)
    {
        if (P.Equals(Player1))
        {
            AggroP1++;
        }
        else
        {
            AggroP2++;
        }
    }

    protected void GenerateAggro(string P)
    {
        if (P.ToUpper() == Player1.name.ToUpper())
        {
            if (AggroP1 < 100)
                AggroP1++;

            if (AggroP1 == 100)
            {
                Target = Player1.gameObject;
            }
        }
        else
        {
            if (AggroP2 < 100)
                AggroP2++;

            if (AggroP2 == 100)
            {
                Target = Player2.gameObject;
            }
        }
    }

    public override void TakeDamage(int damage, string player)
    {
        if (anim.GetBool("OnChange"))
            return;

        enemy.HP -= damage;
        HUD.Life(enemy.HP);
        GenerateAggro(player);
        if (enemy.HP <= LifeToNextPhase)
        {

            StartCoroutine(ChangePhase());
        }
    }

    IEnumerator ChangePhase()
    {
        anim.SetTrigger("Change");
        rb.isKinematic = true;
        rb.useGravity = false;

        float Duration = Time.time + DurationChangePhase;

        Vector3 Pos = Vector3.zero;
        Pos.y = 5;
        transform.position = Pos;
        transform.localRotation = Quaternion.identity;

        while (Duration > Time.time)
        {
            //int ThunderCount = Random.Range(MinRaioCount, MaxRaioCount);

            //int i = 0;
            //while (i != ThunderCount)
            //{

            Instantiate(Raio, Player1.transform.position, Quaternion.identity);
            Instantiate(Raio, Player2.transform.position, Quaternion.identity);

            //i++;
            //}

            yield return new WaitForSeconds(Random.Range(MinRaioCD, MaxRaioCD));
        }

        rb.isKinematic = false;
        rb.useGravity = true;
        anim.SetBool("OnChange", false);

        transform.position = Vector3.zero;

        yield return null;
    }

    IEnumerator AggroControl()
    {
        yield return new WaitForSeconds(5f);

        while (enemy.HP > 0)
        {
            if (AggroP1 > 0)
                AggroP1--;

            if (AggroP2 > 0)
                AggroP2--;

            yield return new WaitForSeconds(3f);
        }
    }
}
