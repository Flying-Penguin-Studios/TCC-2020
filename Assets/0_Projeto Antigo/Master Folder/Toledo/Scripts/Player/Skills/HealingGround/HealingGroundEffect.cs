using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingGroundEffect : MonoBehaviour {


    public int Duration;
    public float HealingFrequency;
    public int HealingPerTick;
    private float NextHealingTick = 0;
    private float WaveRate = 1.25f;
    private float NextWave;

    private Material material;



    private void Start() {
        Destroy(this.gameObject, Duration);
        material = this.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material;
        StartCoroutine("OpeningEffect");
        Invoke("StartFadeOut", 10.5f);
        NextWave = Time.time + 1.75f;
    }



    private void FixedUpdate() {

        if(Time.time >= NextWave) {

            StartCoroutine("Wave");
            NextWave = Time.time + WaveRate;
        }


    }




    /// <summary>
    /// Realiza a cura no jogador, curando "HealingPerTick" a cada "HealingFrequency".
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player")) {
            if(Time.time >= NextHealingTick) {
                other.GetComponent<PlayerMovement>().RecoverHP(HealingPerTick);
                NextHealingTick = Time.time + HealingFrequency;
            }
        }
    }



    /// <summary>
    /// Aparição do circulo de dentro pra fora;
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpeningEffect() {

        float t = 1;
        float k = 0.015f;

        for(float i = 1; i >= 0.5f; i = i - 0.015f) {

            // Ajustes
            t -= i / 325;
            if(i <= 0.55f) {
                k = -0.01f;
            }

            material.SetFloat("_Raio", i + k);
            material.SetFloat("_BorderRadius", t);            

            yield return null;
        }        
    }



    private IEnumerator Wave() {

        for(float i = 1.1f; i >= 0.4f; i = i - 0.015f) {

            material.SetFloat("_RaioOnda", i);

            yield return null;
        }
    }


    private IEnumerator FadeOut() {

        for(float i = 1f; i >= 0; i = i - 0.025f) {

            material.SetFloat("_Alpha", i);

            float t = i - (i*i);

            if(t <= 0) {
                t = 0;
                material.SetFloat("_EmissionPower", t);
            }
            




            yield return null;
        }

    }


    private void StartFadeOut() {
        StartCoroutine("FadeOut");
    }






}
