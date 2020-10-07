﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaController : MonoBehaviour
{
    public static ArenaController Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    //void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.ToUpper().Contains("Ilha".ToUpper()) && !scene.name.ToUpper().Contains("Teste".ToUpper()))
        {
            Destroy(gameObject);
        }

        if (d_ArenaCompleted.Count > 0)
        {
            Arena[] arenas = FindObjectsOfType<Arena>();

            foreach (Arena Arena in arenas)
            {
                if (d_ArenaCompleted[Arena.gameObject.name])
                {
                    Destroy(Arena.gameObject);
                }
            }
        }
    }

    List<string> Arenas = new List<string>();
    Dictionary<string, bool> d_ArenaCompleted = new Dictionary<string, bool>();

    void Start()
    {
        Arena[] arenas = FindObjectsOfType<Arena>();

        for (int i = 0; i < arenas.Length - 1; i++)
        {
            d_ArenaCompleted.Add(arenas[i].gameObject.name, false);
        }
    }

    public void ArenaCompleted(string Arena_Name)
    {
        d_ArenaCompleted[Arena_Name] = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene("Teste_Controllers1");
        }
    }
}