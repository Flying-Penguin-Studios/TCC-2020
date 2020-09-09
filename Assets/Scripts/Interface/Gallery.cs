using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery : MonoBehaviour
{
    public GameObject item;
    bool pressedL;
    bool pressedR;

    void FixedUpdate()
    {
        if (pressedL)
        {
            RotateLeft();
        }
        if (pressedR)
        {
            RotateRight();
        }
    }

    private void RotateLeft()
    {
        item.transform.Rotate(0 , 2.5f, 0);
    }
    private void RotateRight()
    {
        item.transform.Rotate(0, -2.5f, 0);
    }

    public void LeftPressed()
    {
        pressedL = true;
    }
    public void LeftRelease()
    {
        pressedL = false;
    }
    public void RightPressed()
    {
        pressedR = true;
    }
    public void RightRelease()
    {
        pressedR = false;
    }

}
