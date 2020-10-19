using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private int HP_Max;


    protected Enemy enemy;
    protected Rigidbody rb;
    protected Animator anim;



    /// <summary>
    /// Inicializa o Inimigo.
    /// </summary>
    protected void Init(float speed)
    {
        enemy = new Enemy(gameObject.name, HP_Max, speed);
        rb = GetComponent<Rigidbody>();
        anim = this.transform.GetChild(0).GetComponent<Animator>();
    }


    public virtual void TakeDamage(int damage, string player)
    {
        return;
    }








}
