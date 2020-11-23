using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {


    public int id_prooximo = 0;

    public Transform[] next;



    public Transform getNext() {
        int aux = id_prooximo;
        return next[aux];
    }
    

}
