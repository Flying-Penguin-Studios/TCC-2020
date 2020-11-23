using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementoP2Teclado : MonoBehaviour {


    Rigidbody rb;
    private Animator anim;

    public float moveSpeed;
    public float rotationSpeed;



    private void Awake() {
        rb = GetComponent<Rigidbody>();
        anim = this.transform.GetChild(0).GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        rb.angularDrag = 999;
        rb.drag = 4;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }


    private void FixedUpdate() {
        MovimentacaoP2();
        Rotation();
    }


    private void MovimentacaoP2() {

        anim.speed = 1.25f;


        if(Input.GetKey(KeyCode.W)) {
            this.transform.position += (transform.forward * moveSpeed) * Time.deltaTime;
            anim.SetFloat("Speed", 1, 0.075f, Time.fixedDeltaTime);

        }

        if(Input.GetKey(KeyCode.S)) {
            this.transform.position -= (transform.forward * moveSpeed) * Time.deltaTime;
            anim.SetFloat("Speed", 1, 0.075f, Time.fixedDeltaTime);
        }

        if(Input.GetKey(KeyCode.E)) {
            this.transform.position += (transform.right * moveSpeed) * Time.deltaTime;
            anim.SetFloat("Speed", 1, 0.075f, Time.fixedDeltaTime);
        }

        if(Input.GetKey(KeyCode.Q)) {
            this.transform.position -= (transform.right * moveSpeed) * Time.deltaTime;
            anim.SetFloat("Speed", 1, 0.075f, Time.fixedDeltaTime);
        }

        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q)) {
            anim.SetFloat("Speed", 0, 0.075f, Time.fixedDeltaTime);
        }

    }

    private void Rotation() {

        if(Input.GetKey(KeyCode.A)) {
            this.transform.Rotate(this.transform.rotation.x, (this.transform.rotation.y - rotationSpeed) * Time.deltaTime, this.transform.rotation.z);
        }

        if(Input.GetKey(KeyCode.D)) {
            this.transform.Rotate(this.transform.rotation.x, (this.transform.rotation.y + rotationSpeed) * Time.deltaTime, this.transform.rotation.z);
        }
    }




}
