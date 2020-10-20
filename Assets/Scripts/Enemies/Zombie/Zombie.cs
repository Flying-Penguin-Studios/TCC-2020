using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Mob {

    
    [Tooltip("Porcentagem (%) de vida para entrar em Berserker Mode"), Header("Zombie"), Space(10)]
    public float LifeToBerserkerMode;






    private void EnterBerserkerMode() {

    }





    public override void TakeDamage(int damage, string player) {
        
        //Executa as regras da classe pai.
        //base.TakeDamage(damage);        
        
        if(enemy.HP <= (enemy.HP_Max * (LifeToBerserkerMode / 100))) {

        }
    }





}
