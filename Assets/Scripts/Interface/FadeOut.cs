using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    [Range(.2f, .8f)]
    public float FadeSpeed = .4f;

    void Start()
    {
        StartCoroutine(CanvasFadeOut(GetComponent<Canvas>()));
    }

    public void Fade()
    {
        StartCoroutine(CanvasFadeOut(GetComponent<Canvas>()));
    }

    IEnumerator CanvasFadeOut(Canvas canvas)
    {
        for (float t = 1f; t >= 0; t -= Time.fixedDeltaTime * (FadeSpeed + 0.05f))
        {
            canvas.GetComponent<CanvasGroup>().alpha = t;
            yield return null;
        }

        Destroy(gameObject);
    }
}
