using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArena_VFX : MonoBehaviour {



    public GameObject vfx;



    private void Update() {
        
        if(Input.GetKeyDown(KeyCode.B)) {
            vfx.SetActive(true);
        }
    }







}
