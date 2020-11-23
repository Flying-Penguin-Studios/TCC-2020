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
        HUD.gameObject.SetActive(true);

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

        HUD.SetNewLife(1, Phase.HP);
        enemy.HP = Phase.HP;

        Invoke("UnlockAttacks", Random.Range(1f, 3f));

        TimeDistance = Time.time + 5;
        BattleStart = Time.time + 10;

        StartCoroutine(TakeDecision());
    }

    float BattleStart;

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

        while (enemy.HP > 0)
        {
            if (Target == null)
            {
                Target = GetNewTarget();
            }

            DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);

            if (DistanceTarget > 12 && Time.time > BattleStart)
            {
                yield return StartCoroutine(Dash());
                yield return null;
            }
            else if (DistanceTarget < 2.5f)
            {
                yield return StartCoroutine(NormalAttack());
                yield return null;
            }
            else
            {
                yield return StartCoroutine(Seek());
                yield return null;
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
            HealZone Zone = FindObjectOfType<HealZone>();

            if (Zone && Vector3.Distance(transform.position, Zone.transform.position) > 5)
            {
                Collider[] l_Player = Physics.OverlapSphere(Zone.gameObject.transform.position, Zone.gameObject.GetComponent<SphereCollider>().radius, LayerMask.GetMask("Player"));

                if (l_Player.Length > 0)
                {
                    Target = l_Player[Random.Range(0, l_Player.Length - 1)].gameObject;
                    yield return StartCoroutine(Dash());
                    break;
                }
            }

            RaycastHit Shield;

            if (Physics.Raycast(transform.position, transform.forward, out Shield, 1.5f))
            {
                if (Shield.transform.CompareTag("Shield"))
                {
                    yield return StartCoroutine(NormalAttack());
                    continue;
                }
            }

            DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);

            if (DistanceTarget < 2.5f)
            {
                yield return StartCoroutine(NormalAttack());
                break;
            }

            Vector3 moveDir = Vector3.Normalize(Target.transform.position - transform.position);
            moveDir *= Phase.speed;
            anim.SetFloat("Speed", 1);
            rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
            LookToTarget();

            if (TimeDistance < Time.time || (DistanceTarget > 12 && Time.time > BattleStart))
            {
                yield return StartCoroutine(Dash());
                break;
            }

            anim.SetFloat("Speed", 1);
            Target = GetNewTarget();

            yield return new WaitForEndOfFrame();
        }

        //Target = GetNewTarget();
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

            RaycastHit Shield;

            if (Physics.Raycast(transform.position, transform.forward, out Shield, 1.5f))
            {
                if (Shield.transform.CompareTag("Shield"))
                {
                    yield return StartCoroutine(NormalAttack());
                    continue;
                }
            }

            if (DistanceTarget < 2.5f)
            {
                continue;
            }
            else
            {
                anim.SetTrigger("ResetAttack");
                break;
            }
        }

        anim.SetTrigger("ResetAttack");
        AttackCount = 0;
        Target = GetNewTarget();
        TimeDistance = Time.time + +2.5f;

        yield return new WaitForSeconds(0.2f);

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

        yield return new WaitWhile(() => GetBool("DashPreparation"));

        Vector3 Destino = Target.transform.position;
        Destino.y = transform.position.y;
        Vector3 moveDir = Vector3.Normalize(Target.transform.position - transform.position) * 50;

        while (Vector3.Distance(Destino, transform.position) > 1)
        {
            RaycastHit Shield;

            if (Physics.Raycast(transform.position, transform.forward, out Shield, 1.5f))
            {
                if (Shield.transform.CompareTag("Shield"))
                {
                    break;
                }
            }

            rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
            yield return null;
        }

        rb.velocity = Vector3.zero;
        anim.SetTrigger("Attack");

        yield return new WaitWhile(() => GetBool("OnAttack"));
        yield return new WaitForSeconds(.2f);

        Target = GetNewTarget();
        LookToTarget();

        DistanceTarget = Vector3.Distance(transform.position, Target.transform.position);
        TimeDistance = Time.time + 2.5f;

        if (DistanceTarget < 2.5f)
        {
            yield return StartCoroutine(NormalAttack());
        }

        yield return new WaitForSeconds(.5f);

        yield return null;
    }

    bool Waiting;
    bool _TakeDamage = false;

    IEnumerator Wait()
    {
        Waiting = true;

        float duration = Time.time + Random.Range(3f, 5f);

        while (Time.time < duration)
        {
            if (DistanceTarget < 2.5f)
            {
                yield return StartCoroutine(NormalAttack());
                break;
            }

            if (_TakeDamage)
            {
                yield return StartCoroutine(Dash());
                _TakeDamage = false;
                break;
            }

            rb.velocity = Vector3.zero;
            anim.SetFloat("Speed", 0);
            Target = GetNewTarget();
            LookToTarget();

            yield return null;
        }

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
        HUD.SetLife(enemy.HP);

        if (enemy.HP <= 0)
        {
            if (PhaseCount == 3)
            {
                StopAllCoroutines();
                rb.velocity = Vector3.zero;
                anim.SetTrigger("Morre");
                AuraVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();

                Destroy(GameObject.Find("ArenaBoss"));
                Invoke("Win", 5f);
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ChangePhase());
                return;
            }

        }

        if (Waiting)
        {
            _TakeDamage = true;
            Waiting = false;
            Target = player.Contains("1") ? Player1.gameObject : Player2.gameObject;
        }

        GenerateAggro(player);
    }

    void Win()
    {
        HUD.Win();
        Destroy(this);
    }

    public int PhaseCount = 1;
    public GameObject AlabardaVFX;
    public GameObject AuraVFX;
    public GameObject PhaseVFX;

    IEnumerator ChangePhase()
    {
        rb.velocity = Vector3.zero;
        anim.SetFloat("Speed", 0);
        anim.SetBool("OnChange", true);
        anim.ResetTrigger("Attack");

        anim.SetTrigger("Change");

        rb.isKinematic = true;
        rb.useGravity = false;

        Vector3 Pos = new Vector3(1.8f, 3.8f, 2.1f);
        float old_y = transform.position.y;

        while (Vector3.Distance(transform.position, Pos) > .5f)
        {
            transform.position = Vector3.Lerp(transform.position, Pos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, StartRot, Time.deltaTime * 10);

            yield return new WaitForFixedUpdate();
        }

        AuraVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        //while (transform.rotation != StartRot)
        //{
        //    yield return new WaitForFixedUpdate();
        //}

        yield return new WaitWhile(() => GetBool("OnChange"));

        float Duration = Time.time + Phase.DurationChangePhase;
        //float Duration = Time.time + 5;

        while (Duration > Time.time)
        {
            if (Player1.ToVivo)
            {
                GameObject raio1 = Instantiate(Raio, Player1.transform.position, Quaternion.identity);
                raio1.GetComponent<Thunder>().SetDamage(Phase.ThunderDamage);
            }

            if (Player2.ToVivo)
            {
                GameObject raio2 = Instantiate(Raio, Player2.transform.position, Quaternion.identity);
                raio2.GetComponent<Thunder>().SetDamage(Phase.ThunderDamage);
            }

            yield return new WaitForSeconds(Random.Range(Phase.MinRaioCD, Phase.MaxRaioCD));
        }

        PhaseCount++;

        if (PhaseCount == 2)
        {
            AuraVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            AuraVFX.GetComponent<AudioSource>().Play();
            AlabardaVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            Phase = new Boss_Phase2();
        }
        else if (PhaseCount == 3)
        {
            Phase = new Boss_Phase3();

            PhaseVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            PhaseVFX.transform.GetChild(1).GetComponent<ParticleSystem>().Play();

            AudioSource[] v_a = PhaseVFX.GetComponents<AudioSource>();

            foreach (AudioSource item in v_a)
            {
                item.Play();
            }

            yield return new WaitForSeconds(1.22f);

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            anim = this.transform.GetChild(1).GetComponent<Animator>();
            AlabardaVFX = anim.transform.Find("RigBossRosto/Hand.R.001/AlabardaFix/RaioAlabarda").gameObject;
            HitArea = anim.gameObject.GetComponentInChildren<HitBoss>();
            anim.SetTrigger("Change");

            yield return new WaitForSeconds(.5f);

            StartCoroutine(Phase3Chaos());
        }

        anim.SetTrigger("ResetChange");
        Vector3 _Pos = new Vector3(transform.position.x, old_y, transform.position.z);

        while (Vector3.Distance(transform.position, _Pos) > .5f)
        {
            transform.position = Vector3.Lerp(transform.position, _Pos, Time.deltaTime * 7.5f);
            yield return new WaitForFixedUpdate();
        }

        rb.isKinematic = false;
        rb.useGravity = true;

        yield return new WaitWhile(() => GetBool("OnChange"));

        HUD.SetNewLife(PhaseCount, Phase.HP);
        enemy.HP = Phase.HP;

        TimeDistance = Time.time + 15;

        StartCoroutine(TakeDecision());

        yield return null;
    }

    IEnumerator Phase3Chaos()
    {
        yield return new WaitForSeconds(3f);

        while (enemy.HP > 0)
        {
            if (Player1.ToVivo)
            {
                GameObject raio1 = Instantiate(Raio, Player1.transform.position, Quaternion.identity);
                raio1.GetComponent<Thunder>().SetDamage(Phase.ThunderDamage);
            }

            if (Player2.ToVivo)
            {
                GameObject raio2 = Instantiate(Raio, Player2.transform.position, Quaternion.identity);
                raio2.GetComponent<Thunder>().SetDamage(Phase.ThunderDamage);
            }

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
