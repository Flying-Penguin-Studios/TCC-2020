using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    
    public int HP;
    public int MaxHP;
    public bool isAlive = false;

    //----- Hud
    public GameObject Hud;

    //----- Movement and Rotation
    public float speed = 6.0f;
    public float rotateSpeed = 10.0f;
    public float animSpeed = 1.5f;
    private float LastStickUse;
    private GameObject TargetInFront;
    private bool CastRayCast;
    private float GroundPosition;
    private GameObject MarkIndicator;

    //----- Jump -------------
    private float JumpRate = 0.5f;
    private float NextJump;
    private string PlayerJumpInput;
    private bool CanMove;
    private bool CanJump;
    public bool Jumping;
    private float Yvelocity;
    public float JumpDelay;

    Rigidbody rb;
    private Animator anim;
    private Vector3 velocity;
    public float moveAmout;
    private CenarioController cenario;




    private void Awake() {
        rb = GetComponent<Rigidbody>();
        anim = this.transform.GetChild(0).GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start() {
        rb.angularDrag = 999;
        rb.drag = 4;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        NextJump = 0;
        CanMove = true;
        CanJump = true;
        Jumping = false;
        SetJumpPlayerInput();
        LastStickUse = 0;
        CastRayCast = true;
        MarkIndicator = this.transform.GetChild(1).gameObject;
        cenario = GameController.Singleton.cenarioController.GetComponent<CenarioController>();
    }


    // Update is called once per frame
    void FixedUpdate() {
        Movement();
        Jump();
        
        Vector3 vvv = this.transform.forward;
        Quaternion a1 = Quaternion.AngleAxis(-8, new Vector3(0, 1, 0));
        Quaternion a2 = Quaternion.AngleAxis(-4, new Vector3(0, 1, 0));
        Quaternion a3 = Quaternion.AngleAxis(4, new Vector3(0, 1, 0));
        Quaternion a4 = Quaternion.AngleAxis(8, new Vector3(0, 1, 0));


        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), (a1 * vvv) * 20, Color.green);
        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), (a2 * vvv) * 20, Color.green);
        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), this.transform.forward * 15, Color.green);
        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), (a3 * vvv) * 20, Color.green);
        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), (a4 * vvv) * 20, Color.green);
    }


    private void LateUpdate() {
        MarkOnGround();
    }




    private void Movement() {

        if (CanMove) {

            Vector3 v;
            Vector3 h;

            if (this.gameObject.name == "Player 1") {
                v = Input.GetAxis("P1_L_Joystick_Vertical") * Vector3.forward;
                h = Input.GetAxis("P1_L_Joystick_Horizontal") * Vector3.right;
            } else {
                v = Input.GetAxis("P2_L_Joystick_Vertical") * Vector3.forward;
                h = Input.GetAxis("P2_L_Joystick_Horizontal") * Vector3.right;
            }

            Vector3 moveDir = (v + h).normalized;

            float m;

            if (this.gameObject.name == "Player 1") {
                m = Mathf.Abs(Input.GetAxis("P1_L_Joystick_Horizontal")) + Mathf.Abs(Input.GetAxis("P1_L_Joystick_Vertical"));
            } else {
                m = Mathf.Abs(Input.GetAxis("P2_L_Joystick_Horizontal")) + Mathf.Abs(Input.GetAxis("P2_L_Joystick_Vertical"));
            }

            moveAmout = Mathf.Clamp01(m);

            anim.SetFloat("Speed", moveAmout, 0.075f, Time.fixedDeltaTime);
            anim.speed = animSpeed;


            Yvelocity = rb.velocity.y;
            rb.drag = moveAmout > 0 ? 0 : 4;
            rb.drag = (Jumping) ? 0 : 4;
            speed = (Jumping) ? 7 : 6;

            rb.velocity = moveDir * (speed * moveAmout);

            rb.velocity = new Vector3(rb.velocity.x, Yvelocity, rb.velocity.z);




            // Rotação            

            


            if(moveDir != Vector3.zero) {
                LastStickUse = Time.time;
                CastRayCast = false;
            }


            Vector3 targetDir = moveDir;
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;


            Quaternion tr;
            if((moveDir == Vector3.zero) && (Time.time <= (LastStickUse + 0.1f)) && !CastRayCast) {

                TargetInFront = ForwardRaycast();
                if(TargetInFront) this.transform.LookAt(TargetInFront.transform.position);

                CastRayCast = true;

            } else {

                tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotateSpeed * Time.deltaTime);
                transform.localRotation = targetRotation;
            }

            
        }
    }


    private void Jump() {

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position + new Vector3(0, 0.02f, 0), -Vector3.up, out hit, 0.05f)) {

            if (hit.transform.CompareTag("Terrain")) {

                anim.SetBool("Jump", false);

                CanMove = true;


                if (Jumping) {
                    Invoke("CamJump", JumpDelay);
                }

                Jumping = false;

            } 
        }


        if (CanJump) {

            if (Input.GetButton(PlayerJumpInput) && (Time.time >= NextJump)) {

                anim.SetBool("Jump", true);
                this.transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>().Stop();

                Jumping = true;
                CanMove = true;

                rb.drag = 0;


                this.GetComponent<Rigidbody>().AddForce(0, 10, 0, ForceMode.Impulse);
                //rb.velocity = new Vector3(0, 5, 0);
                NextJump = Time.time + JumpRate;

                CanJump = false;
            }

        }

    }


    private void CamJump() {
        CanJump = true;
    }


    private void SetJumpPlayerInput() {

        if (this.gameObject.name == "Player 1") {
            PlayerJumpInput = "P1_A";
        } else if (this.gameObject.name == "Player 2") {
            PlayerJumpInput = "P2_A";
        }
    }


    public void TakeDamage(int damage) {

        this.HP -= damage;

        if(HP <= 0) {
            isAlive = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.GetComponent<Collider>().enabled = false;
        }

    }



    public void RecoverHP(int heal) {

        if(HP < MaxHP) {

            if(HP + heal <= MaxHP) {
                HP += heal;
            } else {
                HP = MaxHP;
            }
        }
    }



    public void Kill() {
        TakeDamage(HP);
    }


    /// <summary>
    /// Ativa os componentes responsáveis por fazer o Player funcionar, assim como ficar visível.
    /// </summary>
    public void BackToLife() {
        isAlive = true;
        this.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Collider>().enabled = true;        
    }



    private GameObject ForwardRaycast() {

        float[] Dist = new float[5];
        Dist[0] = 500;
        Dist[1] = 500;
        Dist[2] = 500;
        Dist[3] = 500;
        Dist[4] = 500;

        float finalDist = 0;


        Vector3 central = this.transform.forward;
        Quaternion FullLeft = Quaternion.AngleAxis(-8, new Vector3(0, 1, 0));
        Quaternion Left = Quaternion.AngleAxis(-4, new Vector3(0, 1, 0));
        Quaternion Right = Quaternion.AngleAxis(4, new Vector3(0, 1, 0));
        Quaternion FullRight = Quaternion.AngleAxis(8, new Vector3(0, 1, 0));


        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), (FullLeft * central) * 20, Color.green);
        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), (Left * central) * 20, Color.green);
        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), this.transform.forward * 20, Color.green);
        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), (Right * central) * 20, Color.green);
        Debug.DrawRay(this.transform.position + new Vector3(0, 1, 0), (FullRight * central) * 20, Color.green);


        RaycastHit hit1;
        if((Physics.Raycast(this.transform.position + new Vector3(0, 1, 0), FullLeft * central, out hit1, 20))) {
            if(hit1.collider.gameObject.CompareTag("Enemy")) {
                Dist[0] = hit1.distance;
            }
        }        

        RaycastHit hit2;
        if((Physics.Raycast(this.transform.position + new Vector3(0, 1, 0), Left * central, out hit2, 20))) {
            if(hit2.collider.gameObject.CompareTag("Enemy")) {
                Dist[1] = hit2.distance;
            }
        }

        RaycastHit hit3;
        if((Physics.Raycast(this.transform.position + new Vector3(0, 1, 0), this.transform.forward, out hit3, 20))) {
            if(hit3.collider.gameObject.CompareTag("Enemy")) {
                Dist[2] = hit3.distance;
            }

        }

        RaycastHit hit4;
        if((Physics.Raycast(this.transform.position + new Vector3(0, 1, 0), Right * central, out hit4, 20))) {
            if(hit4.collider.gameObject.CompareTag("Enemy")) {
                Dist[3] = hit4.distance;
            }
        }

        RaycastHit hit5;
        if((Physics.Raycast(this.transform.position + new Vector3(0, 1, 0), FullRight * central, out hit5, 20))) {
            if(hit5.collider.gameObject.CompareTag("Enemy")) {
                Dist[4] = hit5.distance;
            }
        }


        finalDist = Mathf.Min(Dist[0], Dist[1], Dist[2], Dist[3], Dist[4]);
        
        if(finalDist != 500) {
            if(finalDist == Dist[0]) TargetInFront = hit1.collider.gameObject;
            if(finalDist == Dist[1]) TargetInFront = hit2.collider.gameObject;
            if(finalDist == Dist[2]) TargetInFront = hit3.collider.gameObject;
            if(finalDist == Dist[3]) TargetInFront = hit4.collider.gameObject;
            if(finalDist == Dist[4]) TargetInFront = hit5.collider.gameObject;
        } else {
            TargetInFront = null;
        }
                

        return TargetInFront;

    }


    /// <summary>
    /// Mantem o MarkIndicator no chão mesmo quando o player pular.
    /// </summary>
    private void MarkOnGround() {

        if(cenario.TerrainPossition(this.gameObject).HasGround && isAlive) {
            MarkIndicator.SetActive(true);
            MarkIndicator.transform.localPosition = new Vector3(MarkIndicator.transform.localPosition.x, - this.transform.position.y + cenario.TerrainPossition(this.gameObject).YPosition + 0.02f,MarkIndicator.transform.localPosition.z);
        } else {
            MarkIndicator.SetActive(false);
        }        
    }


}
