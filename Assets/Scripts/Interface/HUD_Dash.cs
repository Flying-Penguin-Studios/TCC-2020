using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HUD_Dash : HUD_Skill
{
    [HideInInspector]
    public int CountDash;

    public Text SlotText;
    public Image SlotCD;

    public override void setCD(float current_time, float skill_cd = 0)
    {
        if (current_time <= 0)
        {
            i_CD_Skill.fillAmount = 0;
        }
        else
        {
            i_CD_Skill.fillAmount = current_time / skill_cd;
        }
    }

    public void setSlotCount(int val)
    {
        CountDash = val;
        SlotText.text = CountDash.ToString();
    }

    public void setSlotCD(float current_time, float skill_cd = 0)
    {
        if (current_time <= 0)
        {
            SlotCD.fillAmount = 0;
        }
        else
        {
            SlotCD.fillAmount = current_time / skill_cd;
        }
    }
}
