using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [Range(.2f, .8f)]
    public float FadeSpeed = .4f;

    void Start()
    {
        StartCoroutine(CanvasFadeIn(GetComponent<Canvas>()));
    }
    public void Fade()
    {
        StartCoroutine(CanvasFadeIn(GetComponent<Canvas>()));
    }

    IEnumerator CanvasFadeIn(Canvas canvas)
    {
        for (float t = 0f; t <= 1; t = t + Time.deltaTime * (FadeSpeed + 0.05f))
        {
            canvas.GetComponent<CanvasGroup>().alpha = t;
            yield return null;
        }

        Destroy(gameObject);
    }
}
