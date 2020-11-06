using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;


public class HUD : MonoBehaviour
{


    // ===== Geral ========================================
    private bool GamePause = false;
    private bool GameStarted = true;


    // ===== Audio Config =================================

    [SerializeField]
    [Header("HUD Components")]
    public Button InitialButton;
    public GameObject PausePanel;
    public AudioMixer MixerGeral;
    public AudioSource SFX_AudioSource;
    public AudioClip ButtonMouseOverSFX;
    public Sprite ButtonBase;
    public Button Options;
    public Button Menu;
    public Button Back;

    bool PanelActive = false;

    [HideInInspector]
    public Slider BGM_Slider;
    [HideInInspector]
    public Slider SFX_Slider;






    private void Start()
    {
        EventSystem = EventSystem.current;
        SetVolume();
    }

    Vector3 lastMouseCoordinate = Vector3.zero;
    Button LastButton;
    EventSystem EventSystem;
    Vector2 HotSpot = new Vector2(Screen.width, Screen.height);
    public Texture2D MouseImage;

    void onPause()
    {
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;

        if (Input.GetKeyDown(KeyCode.K))
        {
            InitialButton.Select();
        }

        if (mouseDelta != Vector3.zero)
        {
            //LastButton = EventSystem.currentSelectedGameObject;
            Cursor.SetCursor(MouseImage, Vector2.zero, CursorMode.Auto);
            Cursor.visible = true;
            EventSystem.firstSelectedGameObject = EventSystem.currentSelectedGameObject;
            EventSystem.SetSelectedGameObject(null);
        }

        if (Cursor.visible)
        {
            if (Input.GetAxis("P1_L_Joystick_Vertical") != 0 ||
                Input.GetAxis("P1_L_Joystick_Horizontal") != 0 ||
                Input.GetButtonDown("Vertical") ||
                Input.GetAxisRaw("Vertical Joy") != 0)
            {
                Cursor.visible = false;
                Cursor.SetCursor(null, HotSpot, CursorMode.Auto);

                if (PanelActive)
                    LastButton.GetComponent<Button>().Select();
                else
                    InitialButton.Select();

                EventSystem.firstSelectedGameObject = EventSystem.currentSelectedGameObject;
            }
        }

        EventSystem.firstSelectedGameObject = EventSystem.currentSelectedGameObject;
        lastMouseCoordinate = Input.mousePosition;
    }

    private void Update()
    {
        PauseGame();
        if (GamePause)
        {
            onPause();
        }
    }

    public void BackToMenu()
    {
        GameController.Singleton.LoadScene("1_Menu");
    }

    public void PauseGame()
    {
        if (Input.GetButtonDown("P1_Start") || Input.GetButtonDown("P2_Start"))
        {
            GamePause = !GamePause;
            PausePanel.SetActive(GamePause);
            Cursor.SetCursor(null, HotSpot, CursorMode.Auto);
            Cursor.visible = false;
            Time.timeScale = GamePause ? 0 : 1;

            try
            {
                GameObject.Find("Options Panel").gameObject.SetActive(false);
                GameObject.Find("Menu Panel").gameObject.SetActive(false);
            }
            catch { }

            if (GamePause)
            {
                EventSystem.SetSelectedGameObject(null);
                EventSystem.SetSelectedGameObject(EventSystem.firstSelectedGameObject);
                InitialButton.Select();
            }

            if (!GamePause)
            {
                FixButtonAnimation(InitialButton);
                FixButtonAnimation(Options);
                FixButtonAnimation(Menu);
            }
            //if (!GamePause)
            //    Invoke("PauseSingle", 0.1f);
            //else
            //    GameController_OLD.Singleton.GamePaused = GamePause;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePause = !GamePause;
            PausePanel.SetActive(GamePause);
            Cursor.visible = GamePause;
            //Cursor.SetCursor(MouseImage, Vector2.zero, CursorMode.Auto);
            Time.timeScale = GamePause ? 0 : 1;

            try
            {
                GameObject.Find("Options Panel").gameObject.SetActive(false);
                GameObject.Find("Menu Panel").gameObject.SetActive(false);
            }
            catch { }

            if (GamePause)
            {
                EventSystem.SetSelectedGameObject(null);
                //EventSystem.SetSelectedGameObject(EventSystem.firstSelectedGameObject);
                //InitialButton.Select();
            }

            if (!GamePause)
            {
                FixButtonAnimation(InitialButton);
                FixButtonAnimation(Options);
                FixButtonAnimation(Menu);
                FixButtonAnimation(Back);
            }

            //if (!GamePause)
            //    Invoke("PauseSingle", 0.1f);
            //else
            //    GameController_OLD.Singleton.GamePaused = GamePause;
        }
    }

    //public void PauseSingle()
    //{
    //    GameController_OLD.Singleton.GamePaused = GamePause;
    //}

    public void ResumeGame()
    {
        GamePause = false;
        PausePanel.SetActive(GamePause);
        Cursor.visible = GamePause;
        Time.timeScale = GamePause ? 0 : 1;
        //Invoke("PauseSingle", 0.1f);
        try
        {
            GameObject.Find("Options Panel").gameObject.SetActive(false);
            GameObject.Find("Menu Panel").gameObject.SetActive(false);
        }
        catch { }
    }


    public void SetVolumeBGM(float volume)
    {
        MixerGeral.SetFloat("VolumeTrilha", volume);
        //GameController_OLD.Singleton.BGM_Volume = volume;
    }


    public void SetVolumeSFX(float volume)
    {
        MixerGeral.SetFloat("VolumeEffects", volume);
        //GameController_OLD.Singleton.SFX_Volume = volume;
    }


    public void HighlightedSFX()
    {
        SFX_AudioSource.PlayOneShot(ButtonMouseOverSFX);
    }


    private void SetVolume()
    {

        //float BGM_Volume = GameController_OLD.Singleton.BGM_Volume;
        //float SFX_Volume = GameController_OLD.Singleton.SFX_Volume;

        //BGM_Slider.value = BGM_Volume;
        //SFX_Slider.value = SFX_Volume;
    }

    public void PanelIsOpen(Button firstSelected)
    {
        PanelActive = true;
        LastButton = firstSelected;
    }

    public void PanelClose()
    {
        PanelActive = false;
    }

    public void FixButtonAnimation(Button b)
    {
        b.GetComponent<Image>().color = new Color(0.8018868f, 0.7602795f, 0.7602795f);
        b.GetComponent<Image>().sprite = ButtonBase;
    }




}
