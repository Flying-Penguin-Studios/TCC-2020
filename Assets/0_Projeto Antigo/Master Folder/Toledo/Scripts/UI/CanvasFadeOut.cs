using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFadeOut : MonoBehaviour {


    public float FadeSpeed;
    private Canvas BlackCanvas;

       

    private void Awake() {
        BlackCanvas = this.gameObject.GetComponent<Canvas>();
        BlackCanvas.GetComponent<CanvasGroup>().alpha = 1;
    }



    private void FixedUpdate() {
        StartCoroutine(BlackCanvasFadeOut(BlackCanvas));
    }



    IEnumerator BlackCanvasFadeOut(Canvas canvas) {

        for(var t = 1f; t >= 0; t -= Time.deltaTime * FadeSpeed) {
            canvas.GetComponent<CanvasGroup>().alpha = t;
            yield return null;
        }

    }


    IEnumerator BlackCanvasFadeIn(Canvas canvas) {

        for(var t = 1f; t >= 0; t -= Time.deltaTime * FadeSpeed) {
            canvas.GetComponent<CanvasGroup>().alpha = t;
            yield return null;
        }

    }


}
