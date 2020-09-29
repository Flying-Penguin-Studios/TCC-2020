using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
    public static GameController Singleton;
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            Singleton.ActualScene = gameObject.scene;
            DontDestroyOnLoad(this);
        }
        else
        {
            Singleton.ActualScene = gameObject.scene;
            Destroy(gameObject);
        }
    }

    [Header("Players")]
    public GameObject Player1;
    public bool CriarPlayer1;
    public GameObject Player2;
    public bool CriarPlayer2;

    [Header("HUD")]
    public GameObject HUD;

    [Header("Fade Effects")]
    public GameObject FadeObject;

    [Header("Sound Config")]
    public AudioMixer MixerGeral;
    public float BGM_Volume;
    public float SFX_Volume;

    Scene ActualScene;

    [HideInInspector]
    public GameObject ScenePlayer1;
    [HideInInspector]
    public GameObject ScenePlayer2;

    //Variáveis da Arena - Temporário?
    [HideInInspector]
    public bool Arena1Completed = false;
    [HideInInspector]
    public int ArenaEnemyCount = 0;
    [HideInInspector]
    public bool ArenaCompleted = false;

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadActualScene();
    }

    private void LoadActualScene()
    {
        if (ActualScene.name.ToUpper().Contains("Menu".ToUpper()))
        {
            StartCoroutine(FadeOut(FadeObject));
        }
        else if (ActualScene.name.ToUpper().Contains("Teste".ToUpper()))
        {
            StartCoroutine(FadeOut(FadeObject));
            if (!FindObjectOfType<HudController>())
            {
                Instantiate(HUD);
            }

            StartPlayers();
        }
        else if (ActualScene.name.ToUpper().Contains("Ilhas".ToUpper()))
        {
            StartCoroutine(FadeOut(FadeObject));
            if (!FindObjectOfType<HudController>())
            {
                Instantiate(HUD);
            }

            StartPlayers();
        }
        else
        {
            print("Cena Não Encontrada GAME CONTROLLER IS DEAD");
        }
    }

    void StartPlayers()
    {
        ScenePlayer1 = Instantiate(Player1);
        ScenePlayer2 = Instantiate(Player2);
        ScenePlayer2.GetComponent<PlayerController>().isPlayer2 = true;
        CheckPointController.Singleton.SetPlayers(ScenePlayer1, ScenePlayer2);

        if (CriarPlayer1 && !CriarPlayer2)
        {
            ScenePlayer2.GetComponent<PlayerController>().Test();
        }
        else if (!CriarPlayer1 && CriarPlayer2)
        {
            ScenePlayer1.GetComponent<PlayerController>().Test();
        }
    }

    [Range(.2f, .8f)]
    public float FadeSpeed = .4f;

    IEnumerator FadeOut(GameObject canvas)
    {
        GameObject CloneCanvas = Instantiate(canvas);

        for (float t = 1f; t >= 0; t -= Time.fixedDeltaTime * (FadeSpeed + 0.05f))
        {
            CloneCanvas.GetComponent<Canvas>().GetComponent<CanvasGroup>().alpha = t;
            yield return null;
        }

        Destroy(CloneCanvas);
        yield return null;
    }

    IEnumerator FadeIn(GameObject canvas)
    {
        GameObject CloneCanvas = Instantiate(canvas);

        for (float t = 0f; t <= 1; t = t + Time.fixedDeltaTime * (FadeSpeed + 0.05f))
        {
            CloneCanvas.GetComponent<Canvas>().GetComponent<CanvasGroup>().alpha = t;
            yield return null;
        }

        Destroy(CloneCanvas);
        yield return null;
    }

    #region Controle de Cena

    IEnumerator ILoadScene(string Scene)
    {
        yield return StartCoroutine(FadeIn(FadeObject));
        SceneManager.LoadScene(Scene);
        yield return null;
    }

    IEnumerator ILoadScene(int Scene)
    {
        yield return StartCoroutine(FadeIn(FadeObject));
        SceneManager.LoadScene(Scene);
        yield return null;
    }

    public void LoadScene(string Scene)
    {
        StartCoroutine(ILoadScene(Scene));
    }

    public void LoadScene(int Scene)
    {
        StartCoroutine(ILoadScene(Scene));
    }

    #endregion
}
