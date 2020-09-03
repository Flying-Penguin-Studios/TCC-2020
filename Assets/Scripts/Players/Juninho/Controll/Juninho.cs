﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Juninho : PlayerController
{
    [SerializeField]
    bool isPlayer2 = true;

    protected override void Start()
    {
        base.Start();
        stats = new playerStats(150, 7f, 8f, 8f, 12f);

        HUD = GameObject.Find("HUD/Player1").GetComponent<HUD_Player>();
        Dash = GetComponent<Juninho_Dash>();
        Sup = GetComponent<Shield>();
        Control = GetComponent<Shout>();
        Damage = GetComponent<QuakePunch>();

        HUD.Init(this);
        Dash.Init(GameObject.Find("HUD/Player1/Dash").GetComponent<HUD_Dash>(), this, "Dash");
        Sup.Init(GameObject.Find("HUD/Player1/SkillLT").GetComponent<HUD_Skill>(), this, "Shield");
        Control.Init(GameObject.Find("HUD/Player1/SkillY").GetComponent<HUD_Skill>(), this, "Shout");
        Damage.Init(GameObject.Find("HUD/Player1/SkillB").GetComponent<HUD_Skill>(), this, "QPunch");

        MarkIndicator = this.transform.GetChild(2).gameObject;

        //SetParter(FindObjectOfType<Angie>());
    }

    void Update()
    {
        if (GameController.Singleton.GamePaused) { return; }

        //Cheats();

        if (ToVivo)
        {
            OnGround();
            DustControl();

            Distance = Vector3.Distance(transform.position, Parter.transform.position);

            if (!isPlayer2)
            {
                Atack("P1_X");
                GetInput(Input.GetAxis("P1_L_Joystick_Vertical"), Input.GetAxis("P1_L_Joystick_Horizontal"));
                UseDash(Input.GetAxis("P1_RT"));
                if (!GetBool("Charging"))
                {
                    if (Parter.ToVivo)
                    {
                        if (Parter.GetBool("Fallen") && Distance <= 2f)
                        {
                            ReviveFriend("P1_A");
                        }
                        else
                        {
                            Jump(Input.GetButtonDown("P1_A"));
                            ResetRevive();
                        }
                    }
                    else
                    {
                        Jump(Input.GetButtonDown("P1_A"));
                        ResetRevive();
                    }


                    ShieldUP(this.Sup, Input.GetAxis("P1_LT"));
                    UseSkill(this.Control, Input.GetButtonDown("P1_Y"));
                    UseSkill(this.Damage, Input.GetButtonDown("P1_B"));
                }
            }
            else
            {
                Atack("P2_X");
                GetInput(Input.GetAxis("P2_L_Joystick_Vertical"), Input.GetAxis("P2_L_Joystick_Horizontal"));
                UseDash(Input.GetAxis("P2_RT"));
                if (!GetBool("Charging"))
                {
                    if (Parter.ToVivo)
                    {
                        if (Parter.GetBool("Fallen") && Distance <= 2f)
                        {
                            ReviveFriend("P2_A");
                        }
                        else
                        {
                            Jump(Input.GetButtonDown("P2_A"));
                        }
                    }
                    else
                    {
                        Jump(Input.GetButtonDown("P2_A"));
                        ResetRevive();
                    }

                    ShieldUP(this.Sup, Input.GetAxis("P2_LT"));
                    UseSkill(this.Control, Input.GetButtonDown("P2_Y"));
                    UseSkill(this.Damage, Input.GetButtonDown("P2_B"));
                }
            }
        }
    }

    public void NewMass(float val = 1f)
    {
        rb.mass = val;
    }

    bool CanTrigger = true;

    public void ShieldUP(Skill Skill, float Trigger)
    {
        if (Skill.IsAvaliable() && Trigger == 1 && OnGround() && CanTrigger && GetBool("canCast"))
        {
            CanTrigger = false;
            SetTrigger(Skill.StringTrigger);
        }

        if (OnGround() && GetBool("ShieldUP"))
        {
            if (Trigger == 0)
            {
                CanTrigger = true;
                SetBool("ShieldUP", false);
            }
        }
    }
}
