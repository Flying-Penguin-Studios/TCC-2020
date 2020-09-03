using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HUD_Player : MonoBehaviour
{
    [Header("HP")]
    public Image i_HP;
    public Text t_HP;
    [Header("Dash CD")]
    public Image i_CD_Dash;
    [Header("CD Skill X")]
    public Image i_CD_Skill_X;
    public Text t_CD_Skill_X;
    [Header("CD Skill Y")]
    public Image i_CD_Skill_Y;
    public Text t_CD_Skill_Y;
    [Header("CD Skill B")]
    public Image i_CD_Skill_B;
    public Text t_CD_Skill_B;

    public void Init(PlayerController n_player)
    {
        setLife(n_player.getStats().currentLife, n_player.getStats().maxLife);
    }

    public void setLife(float currentLife, float maxLife)
    {
        t_HP.text = currentLife + "/" + maxLife;
        i_HP.fillAmount = currentLife / maxLife;
    }

    public void setCD_Skill_X(float current_time, float skill_cd = 0)
    {
        if (current_time <= 0)
        {
            i_CD_Skill_X.fillAmount = 0;
            t_CD_Skill_X.text = "";
        }
        else
        {
            i_CD_Skill_X.fillAmount = current_time / skill_cd;
            //t_CD_Skill_X.text = ((int)current_time).ToString();
            t_CD_Skill_X.text = Mathf.RoundToInt(current_time).ToString();
            //t_CD_Skill_X.text = Mathf.CeilToInt(current_time).ToString();
        }
    }
}
