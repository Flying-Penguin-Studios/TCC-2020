using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mob : EnemyController {

    

    // --- Variáveis gerais
    private PlayerController Player1;
    private GameObject player1O;
    private PlayerController Player2;
    private Vector3 velocity;                           //Velocidade do Inimigo.
    private float accelerationFactor = 2;               //Fator de aceleração do inimigo.
    private float decelerationFactor = 2;               //Fator de desaceleração do inimigo.
    private float speed;                                //Velocidade atual do Inimigo
    private bool inCombat;
    private bool isAlive = true;


    // --- Variáveis de Patrulha
    [Space(20)]
    public Transform nextWaypoint;
    public bool patrulheiro;
    public float PatrolSpeed;                       //Velocidade máxima durante a patrulha.    
    public Vector2 minMaxPatroPause;                    //Tempo minimo e máximo de pausa entre as Patrulhas.
    protected bool acceleration = false;
    private bool isPatrolling;                          //Define se o inimigo está ou não em patrulha.
    private float lastPatrolTime;                       //Guarda o momento (Time.time) da última patrulha.
    private float extraPouseTime;                       //Tempo extra de pausa para acréscimos gerais.



    // --- Variáveis de Combte e Aggro    
    protected GameObject Target;
    [Space(20)]
    public float combatSpeed;
    private bool isVulnerable = false;
    private int P1Agro = 0;
    private int P2Agro = 0;
    private bool P1Incombat = false;
    private bool P2Incombat = false;
    protected float minDistanceToPlayer = 1.5f;
    public float aggroRange;
    [HideInInspector]
    public bool attacking = false;


    

    



    




    private void Start() {

        Player1 = GameController.Singleton.ScenePlayer1.GetComponent<PlayerController>();
        Player2 = GameController.Singleton.ScenePlayer2.GetComponent<PlayerController>();        

        anim = this.transform.GetChild(0).GetComponent<Animator>();

        Init(speed);
        isPatrolling = false;
        lastPatrolTime = 0;

        FreezeConstraints(true);
    }


    private void FixedUpdate() {
        EnemyBehavior();
        transform.GetChild(1).LookAt(Camera.main.gameObject.transform);
    }


    


    /// <summary>
    /// Controla todo o comportamento dos inimigos. Desde movimento, até o combate.
    /// </summary>
    public void EnemyBehavior() {
        
        if(CheckInCombate()) {
            Combat();
        } else {
            if(patrulheiro) Patrol();
            StartCombatByDistance();
        }
    }


 

    /// <summary>
    /// 
    /// </summary>
    private void Patrol() {

        if((Time.time >= lastPatrolTime) && !isPatrolling) {
            transform.rotation = Quaternion.LookRotation(nextWaypoint.position - transform.position, Vector3.up);
            acceleration = true;
            isPatrolling = true;
        }

        Vector3 dist = nextWaypoint.transform.position - transform.position;

        if(dist.magnitude <= 1.5f) {
            nextWaypoint = nextWaypoint.GetComponent<WayPoint>().getNext();
            acceleration = false;
            isPatrolling = false;
            lastPatrolTime = Time.time + Random.Range(minMaxPatroPause.x, minMaxPatroPause.y);
        }

        Accelerate();
    }

    
    /// <summary>
    /// Aumenta ou diminui a velocidade do inimigo de forma gradativa.
    /// </summary>
    private void Accelerate() {

        if(acceleration) {
            FreezeConstraints(false);
            if(speed < 1) speed += accelerationFactor * Time.fixedDeltaTime;
        } else {
            //if(speed > 0) speed -= decelerationFactor * Time.fixedDeltaTime;
            //if(speed <= 0.1f)  speed = 0;
            speed = 0;
            FreezeConstraints(true);
        }

        anim.SetFloat("Speed", speed);


        float movementSpeed;
        movementSpeed = (inCombat) ? combatSpeed : PatrolSpeed;

        velocity = transform.forward * movementSpeed * speed;
        rb.velocity = velocity;        
    }


    protected virtual void Combat() {
                
    }


    private void StartCombatByDistance() {

        if(Player1.ToVivo && (Vector3.Distance(transform.position, Player1.transform.position) <= aggroRange)) {
            inCombat = true;
            P1Incombat = true;
            P1Agro += 1;
        }

        if(Player2.ToVivo && (Vector3.Distance(transform.position, Player2.transform.position) <= aggroRange)) {
            inCombat = true;
            P2Incombat = true;
            P2Agro += 1;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    protected GameObject SetTarget() {

        if(Player1.ToVivo && Player2.ToVivo) {

            if(P1Agro >= P2Agro) {
                return Player1.gameObject;
            } else {
                return Player2.gameObject;
            }

        } else if(!Player2.ToVivo) {
            P2Agro = 0;
            return Player1.gameObject;
        } else {
            P1Agro = 0;
            return Player2.gameObject;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    protected void ChaseTarget() {

        LookToTarget();

        acceleration = !attacking;
        Accelerate();
    }


    protected virtual void Attack() {
        
    }


    /// <summary>
    /// Retorna verdadeiro se estiver em combate com algum dos Jogadores.
    /// </summary>
    private bool CheckInCombate() {

        if(((Player1.ToVivo && !Player1.Caido && P1Incombat) || (Player2.ToVivo && !Player2.Caido && P2Incombat)) && isAlive) {

            inCombat = true;
            anim.SetFloat("InCombat", 1);
            return true;

        } else {

            inCombat = false;
            P1Agro = 0;
            P2Agro = 0;
            P1Incombat = false;
            P2Incombat = false;
            return false;
        }
    }

       
    /// <summary>
    /// Regras ao tomar dano. Ex: Stag, verificação de HP, se morreu, etc...
    /// </summary>
    public override void TakeDamage(int damage, string player) {

        if(player == "Player1") {
            P1Incombat = true;
            P1Agro += damage;
        } else if(player == "Player2") {
            P2Incombat = true;
            P2Agro += damage;
        }

        if(!isVulnerable) enemy.HP -= damage;

        UpdateHP();

        if(enemy.HP <= 0 && isAlive) {
            isAlive = false;
            anim.SetTrigger("Dead");
            Die();
        }
    }


    /// <summary>
    /// Destroi o Inimigo.
    /// </summary>
    public void Die() {
        Destroy(this.gameObject, 5);
    }


    /// <summary>
    /// Trava ou libera as Constraints de posição do inimigo.
    /// </summary>
    /// <param name="freeze"></param>
    private void FreezeConstraints(bool freeze) {

        if(freeze) {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        } else {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }


    /// <summary>
    /// Calcula a distancia até o Alvo, desconsiderando o eixo Y.
    /// </summary>
    protected float DistanceToTarget() {
        return Vector3.Distance(transform.position, new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z));         
    }


    /// <summary>
    /// Olha pra direção do alvo.
    /// </summary>
    private void LookToTarget() {
        transform.LookAt(new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z));
    }




    /// <summary>
    /// Atualiza A Barra de vida dos inimigos.
    /// </summary>
    private void UpdateHP() {
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
        this.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = ((float)enemy.HP / (float)enemy.HP_Max);
    }





}
