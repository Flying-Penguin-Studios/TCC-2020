using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Boss : EnemyController
{
    //protected HUD_Player HUD;   

    [SerializeField] protected GameObject Target;
    [SerializeField] protected PlayerController Player1;
    [SerializeField] protected PlayerController Player2;

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
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (GameController.Singleton)
        {
            Player1 = GameController.Singleton.ScenePlayer1.GetComponent<PlayerController>();
            Player2 = GameController.Singleton.ScenePlayer2.GetComponent<PlayerController>();
        }
        else
        {
            //Player1 = FindObjectOfType<Juninho>();
            //Player2 = FindObjectOfType<Angie>();
        }

        Target = SortTarget();
        StartCoroutine(AggroControl());

        HUD.SetLife(enemy.HP);

        Invoke("UnlockAttacks", Random.Range(1f, 3f));

        TimeDistance = Time.time + 10;

        StartCoroutine(TakeDecision());
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
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    StartCoroutine(NormalAttack());
        //}

        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    StartCoroutine(Dash());
        //}

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
    }

    public void SetBool(string Name, bool Value)
    {
        anim.SetBool(Name, Value);
    }

    bool GetBool(string Name)
    {
        return anim.GetBool(Name);
    }

    IEnumerator TakeDecision()
    {
        yield return new WaitForSeconds(.1f);

        while (enemy.HP >= 0)
        {
            if (Target == null)
            {
                Target = GetNewTarget();
            }

            if (Input.GetKey(KeyCode.K))
            {
                StartCoroutine(NormalAttack());
            }

            if (Input.GetKey(KeyCode.H))
            {
                yield return StartCoroutine(Dash());
            }

            DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);

            if (DistanceTarget > 15)
            {
                yield return StartCoroutine(Dash());
            }
            else if (DistanceTarget < 2.5f)
            {
                yield return StartCoroutine(NormalAttack());
            }
            else
            {
                if (Random.Range(0f, 100f) <= 10f)
                {
                    //yield return StartCoroutine(Wait());
                    yield return StartCoroutine(Seek());
                }
                //else if (Random.Range(0f, 100f) <= 30f)
                //{
                //    print("Vagando");
                //    StartCoroutine(Wander());
                //}
                else
                {
                    yield return StartCoroutine(Seek());
                }
            }
        }

        yield return null;
    }

    float DistanceTarget = 0;
    float TimePassed;
    float TimeDistance;

    bool Moving = false;

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
        float Duration = Time.time + Random.Range(5f, 15f);

        while (Time.time < Duration)
        {
            DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);

            if (DistanceTarget < 2.5f)
            {
                yield return StartCoroutine(NormalAttack());
            }

            Vector3 moveDir = Vector3.Normalize(Target.transform.position - transform.position);
            moveDir *= Phase.speed;
            anim.SetFloat("Speed", 1);
            rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
            LookToTarget();

            if (TimeDistance < Time.time || DistanceTarget > 12)
            {
                yield return StartCoroutine(Dash());
            }

            yield return new WaitForEndOfFrame();
        }

        Target = GetNewTarget();
        Moving = false;

        yield return null;
    }

    IEnumerator NormalAttack()
    {
        while (AttackCount <= 3)
        {
            Attack();
            anim.SetFloat("Speed", 0);
            anim.SetBool("CanAttack", false);

            yield return new WaitUntil(() => GetBool("CanAttack"));

            DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);

            if (DistanceTarget < 2.5f)
            {
                continue;
            }
            else
            {
                float RandAttack = Random.Range(0f, 100f);
                if (RandAttack >= 50)
                {
                    continue;
                }
                else
                {
                    anim.SetTrigger("ResetAttack");
                    break;
                }
            }
        }

        anim.SetTrigger("ResetAttack");
        AttackCount = 0;
        Target = GetNewTarget();
        TimeDistance = Time.time + 10;
        yield return null;
    }

    public IEnumerator Dash()
    {
        anim.SetFloat("Speed", 0);
        rb.velocity = Vector3.zero;
        HitArea.setNewDamage(Phase.Skill1Damage);
        LookToTarget();
        anim.SetTrigger("Dash");
        anim.SetBool("DashPreparation", true);

        Vector3 Destino = Target.transform.position;
        Destino.y = transform.position.y;
        Vector3 moveDir = Vector3.Normalize(Target.transform.position - transform.position) * 25;

        //print(GetBool("DashPreparation"));  

        yield return new WaitWhile(() => GetBool("DashPreparation"));

        //print(GetBool("DashPreparation"));

        while (Vector3.Distance(Destino, transform.position) > 1)
        {
            rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
            yield return null;
        }

        rb.velocity = Vector3.zero;
        anim.SetTrigger("Attack");

        yield return new WaitWhile(() => GetBool("OnAttack"));

        Target = GetNewTarget();
        LookToTarget();

        DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);
        TimeDistance = Time.time + 10;

        if (DistanceTarget < 2.5f)
        {
            yield return StartCoroutine(NormalAttack());
        }

        yield return new WaitForSeconds(.5f);

        yield return null;
    }


    IEnumerator Wait()
    {
        Moving = true;
        rb.velocity = Vector3.zero;
        anim.SetFloat("Speed", 0);

        yield return new WaitForSeconds(Random.Range(1f, 2f));

        Target = GetNewTarget();

        yield return null;
    }

    int AttackCount = 1;

    void LookToTarget()
    {
        transform.LookAt(new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z));
    }

    void Attack()
    {
        rb.velocity = Vector3.zero;
        HitArea.setNewDamage(Phase.AttackDamage);
        LookToTarget();
        anim.SetTrigger("Attack");
        AttackCount++;
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
            StopAllCoroutines();
            StartCoroutine(ChangePhase());
            //StartCoroutine(Phase3Chaos());
        }
    }

    int PhaseCount = 1;

    IEnumerator ChangePhase()
    {
        rb.velocity = Vector3.zero;

        anim.SetTrigger("Change");

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

        yield return new WaitUntil(() => GetBool("OnChange"));

        float Duration = Time.time + Phase.DurationChangePhase;
        //float Duration = Time.time + 5;

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

        yield return new WaitWhile(() => GetBool("OnChange"));

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

        StartCoroutine(TakeDecision());

        yield return null;
    }

    IEnumerator Phase3Chaos()
    {
        yield return new WaitForSeconds(3f);

        while (enemy.HP > 0)
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
