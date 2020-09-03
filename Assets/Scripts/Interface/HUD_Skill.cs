using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HUD_Skill : MonoBehaviour
{
    public Image i_CD_Skill;
    public Image BlackGroundCircle;
    public Text t_CD_Skill;

    public virtual void setCD(float current_time, float skill_cd = 0)
    {
        if (current_time <= 0)
        {
            BlackGroundCircle.enabled = false;
            i_CD_Skill.fillAmount = 0;
            t_CD_Skill.text = "";
        }
        else
        {
            BlackGroundCircle.enabled = true;
            i_CD_Skill.fillAmount = current_time / skill_cd;
            t_CD_Skill.text = Mathf.RoundToInt(current_time).ToString();
        }
    }

    public virtual void WaitCD()
    {
        i_CD_Skill.fillAmount = 1;
        t_CD_Skill.text = "";
    }
}
