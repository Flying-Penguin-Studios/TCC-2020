using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : EnemyController {




    // --- Variáveis gerais
    public Vector3 velocity;                            //Velocidade do Inimigo.
    public float EnemyMass;                             //Massa do Inimigo
    private bool acceleration;                          //Define se o inimigo irá acelerar ou desacelerar.
    public float accelerationFactor;                    //Fator de aceleração do inimigo.
    public float decelerationFactor;                    //Fator de desaceleração do inimigo.
    private float yGround;                              //Posição em Y do terreno;

    [Range(0.0f, 1.0f)]
    public float speed;                                 //Velocidade atual do Inimigo



    // --- Variáveis de Patrulha
    public Vector3 targetPosition;
    public List<Vector3> spotPosition;                  //Próxima posição para onde o inimigo deve se mover na patrulha.
    public float PatrolSpeed = 2;                       //Velocidade máxima durante a patrulha.    
    public Vector2 minMaxPatroPause;                    //Tempo minimo e máximo de pausa entre as Patrulhas.
    private bool isPatrolling;                          //Define se o inimigo está ou não em patrulha.
    private float lastPatrolTime;                       //Guarda o momento (Time.time) da última patrulha.
    public float endRoutPauseTime = 10;                 //Tempo extra de pausa ao chegar no ultimo ponto da patrulha.
    private float extraPouseTime;                       //Tempo extra de pausa para acréscimos gerais.
    private bool isLastSpot;                            //Indica se é o último spot a ser patrulhado.
    private int spotNumber;
    private int direction;                              //Define a direção que o inimigo está fazendo a patrulha. 1 indo e -1 voltando.






    // --- Variaveis de Combate
    public float CombatSpeed = 5;
    public float aggroRange;




    private void Start() {

        anim = this.transform.GetChild(0).GetComponent<Animator>();

        Init(speed);
        isPatrolling = false;
        lastPatrolTime = 0;
        spotNumber = 0;
        direction = -1;
        yGround = GetGoundYPosition();
    }


    private void FixedUpdate() {
        EnemyBehavior();
    }


    


    /// <summary>
    /// Controla todo o comportamento dos inimigos. Desde movimento, até o combate.
    /// </summary>
    public void EnemyBehavior() {
        Patrol();
        Movement();
    }


 


    private void Movement() {
        Accelerate();
    }
    

    /// <summary>
    /// Realiza a patrulha entre spots pré-definidos no mapa. Ao acabar a sequencia de spots,
    /// patrulha os mesmos spots em ordem contrária, fazendo o mesmo caminho de volta.
    /// </summary>
    private void Patrol() {        

        if((Time.time >= lastPatrolTime) && !isPatrolling) {
            
            
            targetPosition = spotPosition[spotNumber];
            targetPosition.y = this.transform.position.y;
            
            this.transform.LookAt(targetPosition);

            

            extraPouseTime = 0;
            if((spotNumber == spotPosition.Count - 1) || (spotNumber == 0)) {
                direction *= -1;
                extraPouseTime = endRoutPauseTime;    
            } 
            spotNumber += direction;


            acceleration = true;
            isPatrolling = true;
            
        }

        this.transform.LookAt(targetPosition);


        if((Vector3.Distance(targetPosition, transform.position) <= 1.5f) && isPatrolling) {
            acceleration = false;
            lastPatrolTime = Time.time + Random.Range(minMaxPatroPause.x, minMaxPatroPause.y) + extraPouseTime;
            isPatrolling = false;
            
        }
    
        velocity = transform.forward * PatrolSpeed * speed;
        velocity.y = yGround;

        anim.SetFloat("WalkType", 1);
        anim.SetFloat("GuardMoveSpeedModifier", speed);
        rb.velocity = velocity;
    }


    /// <summary>
    /// Aumenta ou diminui a velocidade do inimigo de forma gradativa.
    /// </summary>
    private void Accelerate() {

        if(acceleration) {
            if(speed < 1) speed += accelerationFactor * Time.fixedDeltaTime;
        } else {
            if(speed > 0) speed -= decelerationFactor * Time.fixedDeltaTime;
            if(speed <= 0.1f) rb.velocity = Vector3.zero;
        }

        anim.SetFloat("Speed", speed);
    }



    /// <summary>
    /// Retorna a posição Y do terreno abaixo do inimigo.
    /// </summary>
    private float GetGoundYPosition() {

        RaycastHit hit;

        if(Physics.Raycast(this.transform.position, Vector3.down, out hit, 10)) {

            if(hit.collider.CompareTag("Terrain")) {
                return hit.transform.position.y;
            } else {
                return this.transform.position.y;
            }
        }
        return this.transform.position.y;
    }

       
    /// <summary>
    /// Regras ao tomar dano. Ex: Stag, verificação de HP, se morreu, etc...
    /// </summary>
    public virtual void TakeDamage() {
        Die();
    }



    public void Die() {
        Destroy(this.gameObject);
    }


}
