using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIsland : MonoBehaviour
{


    [Range(0, 3)]
    public float Magnitude;
    [Range(0.5f, 2.5f)]
    public float Frequency;

    private bool FlaotingOn;
    private float startY;



    private void Start()
    {
        startY = this.transform.position.y;
        FlaotingOn = true;
    }


    private void Update()
    {
        Levitating();
    }




    private void Levitating()
    {

        if (FlaotingOn)
        {
            float x = this.transform.position.x;
            float y = startY + Magnitude * Mathf.Sin(Time.time * Frequency);
            float z = this.transform.position.z;

            this.transform.position = new Vector3(x, y, z);
        }
    }





}
