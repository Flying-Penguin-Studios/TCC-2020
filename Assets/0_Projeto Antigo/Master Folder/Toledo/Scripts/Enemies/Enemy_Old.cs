using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Old {

    //variáveis de Status
    public string Name;
    public int HP;
    public int MaxHP;
    public float CombatSpeed;
    public float CombaAnimSpeed;
    public float PatrolSpeed;
    public float PatrolAnimSpeed;

    public bool isAlive = true;
    public bool inCombat = false;
    public bool IsVulnerable = true;


    public float AccelerateSpeed;
    public float DecelerateSpeed;


    public bool P1Alive = false;
    public bool P2Alive = false;
    public int Target;
    public string EnemyRangeType;
    public bool P1InAttackRange;
    public bool P2InAttackRange;
    public bool Player1InCombat = false;
    public bool Player2InCombat = false;
    public float AggoRange;
    public float MeleeAttackDistance;
    public float RangedAttackDistance;
    public bool P1InMeleeRange = false;
    public bool P2InMeleeRange = false;

    public float ThreatPlayer1;
    public float ThreatPlayer2;

    //----------------------------
    public bool BerserkerMode = false;
    public bool ArcherAttack2;
    public float Attack2Rate = 10;
    public float nextAttack2 = 0;
    public bool CanAttack2 = false;


    //----------------------------
    public string[] Threats = new string[2];
    
    


    //Define um alvo caso ele se aproxime.
    public void StartCombatByRange(float distanceP1, float distanceP2) {

        if(!P1Alive) { ThreatPlayer1 = 0; }
        if(!P2Alive) { ThreatPlayer2 = 0; }

        // Entra em combate com um jogador caso ele se aproxime.
        // - Entra em combate quando a distancia é menor que _AggroRange.        
        //Player 1
        if(P1Alive && (ThreatPlayer2 == 0) && InAggroRange(distanceP1)) {

            Target = 1;
            inCombat = true;
            Player1InCombat = true;
            P1InAttackRange = false;

            ThreatPlayer1 = 1;
        } 

        //Player 2
        if(P2Alive && (ThreatPlayer1 == 0) && InAggroRange(distanceP2)) {

            Target = 2;
            inCombat = true;
            Player2InCombat = true;
            P2InAttackRange = false;

            ThreatPlayer2 = 1;
        }
    }


    /// <summary>
    /// Define qual dos 2 Players será o alvo.
    /// </summary>
    /// <param name="distanceP1"></param>
    /// <param name="distanceP2"></param>
    public void SetTargetByThreat(float distanceP1, float distanceP2) {

        if((P1Alive && (ThreatPlayer1 >= 1)) || (P2Alive && (ThreatPlayer2 >= 1))) {

            if((ThreatPlayer1 > ThreatPlayer2)) {
                Target = 1;
            } else if((ThreatPlayer1 < ThreatPlayer2)) {
                Target = 2;
            }

            if(!P1Alive) { ThreatPlayer1 = 0; }
            if(!P2Alive) { ThreatPlayer2 = 0; }

        } else {
            Target = 0;
        }

    }


    /// <summary>
    /// Define se o inimigo está em combate.
    /// </summary>
    /// <returns></returns>
    public bool IsInCombat() {

        

        if((P1Alive && (ThreatPlayer1 >= 1)) || (P2Alive && (ThreatPlayer2 >= 1))) {
            inCombat = true;
            return true;
        } else {
            inCombat = false;    //Mudar para falso caso seja decidio que os inimigos podem sair de combate..
            return false;
        }

    }


    /// <summary>
    /// Retorna true caso o Jogador esteja em AggroRange, não estando em AttackRange (distância suficiente para atacar).
    /// </summary>
    /// <param name="distance"></param>
    public bool InAggroRange(float distance) {
        return ((distance <= AggoRange) && !InAttackRange(distance)) ? true : false ;
    }
    

    //Retorna False se não estiver em Range pra atacar.
    public bool InAttackRange(float PlayerDistance) {

        if((EnemyRangeType == "Melee") && (PlayerDistance >= MeleeAttackDistance)) {
            return false;
        } else if((EnemyRangeType == "Ranged") && (PlayerDistance >= RangedAttackDistance)) {
            return false;
        } else {
            return true;
        }
    }


    //Atualiza a variável de Threat do inimigo;
    public void GenerateThreat(string player, int threat) {

        if(player == "Player 1") {
            Player1InCombat = true;
            ThreatPlayer1 += threat;
        } else if(player == "Player 2") {
            Player2InCombat = true;
            ThreatPlayer2 += threat;
        }

    }
    

    //Remove vida do inimigo, e verifica se ele continua vivo.
    public void TakeDamage(int damage) {

        if(IsVulnerable) {
            HP -= damage;
        }

        if(HP <= 0) {
            isAlive = false;
        }
    }


    //Retorna o Jogador alvo.
    public int SetTarget() {
        
        if(P1Alive && Player1InCombat && (ThreatPlayer1 > ThreatPlayer2)) {
            Target = 1;
        } else if(P2Alive && Player2InCombat && (ThreatPlayer2 > ThreatPlayer1)) {
            Target = 2;
        }

        if(!Player1InCombat && !Player2InCombat) {
            Target = 0;
            inCombat = false;
        }

        return Target;
    }




    /// <summary>
    /// Defini o comportamento em combate de cada inimigo.
    /// </summary>
    public void EnemyBehaviour() {

        if(Name == "Archer") {

            //ao entrar em combate, programa o Attack2 pra daqui a 10s.
            if(inCombat && !CanAttack2) { ReleaseAttack2(); CanAttack2 = true; }

            if(Time.time >= nextAttack2) {
                nextAttack2 = Time.time + Attack2Rate;
                ArcherAttack2 = true;
            }
            

        } else if(Name == "Guard") {

        } else if(Name == "Zombie") {
            
            //Berserker Mode
            if((float)HP/(float)MaxHP <= 0.5f) {
                BerserkerMode = true;
            }

        }
    }
    
    
    /// <summary>
    /// 
    /// </summary>
    private void ReleaseAttack2() {
        nextAttack2 = Time.time + Attack2Rate;
    }


    /// <summary>
    /// 
    /// </summary>
    public void SetEnemyDead() {
        isAlive = false;
    }
    












}
