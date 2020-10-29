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

    bool CanAttack = false;
    bool CanSkill1 = false;
    bool CanSkill2 = false;

    [Space(20)]

    [SerializeField] protected int AttackDamage;
    [SerializeField] protected int Skill1Damage;
    [SerializeField] protected int Skill2Damage;

    [Space(20)]

    [SerializeField] protected float PercentToChangePhase;
    [SerializeField] protected float DurationChangePhase;
    float LifeToChangePhase;

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

        Invoke("UnlockAttacks", Random.Range(1f, 3f));

        TimeDistance = Time.time + 10;
    }

    void UnlockAttacks()
    {
        CanAttack = true;
        CanSkill1 = true;
        CanSkill2 = true;
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
        Vector3 Dir = transform.forward.normalized * 4;
        rb.AddForce(Dir, ForceMode.Impulse);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            NormalAttack();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Dash();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            enemy.HP -= 100;
            HUD.Life(enemy.HP);
            if (enemy.HP <= PercentToChangePhase)
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

    bool Moving = false;

    void TakeDecision()
    {
        if (anim.GetBool("OnAttack"))
            return;

        if (Target == null)
        {
            Target = GetNewTarget();
        }

        DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceTarget > 20 && CanAttack)
        {
            Dash();
        }
        else if (DistanceTarget < 1 && CanAttack)
        {
            NormalAttack();
        }
        else
        {
            if (!Moving)
            {
                if (Random.Range(0f, 100f) <= 10f)
                {
                    print("Parada");
                    StartCoroutine(Wait());
                }
                //else if (Random.Range(0f, 100f) <= 30f)
                //{
                //    print("Vagando");
                //    StartCoroutine(Wander());
                //}
                else
                {
                    print("Seguindo");
                    StartCoroutine(Seek());
                }
            }
        }
    }

    GameObject GetNewTarget()
    {
        if (!Player1.ToVivo || Player1.Caido)
            return Player2.gameObject;
        else if (!Player2.ToVivo || Player2.Caido)
            return Player1.gameObject;

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


    IEnumerator Seek()
    {
        Moving = true;
        float Duration = Time.time + Random.Range(2f, 5f);

        while (Time.time < Duration)
        {
            Vector3 moveDir = Vector3.Normalize(Target.transform.position - transform.position);
            moveDir *= speed;
            anim.SetFloat("Speed", 1);
            rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
            transform.LookAt(new Vector3(Target.transform.position.x, 0, Target.transform.position.z));

            if (TimeDistance + 5 < Time.time)
            {
                Dash();
            }

            yield return null;
        }

        Target = GetNewTarget();
        Moving = false;

        yield return null;
    }

    IEnumerator Wait()
    {
        Moving = true;
        rb.velocity = Vector3.zero;
        anim.SetFloat("Speed", 0);

        yield return new WaitForSeconds(Random.Range(1f, 2f));

        Target = GetNewTarget();

        if (TimeDistance + 5 < Time.time)
        {
            Dash();
        }
        else
        {
            StartCoroutine(Seek());
        }

        yield return null;
    }

    IEnumerator Wander()
    {
        Moving = true;
        anim.SetFloat("Speed", 1);
        float WanderDuration = Time.time + Random.Range(2f, 5f);
        float angle = Random.Range(-45.0f, 45.0f);

        while (Time.time < WanderDuration)
        {
            Vector3 changeDir = Quaternion.AngleAxis(angle, Vector3.forward) * rb.velocity;
            Vector3 desired_vel = changeDir.normalized * speed;
            rb.velocity = new Vector3(desired_vel.x, rb.velocity.y, desired_vel.z);
            transform.LookAt(new Vector3(changeDir.normalized.x, 0, changeDir.normalized.z));

            if (TimeDistance + 5 < Time.time)
            {
                Dash();
            }

            yield return null;
        }

        Target = GetNewTarget();
        Moving = false;
    }

    void CancelWanderWait()
    {
        StopCoroutine(Wander());
        StopCoroutine(Wait());
        StopCoroutine(Seek());
        Moving = false;
    }

    void NormalAttack()
    {
        anim.SetBool("OnAttack", true);
        CancelWanderWait();
        HitArea.setNewDamage(AttackDamage);
        rb.velocity = Vector3.zero;
        transform.LookAt(new Vector3(Target.transform.position.x, 0, Target.transform.position.z));
        anim.SetTrigger("Attack");
        StartCoroutine(CountAttack());
        Target = GetNewTarget();
        TimeDistance = Time.time;
    }

    IEnumerator CountAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(AttackCD);
        CanAttack = true;
        yield return null;
    }

    void Dash()
    {
        CancelWanderWait();
        CanAttack = false;
        HitArea.setNewDamage(Skill1Damage);
        anim.SetBool("OnAttack", true);
        anim.SetFloat("Speed", 0);
        rb.velocity = Vector3.zero;
        transform.LookAt(new Vector3(Target.transform.position.x, 0, Target.transform.position.z));
        anim.SetTrigger("Dash");
        TimeDistance = Time.time;
    }

    public IEnumerator IDash()
    {
        Vector3 Destino = Target.transform.position;
        Vector3 moveDir = Vector3.Normalize(Target.transform.position - transform.position) * 14;

        while (Vector3.Distance(Destino, transform.position) > 1)
        {
            rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
            yield return null;
        }

        anim.SetTrigger("Attack");
        rb.velocity = Vector3.zero;
        Target = GetNewTarget();
        transform.LookAt(new Vector3(Target.transform.position.x, 0, Target.transform.position.z));

        if (Vector3.Distance(Destino, transform.position) < 1)
        {
            //Combo
        }

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
        if (enemy.HP <= HP_Max * (PercentToChangePhase / 100))
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
