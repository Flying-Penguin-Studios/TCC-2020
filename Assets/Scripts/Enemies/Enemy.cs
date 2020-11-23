using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy {

    public string name;
    public int HP;
    public int HP_Max;
    public float speed;


    public Enemy(string name, int HP_Max, float speed) {
        this.name = name;
        this.HP = HP_Max;
        this.HP_Max = HP_Max;
        this.speed = speed;
    }


    /// <summary>
    /// Atualiza o HP do inimigo.
    /// </summary>
    public void ReduceHP() {
        return;
    }       


}
