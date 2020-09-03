using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CloseOnB : MonoBehaviour
{
    EventSystem EventSystem;
    [SerializeField]
    Button last;

    private void OnEnable()
    {
        EventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Cancel Joy"))
        {
            if(last)
            {
                last.Select();
                EventSystem.SetSelectedGameObject(last.gameObject);
            }
            gameObject.SetActive(false);
        }
    }
}
