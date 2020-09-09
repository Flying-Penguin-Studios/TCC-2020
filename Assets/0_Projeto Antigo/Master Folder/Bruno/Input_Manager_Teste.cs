using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager_Teste : MonoBehaviour
{
    #region Singleton

    public static Input_Manager_Teste Singleton;

    private void Awake()
    {
        if (Singleton is null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public string UP = "W";
    public string DOWN = "S";
    public string LEFT = "A";
    public string RIGHT = "D";
    public string JUMP = "Space";
    public string ATACK = "P";

    public Dictionary<string, string> input_dic = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        input_dic.Add(UP.ToUpper(), "P1_L_Joystick_Horizontal");
        input_dic.Add(DOWN.ToUpper(), "P1_L_Joystick_Horizontal");
        input_dic.Add(RIGHT.ToUpper(), "P1_L_Joystick_Vertical");
        input_dic.Add(DOWN.ToUpper(), "P1_L_Joystick_Vertical");
        input_dic.Add(JUMP.ToUpper(), "P1_A");
        input_dic.Add(ATACK.ToUpper(), "P1_X");
    }

    // Update is called once per frame
    void Update()
    {
        //string input = Input.inputString.ToUpper();
    }

    public string getKey(string input)
    {
        if (input_dic.ContainsKey(input))
        {
            return input_dic[input];
        }

        return "";
    }
}
