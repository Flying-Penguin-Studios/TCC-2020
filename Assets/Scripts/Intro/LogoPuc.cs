using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoPuc : MonoBehaviour
{


    public float LogoDuration;
    private CanvasGroup Logo;
    public GameObject test;


    private void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.visible = false;
        Logo = this.GetComponent<CanvasGroup>();
        StartCoroutine(ShowHideLogo());
    }

    private void Update()
    {
        if ((Input.anyKey) || (Input.GetButton("Submit") || Input.GetButton("Submit Joy") || Input.GetButton("Cancel") || Input.GetButton("Cancel Joy")))
        {
            //print("teste");
            StopAllCoroutines();
            SceneManager.LoadScene("1_Menu");
        }

    }

    private IEnumerator ShowHideLogo()
    {

        yield return new WaitForSeconds(1);

        for (float i = 0; i <= 1; i = i + Time.deltaTime * 0.4f)
        {
            Logo.alpha = i;
            yield return null;
        }

        yield return new WaitForSeconds(LogoDuration);

        for (float i = 1; i >= -0.5f; i = i - Time.deltaTime * 0.4f)
        {
            Logo.alpha = i;
            yield return null;
        }

        test.SetActive(false);

        yield return new WaitForSeconds(1f);

        //SceneManager.LoadScene(1);

    }
}
