using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaLineFadeIN : MonoBehaviour {


    private GameObject Line1;
    private GameObject Line2;
    private GameObject Line3;
    private GameObject Line4;
    private GameObject Line5;
    private GameObject Line6;
    private GameObject Line7;
    private GameObject Line8;

    public float speed;



    private void Start() {

        Line1 = transform.GetChild(8).gameObject;
        Line2 = transform.GetChild(9).gameObject;
        Line3 = transform.GetChild(10).gameObject;
        Line4 = transform.GetChild(11).gameObject;
        Line5 = transform.GetChild(12).gameObject;
        Line6 = transform.GetChild(13).gameObject;
        Line7 = transform.GetChild(14).gameObject;
        Line8 = transform.GetChild(15).gameObject;


        StartCoroutine(Showlines());

    }


  


    private IEnumerator Showlines() {

        yield return new WaitForSeconds(2);

        for(float i = 0; i <= 1; i += speed * Time.deltaTime) {

            Line1.GetComponent<LineRenderer>().startColor = new Vector4(Line1.GetComponent<LineRenderer>().startColor.r, Line1.GetComponent<LineRenderer>().startColor.g, Line1.GetComponent<LineRenderer>().startColor.b, i);
            Line2.GetComponent<LineRenderer>().startColor = new Vector4(Line2.GetComponent<LineRenderer>().startColor.r, Line2.GetComponent<LineRenderer>().startColor.g, Line2.GetComponent<LineRenderer>().startColor.b, i);
            Line3.GetComponent<LineRenderer>().startColor = new Vector4(Line3.GetComponent<LineRenderer>().startColor.r, Line3.GetComponent<LineRenderer>().startColor.g, Line3.GetComponent<LineRenderer>().startColor.b, i);
            Line4.GetComponent<LineRenderer>().startColor = new Vector4(Line4.GetComponent<LineRenderer>().startColor.r, Line4.GetComponent<LineRenderer>().startColor.g, Line4.GetComponent<LineRenderer>().startColor.b, i);
            Line5.GetComponent<LineRenderer>().startColor = new Vector4(Line5.GetComponent<LineRenderer>().startColor.r, Line5.GetComponent<LineRenderer>().startColor.g, Line5.GetComponent<LineRenderer>().startColor.b, i);
            Line6.GetComponent<LineRenderer>().startColor = new Vector4(Line6.GetComponent<LineRenderer>().startColor.r, Line6.GetComponent<LineRenderer>().startColor.g, Line6.GetComponent<LineRenderer>().startColor.b, i);
            Line7.GetComponent<LineRenderer>().startColor = new Vector4(Line7.GetComponent<LineRenderer>().startColor.r, Line7.GetComponent<LineRenderer>().startColor.g, Line7.GetComponent<LineRenderer>().startColor.b, i);
            Line8.GetComponent<LineRenderer>().startColor = new Vector4(Line8.GetComponent<LineRenderer>().startColor.r, Line8.GetComponent<LineRenderer>().startColor.g, Line8.GetComponent<LineRenderer>().startColor.b, i);


            Line1.GetComponent<LineRenderer>().endColor = new Vector4(Line1.GetComponent<LineRenderer>().startColor.r, Line1.GetComponent<LineRenderer>().startColor.g, Line1.GetComponent<LineRenderer>().startColor.b, i);
            Line2.GetComponent<LineRenderer>().endColor = new Vector4(Line2.GetComponent<LineRenderer>().startColor.r, Line2.GetComponent<LineRenderer>().startColor.g, Line2.GetComponent<LineRenderer>().startColor.b, i);
            Line3.GetComponent<LineRenderer>().endColor = new Vector4(Line3.GetComponent<LineRenderer>().startColor.r, Line3.GetComponent<LineRenderer>().startColor.g, Line3.GetComponent<LineRenderer>().startColor.b, i);
            Line4.GetComponent<LineRenderer>().endColor = new Vector4(Line4.GetComponent<LineRenderer>().startColor.r, Line4.GetComponent<LineRenderer>().startColor.g, Line4.GetComponent<LineRenderer>().startColor.b, i);
            Line5.GetComponent<LineRenderer>().endColor = new Vector4(Line5.GetComponent<LineRenderer>().startColor.r, Line5.GetComponent<LineRenderer>().startColor.g, Line5.GetComponent<LineRenderer>().startColor.b, i);
            Line6.GetComponent<LineRenderer>().endColor = new Vector4(Line6.GetComponent<LineRenderer>().startColor.r, Line6.GetComponent<LineRenderer>().startColor.g, Line6.GetComponent<LineRenderer>().startColor.b, i);
            Line7.GetComponent<LineRenderer>().endColor = new Vector4(Line7.GetComponent<LineRenderer>().startColor.r, Line7.GetComponent<LineRenderer>().startColor.g, Line7.GetComponent<LineRenderer>().startColor.b, i);
            Line8.GetComponent<LineRenderer>().endColor = new Vector4(Line8.GetComponent<LineRenderer>().startColor.r, Line8.GetComponent<LineRenderer>().startColor.g, Line8.GetComponent<LineRenderer>().startColor.b, i);


            yield return null;

        }
    }







}
