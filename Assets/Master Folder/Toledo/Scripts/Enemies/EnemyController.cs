using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public Enemy Enemy;
    public int Target;
    private string EnemyType;
    private Rigidbody rb;
    private Animator anim;
    private Transform TargetPosition;
    private bool IsAlive;
    public bool IsArenaEnemy;

    [Space(20)]
    //------------- Variáveis de Players -------------
    public GameObject Player1;
    public GameObject Player2;
    private float DistanceToPlayer1;
    private float DistanceToPlayer2;


    //------------- Variáveis de Movimento -----------

    public bool CanMove;
    private float IdleSpeed;
    private float moveAmout;
    private float OriginalEnemyAcceleration;
    private float EnemyAcceleration;

    private float PatrolRate = 0;
    private float NextPatrol;
    private bool ReachebleTarget;

    //------------- Variáveis de Combate -------------

    public bool InCombat;
    private bool Attacking;
    public bool BerserkerModeOn;
    public bool ActivatingBerserker;

    [Header("Range Distance")]
    public float AggoRange;
    public float MeleeAttackDistance;
    public float RangedAttackDistance;

    [Header("Patrol")]
    public bool HasPatrol;
    public float PatrolRange = 10;

    [Header("Attack Prefabs")]
    public GameObject ArcherProjetil;

    //------------ Archer --------------------------
    public bool ArcherAttack2;


    //------------ Zombie --------------------------
    private Animation attackAnimatio;

    private bool CanMoveToPoint;
    Vector3 NextRoutePoint;



    private void Awake() {
        rb = GetComponent<Rigidbody>();
        anim = this.transform.GetChild(0).GetComponent<Animator>();        
    }

    private void Start() {

        Player1 = GameController.Singleton.InstantiatedPlayer1;
        Player2 = GameController.Singleton.InstantiatedPlayer2;

        DefineEnemy();
        IsAlive = true;
        rb.angularDrag = 999;
        rb.drag = 0;
        IdleSpeed = 1;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        CanMove = true;
        OriginalEnemyAcceleration = 1f;
        EnemyAcceleration = OriginalEnemyAcceleration; 
        Attacking = false; 
        Enemy.AggoRange = AggoRange;
        Enemy.MeleeAttackDistance = MeleeAttackDistance;
        Enemy.RangedAttackDistance = RangedAttackDistance;
        BerserkerModeOn = false;
        ActivatingBerserker = false;
        NextPatrol = Time.time + 1;
        CanMoveToPoint = false;
        IsArenaEnemy = false;
        ReachebleTarget = true;
    }


    private void FixedUpdate() {
        EnemyIA();                
        UpdateHud();
        ComandoMatar();
    }




    /// <summary>
    /// Define o tipo de inimigo que será instanciado (Archer, Guard or Zombie).
    /// </summary>
    private void DefineEnemy() {
        if(this.name.Contains("Soldier")) { Enemy = new Soldier(); EnemyType = "Soldier"; Enemy.EnemyRangeType = "Melee"; }
        if(this.name.Contains("Guard")) { Enemy = new Guard(); EnemyType = "Guard"; Enemy.EnemyRangeType = "Melee"; }
        if(this.name.Contains("Archer")) { Enemy = new Archer(); EnemyType = "Archer"; Enemy.EnemyRangeType = "Ranged"; }
        if(this.name.Contains("Zombie")) { Enemy = new Zombie(); EnemyType = "Zombie"; Enemy.EnemyRangeType = "Melee"; }
    }

     
    /// <summary>
    /// Controla as tomadas de decisões do inimigo.
    /// </summary>
    private void EnemyIA() {

        InCombat = CheckIncombat();


        if(CheckAlive()) {

            CheckPlayerAlive();
            CheckDistance();
            FakeGravity();

            if(InCombat) {

                EnemyBehaviour();

                SetTargetByThreat();
                MoveToPlayer();

                EnemyAttack();                

            } else {
                Patrol();
                StartCombatByRange();
            }

        } else {
            EnemyDeath();
            SlowOnDeath();
        }
        
        
    }


    /// <summary>
    /// Inicia combate com um jogador caso ele se aproxime e entre no AgroRange.
    /// </summary>
    private void StartCombatByRange() {
        Enemy.StartCombatByRange(DistanceToPlayer1, DistanceToPlayer2);
    }


    /// <summary>
    /// Inicia combate com um jogador caso ele ataque.
    /// O alvo do inimgo será o jogador que gerar mais Threat, tanto batendo quanto curando.
    /// </summary>
    private void SetTargetByThreat() {
        Enemy.SetTargetByThreat(DistanceToPlayer1, DistanceToPlayer2);
    }


    /// <summary>
    /// Define a distância até os jogador.
    /// </summary>
    private void CheckDistance() {
        DistanceToPlayer1 = Vector3.Distance(this.transform.position, Player1.transform.position);
        DistanceToPlayer2 = Vector3.Distance(this.transform.position, Player2.transform.position);
    }


    /// <summary>
    /// Patrulhas se HasPatrol estiver ativado no Inspector.
    /// </summary>
    private void Patrol() {
        if(HasPatrol) {
            RandonRoute();
        }        
    }
    
    
    /// <summary>
    /// Caso o inimigo esteja fora de combate, define pontos aleatórios para movimentação no mapa.
    /// </summary>
    private void RandonRoute() {
        
        float PointX;
        float PointZ;
        float distance;

        if(Time.time >= NextPatrol) {

            NextPatrol = Time.time + PatrolRate;

            //Gera uma posição aleatória
            PointX = Random.Range(-PatrolRange, PatrolRange);
            PointZ = Random.Range(-PatrolRange, PatrolRange);    
            
            NextRoutePoint = new Vector3(this.transform.position.x + PointX, this.transform.position.y, this.transform.position.z + PointZ);
            distance = Vector3.Distance(this.transform.position, NextRoutePoint);


            RaycastHit Hit1;            
            if(!Physics.Linecast(this.transform.position + new Vector3(0, 1f, 0), NextRoutePoint, out Hit1) && (distance > 4)) {

                RaycastHit Hit2, Hit3;
                if((Physics.Raycast(NextRoutePoint, Vector3.down, out Hit2) && (Hit2.collider.gameObject.CompareTag("Terrain") || Hit2.collider.gameObject.CompareTag("ObjetosDeCena"))) &&
                    (Physics.Raycast((this.transform.position + NextRoutePoint) / 2, Vector3.down, out Hit3) && (Hit3.collider.gameObject.CompareTag("Terrain") || Hit3.collider.gameObject.CompareTag("ObjetosDeCena")))) {

                    CanMoveToPoint = true;
                    NextPatrol = Time.time + 5;

                } else {
                    CanMoveToPoint = false;                    
                }

            } else {
                CanMoveToPoint = false;
            }
        }

        if(CanMoveToPoint) { MoveToPoint(NextRoutePoint); }

    }


    /// <summary>
    /// Morte do Inimigo. Destroi o gameObject, mas antes disso aplica um fadeOut gradativo na malha do Objeto; ((( --- ótimo lugar pra se usar um Shader ---)))
    /// </summary>
    private void EnemyDeath() {

        if(IsAlive) {

            IsAlive = false;

            Enemy.SetEnemyDead();

            StartCoroutine("AnimatorSlowDown");
            StartCoroutine("FadeOnDeath");

            //Desativa o Collider, permitindo que os tiros atravessem o inimigo enquanto ele dissolve.
            //Também trava a posição em Y, para que o iniimgo n caia, já que está sem o collider.
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            this.GetComponent<Collider>().enabled = false;

            ArenaEnemyCount();
            Destroy(this.gameObject, 1.75f);
        }
             
    }


    /// <summary>
    /// Retorna verdadeiro caso o iniimgo tenha terreno em sua frente.. Caso contrário, retorna falso
    /// </summary>
    /// <returns></returns>
    private bool EnemyCanMove() {

        RaycastHit Terrain;
        if(Physics.Raycast(this.transform.position + (this.transform.forward * 1.5f) + new Vector3(0, 1, 0), Vector3.down * 1.3f, out Terrain, 1.3f)) {

            if(Terrain.collider.CompareTag("Terrain") || Terrain.collider.CompareTag("ObjetosDeCena") || Terrain.collider.CompareTag("HitCollider")) {
                return true;
            } else {

                if(ReachebleTarget) {   //Troca de alvo a cada 0.25s caso o alvo atual esteja em outra ilha, e inalcansável.
                    ReachebleTarget = false;
                    StartCoroutine(SetReachebleTarget());                    
                }

                anim.SetFloat("Speed", 0);
                return false;
            }            

        } else {

            if(ReachebleTarget) {   //BoyBand - Troca de alvo a cada 0.25s caso o alvo atual esteja em outra ilha, e inalcansável.
                ReachebleTarget = false;
                StartCoroutine(SetReachebleTarget());
            }

            anim.SetFloat("Speed", 0);
            return false;            
        }        
    }


    /// <summary>
    /// Troca de alvo caso o alvo atual esteja fora da ilha e inalcansável.
    /// </summary>
    /// <returns></returns>
    IEnumerator SetReachebleTarget() {

        yield return new WaitForSeconds(0.5f);

        if(Enemy.Target == 1) {
            Enemy.ThreatPlayer2 = Enemy.ThreatPlayer1;
            Enemy.Target = 2;
        } else if(Enemy.Target == 2) {
            Enemy.ThreatPlayer1 = Enemy.ThreatPlayer2;
            Enemy.Target = 1;
        }

        yield return new WaitForSeconds(0.5f);

        ReachebleTarget = true;
        yield return null;

    }



    /// <summary>
    /// Checa a distancia até o jogador, e caso ele esteja próximo, começa a persegui-lo.
    /// </summary>
    private void MoveToPlayer() {

        Target = Enemy.Target;

        if(EnemyCanMove() && CanMove) {

            if((Target == 1) && !Attacking && !ActivatingBerserker && !InAttackRange(DistanceToPlayer1)) {

                ChangeSpeed(true);
                LookToPoint(Player1);

            } else if((Target == 2) && !Attacking && !ActivatingBerserker && !InAttackRange(DistanceToPlayer2)) {

                ChangeSpeed(true);
                LookToPoint(Player2);

            //Tem range pra atacar, mas não está atacando. Ou sejá, acabou de entrar em range para atacar. Ou sejá, para o inimigo antes de iniciar o ataque.
            } else if(Attacking || ActivatingBerserker) {

                moveAmout = 0; 
                rb.velocity = Vector3.zero;
                anim.SetFloat("Speed", moveAmout);
                MoveAnimationModifier(moveAmout);

            } 

            anim.SetFloat("WalkType", 1);
            MoveAnimationModifier(moveAmout * Enemy.CombaAnimSpeed);


            //KinematicMovement(Enemy.CombatSpeed);

            rb.velocity = this.transform.forward.normalized * (Enemy.CombatSpeed * moveAmout);
        }
        
    }


    /// <summary>
    /// Move o inimigo até o ponto definido em RandomRoute.
    /// </summary>
    /// <param name="point"></param>
    private void MoveToPoint(Vector3 point) {

        anim.SetFloat("WalkType", 0);

        if(Vector3.Distance(this.transform.position, point) >= 3f) {     // <= 2 por causa do collider do player.. Caso contrário ele fica girando ao chegar no ponto.           

            var lookPos = point - this.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

            ChangeSpeed(true);
            MoveAnimationModifier(moveAmout * Enemy.PatrolAnimSpeed);
            //KinematicMovement(Enemy.PatrolSpeed);
            rb.velocity = this.transform.forward.normalized * (Enemy.PatrolSpeed * moveAmout);

        } else {

            ChangeSpeed(false);
            MoveAnimationModifier(IdleSpeed);   //Define a velocidade do Idle;
            rb.velocity = this.transform.forward.normalized * (Enemy.PatrolSpeed * moveAmout);
        }

    }

    
    /// <summary>
    /// Acelera ou desacelera o inimigo com base na boleana Accelerate.
    /// </summary>
    /// <param name="Accelerate"></param>
    private void ChangeSpeed(bool Accelerate) {

        if(Accelerate) {

            if(moveAmout < 1) {
                moveAmout += 0.1f;
            }

        } else {

            if(moveAmout > 0) {

                moveAmout -= 0.04f;
                if(moveAmout >= 0.6) { MoveAnimationModifier(moveAmout); }

            }
        }

        anim.SetFloat("Speed", moveAmout);
    }


    /// <summary>
    /// Retorna falso se não estiver em Range pra atacar.
    /// </summary>
    /// <param name="DistanceToPlayer"></param>
    /// <returns></returns>
    public bool InAttackRange(float DistanceToPlayer) {

        if((Enemy.EnemyRangeType == "Melee") && (DistanceToPlayer >= Enemy.MeleeAttackDistance)) {
            return false;
        } else if((Enemy.EnemyRangeType == "Ranged") && (DistanceToPlayer >= Enemy.RangedAttackDistance)) {
            return false;
        } else {
            return true;
        }
    }


    /// <summary>
    /// Checa e seta se os players estão vivos.
    /// </summary>
    private void CheckPlayerAlive() {

        if (Player1.GetComponent<PlayerController>().ToVivo){
            Enemy.P1Alive = !Player1.GetComponent<PlayerController>().Caido;            
        }
        else{
            Enemy.P1Alive = Player1.GetComponent<PlayerController>().ToVivo;
        }

        if (Player2.GetComponent<PlayerController>().ToVivo){
            Enemy.P2Alive = !Player2.GetComponent<PlayerController>().Caido;
        }
        else{
            Enemy.P2Alive = Player2.GetComponent<PlayerController>().ToVivo;
        }

        //Enemy.P1Alive = Player1.GetComponent<PlayerController>().ToVivo;
        //Enemy.P2Alive = Player2.GetComponent<PlayerController>().ToVivo;
    }


    /// <summary>
    /// Verifica se o inimigo está em combate.
    /// </summary>
    /// <returns></returns>
    private bool CheckIncombat() {

        if(Enemy.IsInCombat()) {
            anim.SetBool("InCombat", true);
            return true;
        } else {
            anim.SetBool("InCombat", false);
            return false;
        }

    }


    /// <summary>
    /// Ataca o Jogador caso ele seteja vivo e no range de ataque.
    /// </summary>
    private void EnemyAttack() {

        if((Target == 1) && Enemy.P1Alive && InAttackRange(DistanceToPlayer1)) {
            
            if(!Attacking) {
                Attacking = true;
                anim.SetTrigger("Attack");
            } 

            LookToPoint(Player1);

        } else if((Target == 2) && InAttackRange(DistanceToPlayer2) && Enemy.P2Alive) {
            
            if(!Attacking) {
                Attacking = true;
                anim.SetTrigger("Attack");
            }

            LookToPoint(Player2);
        }
    }


    /// <summary>
    /// Faz com que o inimigo esteja sempre olhando pro jogador alvo.
    /// </summary>
    /// <param name="target"></param>
    private void LookToPoint(GameObject target) {

        if(Enemy.EnemyRangeType == "Melee") {

            var lookPos = target.transform.position - this.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

        } else if(Enemy.EnemyRangeType == "Ranged") {

            var lookPos = target.transform.position - this.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

        }
    }


    /// <summary>
    /// Seta Attacking como falso, permitindo a execução de outras ações.
    /// </summary>
    public void AttackReset() {
        Attacking = false;
    }


    /// <summary>
    /// Mecanismo alternativo para mover o Inimigo.. Esse méto deve ser usado quando isKinematic no RigidBody estiver ativado.
    /// Também, uma falsa gravidade é criada.
    /// </summary>
    private void KinematicMovement(float speed) {

        this.transform.position += this.transform.forward * (Time.deltaTime * speed * moveAmout);

        RaycastHit Hit;
        if(!Physics.Raycast(this.transform.position, Vector3.down, out Hit, 0.15f)) {            
            this.transform.position -= this.transform.up * (Time.deltaTime * 4);
        }
    }


    /// <summary>
    /// Define o modo de combate e as ações de cada inimigo.
    /// </summary>
    private void EnemyBehaviour() {

        Enemy.EnemyBehaviour();

        //Archer
        FocusOnTarget();

        //Zombie
        EnterBerserkerMode();
    }


    public void TakeDamage(int dano, float stabDuratation = 0.15f) {

        Enemy.TakeDamage(dano);

        if(!Enemy.Name.Contains("Zombie")) {
            CanMove = false;
            anim.SetTrigger("Stag");
            Invoke("ReleaseEnemyMovemente", stabDuratation);
        }        

    }



    //Dissolve gradativamente a textura do inimigo.  ================> Corrigir e padronizar esses materiais!! <========================
    IEnumerator FadeOnDeath() {
        for(var t = 0f; t <= 1; t += Time.deltaTime * 0.5f) {

            this.transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1 - (t + 0.2f); //(t + 0.2) No caso, o 0.2 é para que a barra desapareça ppor total antes de acabar o efeito.

            if(Enemy.Name == "Soldier") {
                this.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetFloat("_Amount", t);
                this.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetFloat("_Amount", t);
                this.transform.GetChild(0).GetChild(3).GetComponent<Renderer>().material.SetFloat("_Amount", t);
            } else if(Enemy.Name == "Guard") {
                this.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetFloat("_Amount", t);
            } else if(Enemy.Name == "Archer") {
                this.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetFloat("_Amount", t);
                this.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetFloat("_Amount", t);
                this.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetFloat("_Amount", t);
            } else if(Enemy.Name == "Zombie") {
                this.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetFloat("_Amount", t);
            }   

            yield return null;
        }
    }


    /// <summary>
    /// Desacelera gradativamente o Animator. Usado para desacelerar as animações do inimigo ao morrer.
    /// </summary>
    IEnumerator AnimatorSlowDown() {
        for(var t = 1f; t >= 0; t -= Time.deltaTime * 1.5f) {
            anim.speed = t;
            yield return null;
        }
    }


    /// <summary>
    /// Ajusta a velocidade da animação de movimento, conforme a velocidade do movimento.
    /// </summary>
    /// <param name="modifier"></param>
    private void MoveAnimationModifier(float modifier) {
        if(this.Enemy.Name == "Soldier") anim.SetFloat("SoldierMoveSpeedModifier", modifier);
        if(this.Enemy.Name == "Archer") anim.SetFloat("ArcherMoveSpeedModifier", modifier);
        if(this.Enemy.Name == "Guard") anim.SetFloat("GuardMoveSpeedModifier", modifier);
        if(this.Enemy.Name == "Zombie" && !BerserkerModeOn) anim.SetFloat("ZombieMoveSpeedModifier", modifier);
    }


    /// <summary>
    /// Elimina todos os inimigos ao apertar a tecla M.
    /// </summary>
    private void ComandoMatar() {

        if(Input.GetKeyDown(KeyCode.M)) {
            Enemy.isAlive = false;
        }
    }


    /// <summary>
    /// Atualiza Hud com o Threat de cada player.
    /// </summary>
    private void UpdateHud() {
        FixLifeBarRotation();
        UpdateLifeBar();
    }


    /// <summary>
    /// Mantem a Barra de Vida dos inimigos sempre viradas pra câmera.
    /// </summary>
    private void FixLifeBarRotation() {
        this.transform.GetChild(1).GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
    }


    /// <summary>
    /// Atualiza a Barra de Vida dos Inimigos.
    /// </summary>
    private void UpdateLifeBar() {
        this.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = ((float)Enemy.HP / (float)Enemy.MaxHP);
    }


    /// <summary>
    /// Coloca o Zumbi em modo Berserker.
    /// </summary>
    private void EnterBerserkerMode() {

        if(Enemy.BerserkerMode && !BerserkerModeOn) {

            Enemy.IsVulnerable = false;
            anim.SetFloat("ZombieAtkSpeedModifier", 1.8f);
            anim.SetFloat("ZombieMoveSpeedModifier", 1f);
            Enemy.CombatSpeed = 3.5f;
            Enemy.MeleeAttackDistance = 2.25f;
            StartCoroutine("MakeZombieBigger");
            ActivatingBerserker = true;
            anim.SetTrigger("Berserker");
            StartCoroutine(MakeZombieVulnerable());
            BerserkerModeOn = true;
        }
    }

    
    /// <summary>
    /// Aumenta o tamanho do modelo do Zumbi.
    /// </summary>
    IEnumerator MakeZombieBigger() {

        float FadeSpeed = 0.2f;

        for(var t = 1.4f; t <= 1.7f; t += Time.deltaTime * FadeSpeed) {
            this.transform.localScale = new Vector3(t, t, t);
            yield return null;
        }
    }


    IEnumerator MakeZombieVulnerable() {
        yield return new WaitForSeconds(2.5f);
        Enemy.IsVulnerable = true;
    }
    

    /// <summary>
    /// Mantem o inimigo olhando para o player mesmo com ele fora do AttackRange.
    /// Método usado no scopo de comportamento da Arqueira.
    /// </summary>
    private void FocusOnTarget() {
        if(Target == 1 && Enemy.P1Alive) LookToPoint(Player1);
        if(Target == 2 && Enemy.P2Alive) LookToPoint(Player2);
    }


    /// <summary>
    /// Gera 1 de agro neste inimigo para o player 1. Usado na arena pra forçar os inimigos a atacarem o jogador.
    /// Alterar futuramente pra gerar agro aleatóriamente entre player 1 e 2, quando o player 2 estiver implementado.
    /// </summary>
    public void SetRandomTarget() {
        Enemy.GenerateThreat("Player 1", 1);
    }


    /// <summary>
    /// Subtrai 1 na contagem dos inimigos da arena.
    /// </summary>
    private void ArenaEnemyCount() {
        if(IsArenaEnemy) {
            GameController.Singleton.cenarioController.GetComponent<CenarioController>().ArenaEnemyCount--;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void FakeGravity() {
        rb.velocity += Vector3.down;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool CheckAlive() {
        
        if(Enemy.isAlive) {
            return true;
        } else {
            return false;
        }
    }


    private void SlowOnDeath() {
        ChangeSpeed(false);
        rb.velocity = this.transform.forward.normalized * (Enemy.PatrolSpeed * moveAmout);
    }


    public void ReleaseEnemyMovemente() {
        CanMove = true;
    }


    /// <summary>
    /// 
    /// </summary>
    public void ShoutThreat() {
        Enemy.ThreatPlayer1 += 100;
        Invoke("RemoveThreat", 5);
    }


    private void RemoveThreat() {
        Enemy.ThreatPlayer1 = 1;
        Enemy.ThreatPlayer2 = 1;
    }






}