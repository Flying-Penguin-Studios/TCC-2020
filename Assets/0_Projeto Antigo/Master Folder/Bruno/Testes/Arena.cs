using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arena : MonoBehaviour
{    
    public List<Wave> Waves = new List<Wave>();


}

[Serializable]
public class Wave
{
    public int WaveTime;
    public List<GameObject> Enemys = new List<GameObject>();
}
