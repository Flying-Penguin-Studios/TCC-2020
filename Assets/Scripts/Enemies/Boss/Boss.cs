using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Boss : EnemyController
{
    //protected HUD_Player HUD;   

    [SerializeField] protected GameObject Target;
    protected PlayerController Player1;
    protected PlayerController Player2;

    [Space(20)]

    [SerializeField] protected HitBoss HitArea;
    [SerializeField] protected HUD_Boss HUD;

    [Space(20)]

    [SerializeField] float AggroP1;
    [SerializeField] float AggroP2;

    float PercentToChangeTarget;

    bool CanAttack = false;
    bool CanSkill1 = false;
    bool CanSkill2 = false;

    public GameObject Raio;

    public BossPhase Phase;

    Vector3 StartPosition;
    Quaternion StartRot;
    Quaternion StartLocRot;


    void Start()
    {
        Phase = new Boss_Phase1();
        StartPosition = transform.position;
        StartRot = transform.rotation;
        StartLocRot = transform.localRotation;

        Init(Phase.speed);
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
            int d = enemy.HP - (int)(enemy.HP * (Phase.PercentToChangePhase / 100));
            TakeDamage(d, "Player1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GenerateAggro(Player1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GenerateAggro(Player2);
        }

        if (!(anim.GetBool("OnAttack") || anim.GetBool("OnChange")))
            TakeDecision();
    }

    float DistanceTarget = 0;
    float TimePassed;
    float TimeDistance;

    bool Moving = false;

    void TakeDecision()
    {
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
        else if (DistanceTarget < 1 && !CanAttack && !Moving)
        {
            StartCoroutine(Wait());
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

    public void UnlockAttack()
    {
        anim.SetBool("CanAttack", true);
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
            moveDir *= Phase.speed;
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

        //if (TimeDistance + 5 < Time.time)
        //{
        //    Dash();
        //}
        //else
        //{
        //    StartCoroutine(Seek());
        //}

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
            Vector3 desired_vel = changeDir.normalized * Phase.speed;
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
        //StopCoroutine(Wander());
        //StopCoroutine(Wait());
        //StopCoroutine(Seek());

        StopAllCoroutines();

        Moving = false;
        rb.velocity = Vector3.zero;
        anim.SetFloat("Speed", 0);
    }

    int AttackCount = 1;

    void NormalAttack()
    {
        anim.SetBool("OnAttack", true);
        CancelWanderWait();
        HitArea.setNewDamage(Phase.AttackDamage);
        transform.LookAt(new Vector3(Target.transform.position.x, 0, Target.transform.position.z));
        anim.SetTrigger("Attack");
        //StartCoroutine(CountAttack());

        if (AttackCount != 3)
            AttackCount++;
        else
        {
            AttackCount = 0;
            Target = GetNewTarget();
            TimeDistance = Time.time;
            return;
        }

        //DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);
        //if (DistanceTarget < 2)
        //{
        //    Invoke("NormalAttack", 0.1f);
        //    TimeDistance = Time.time;
        //    return;
        //}

        Target = GetNewTarget();
        TimeDistance = Time.time;
    }

    IEnumerator CountAttack()
    {
        CanAttack = false;
        yield return new WaitForSeconds(Phase.AttackCD);
        CanAttack = true;
        yield return null;
    }

    void Dash()
    {
        CancelWanderWait();
        CanAttack = false;
        HitArea.setNewDamage(Phase.Skill1Damage);
        anim.SetBool("OnAttack", true);
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

        yield return new WaitForSeconds(Phase.AttackCD);

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

        if (enemy.HP <= 0)
        {
            rb.velocity = Vector3.zero;
            anim.Play("IdleChangePhase");
            Destroy(this);
        }

        GenerateAggro(player);

        if (enemy.HP <= HP_Max * (Phase.PercentToChangePhase / 100))
        {
            CancelWanderWait();
            StartCoroutine(ChangePhase());
            //StartCoroutine(Phase3Chaos());
        }
    }

    int PhaseCount = 1;

    IEnumerator ChangePhase()
    {
        //CancelWanderWait();

        anim.SetTrigger("Change");

        Moving = true;
        CanAttack = false;

        rb.isKinematic = true;
        rb.useGravity = false;

        Vector3 Pos = StartPosition;
        Pos.y += 5;
        transform.position = Pos;

        while (transform.rotation != StartRot)
        {
            transform.rotation = StartRot;
            yield return new WaitForFixedUpdate();
        }

        //transform.localRotation = StartLocRot;
        //transform.rotation = StartRot;       

        yield return new WaitForSeconds(.3f);

        //float Duration = Time.time + Phase.DurationChangePhase;
        float Duration = Time.time + 5;

        while (Duration > Time.time)
        {
            GameObject raio1 = Instantiate(Raio, Player1.transform.position, Quaternion.identity);
            GameObject raio2 = Instantiate(Raio, Player2.transform.position, Quaternion.identity);

            raio1.GetComponent<ThunderWarning>().SetThunderDamage(Phase.ThunderDamage);
            raio2.GetComponent<ThunderWarning>().SetThunderDamage(Phase.ThunderDamage);

            yield return new WaitForSeconds(Random.Range(Phase.MinRaioCD, Phase.MaxRaioCD));
        }

        while (transform.position != StartPosition)
        {
            transform.position = Vector3.Lerp(transform.position, StartPosition, Time.deltaTime * 10);
            yield return new WaitForFixedUpdate();
        }

        anim.SetTrigger("ResetChange");
        rb.isKinematic = false;
        rb.useGravity = true;

        PhaseCount++;

        if (PhaseCount == 2)
        {
            Phase = new Boss_Phase2();
        }
        else if (PhaseCount == 3)
        {
            Phase = new Boss_Phase3();
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            anim = this.transform.GetChild(1).GetComponent<Animator>();

            StartCoroutine(Phase3Chaos());
        }

        TimeDistance = Time.time + 5;

        Moving = false;
        CanAttack = true;

        yield return null;
    }

    IEnumerator Phase3Chaos()
    {
       yield return new WaitForSeconds(3f);

        while (true)
        {
            GameObject raio1 = Instantiate(Raio, Player1.transform.position, Quaternion.identity);
            GameObject raio2 = Instantiate(Raio, Player2.transform.position, Quaternion.identity);

            raio1.GetComponent<ThunderWarning>().SetThunderDamage(Phase.ThunderDamage);
            raio2.GetComponent<ThunderWarning>().SetThunderDamage(Phase.ThunderDamage);

            yield return new WaitForSeconds(Random.Range(Phase.MinRaioCD, Phase.MaxRaioCD));
        }               
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
