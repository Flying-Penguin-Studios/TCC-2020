using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttack : MonoBehaviour {


    public float BasicAttackSpeed;
    public GameObject ArcherProjetil;
    public GameObject ArrowSmokeParticle;
    public Animator anim;    
    private GameObject ArrowOrigin;
    private GameObject InstantiatedArrow;
    private bool ShadowAttackArrow;


    private void Start() {
        ArcherProjetil = this.transform.parent.GetComponent<EnemyController_Old>().ArcherProjetil;
        ArrowOrigin = this.transform.parent.gameObject.transform.GetChild(2).gameObject;
        anim = this.GetComponent<Animator>();
    }


    //Instancia a flecha do inimigo ranged.
    public void Attack() {

        if(this.transform.parent.GetComponent<EnemyController_Old>().Enemy.isAlive) {
            if(this.transform.parent.GetComponent<EnemyController_Old>().Enemy.ArcherAttack2) {
                //TripleAttack();
                ShadowArrow();
            } else {
                BasicAttack();
            }
        }
        
    }


    /// <summary>
    /// Attack Simples. Pode conter efeito de Partículas.
    /// </summary>
    private void InstantiateAttack() {

        InstantiatedArrow = Instantiate(ArcherProjetil, new Vector3(ArrowOrigin.transform.position.x, ArrowOrigin.transform.position.y, ArrowOrigin.transform.position.z), Quaternion.Euler(ArcherProjetil.transform.rotation.x, this.transform.eulerAngles.y, ArcherProjetil.transform.rotation.z));

        if(ShadowAttackArrow) {
            InstantiatedArrow.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
            ShadowAttackArrow = false;
            this.transform.parent.GetComponent<EnemyController_Old>().Enemy.ArcherAttack2 = false;
        }

    }



    private void BasicAttack() {
        anim.SetFloat("ArcherAttackSpeedModifier", BasicAttackSpeed);
        Invoke("InstantiateAttack", 0.8f);
    }



    /// <summary>
    /// Ataque Triplo.
    /// </summary>
    public void TripleAttack() {
        anim.SetFloat("ArcherAttackSpeedModifier", 1);
        this.transform.parent.GetChild(3).GetChild(0).GetComponent<ParticleSystem>().Play();
        this.transform.parent.GetChild(3).GetChild(1).GetComponent<ParticleSystem>().Play();
        Invoke("InstantiateAttack", 1.55f);
        Invoke("InstantiateAttack", 1.65f);
        Invoke("InstantiateAttack", 1.75f);
        this.transform.parent.GetComponent<EnemyController_Old>().Enemy.ArcherAttack2 = false;
    }


    /// <summary>
    /// Um ataque mais forte com efeitos de partícula.. Nome e cores do ataque precisam de ajustes.
    /// </summary>
    private void ShadowArrow() {
        anim.SetFloat("ArcherAttackSpeedModifier", 1);
        ShadowAttackArrow = true;
        Invoke("InstantiateAttack", 1.75f);
        Invoke("ShadowAttackParticle", 1.75f);
        this.transform.parent.GetComponent<EnemyController_Old>().Enemy.ArcherAttack2 = false;
    }


    /// <summary>
    /// 
    /// </summary>
    private void ShadowAttackParticle() {
        Instantiate(ArrowSmokeParticle, new Vector3(ArrowOrigin.transform.position.x, ArrowOrigin.transform.position.y - 1.4f, ArrowOrigin.transform.position.z), this.transform.rotation);
    }


    /// <summary>
    /// 
    /// </summary>
    private void SetAttackSpeed() {

        if(this.transform.parent.GetComponent<EnemyController_Old>().Enemy.ArcherAttack2) {
            //anim.SetFloat("ArcherAttackSpeedModifier", 1);
        } else {
            //anim.SetFloat("ArcherAttackSpeedModifier", BasicAttackSpeed);
        }
    }



}
