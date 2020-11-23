using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HUD_Teste : MonoBehaviour
{
    [SerializeField]
    [Header("HUD Components")]
    private GameObject PausePanel;
    bool GamePause = false;

    public void LoadScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetButtonDown("P1_Start") ||
            Input.GetButtonDown("P2_Start") ||
            Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        GamePause = !GamePause;
        Time.timeScale = GamePause ? 0 : 1;
        PausePanel.SetActive(GamePause);
    }
}
