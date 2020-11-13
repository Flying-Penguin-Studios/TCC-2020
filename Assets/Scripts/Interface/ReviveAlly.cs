using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveAlly : MonoBehaviour
{
    public Image percentage;

    // Start is called before the first frame update
    void Start()
    {
        percentage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.gameObject.transform);
    }

    public void ResetPercentage()
    {
        percentage.fillAmount = 0;
    }

    public void FillPercentage(float currentTime, float cooldown)
    {
        percentage.fillAmount = currentTime / cooldown;
        //print(percentage.fillAmount);
    }
}
