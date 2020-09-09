using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Mob {

    
    [Tooltip("Porcentagem (%) de vida para entrar em Berserker Mode"), Header("Zombie"), Space(10)]
    public float LifeToBerserkerMode;






    private void EnterBerserkerMode() {

    }





    public override void TakeDamage() {
        
        //Executa as regras da classe pai.
        base.TakeDamage();        
        
        if(enemy.HP <= (enemy.HP_Max * (LifeToBerserkerMode / 100))) {

        }
    }





}
