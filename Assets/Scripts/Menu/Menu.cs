using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{


    // ===== Geral ========================================
    private bool GameStarted = true;
    public Texture2D MouseImage;
    public Button StartButton;
    public Sprite ButtonBase;

    bool PanelActive = false;
    GameObject CurrentButton;
    Button LastButton;

    EventSystem EventSystem;


    // ===== Audio Config ========================================
    [Space(20)]
    public AudioMixer MixerGeral;
    public AudioSource BGM_AudioSource;
    public AudioSource SFX_AudioSource;
    public AudioClip StartSFX;
    public AudioClip ButtonMouseOverSFX;
    public AudioClip ButtonClick;
    [Space(20)]
    public Slider BGM_Slider;
    public Slider SFX_Slider;


    // ===== Resolução ========================================
    private List<Vector2> resolutionsList = new List<Vector2>();
    private int ResolutionIndex;
    public Text resol;


    Resolution[] resolutions;
    [Space(20)]
    private int currentResolutionValue = 0;
    private int listTotal;




    private void OnEnable()
    {
        EventSystem = EventSystem.current;
    }


    private void Start()
    {
        HotSpot = new Vector2(Screen.width, Screen.height);
        SetCursor();
        MixerGeral = GameController.Singleton.MixerGeral;
        SetVolume();
        SetResolution();
        SelectButton();
    }


    private void SetCursor()
    {
        Cursor.SetCursor(MouseImage, HotSpot, CursorMode.Auto);
        Cursor.visible = false;
    }


    private void SelectButton()
    {
        EventSystem.SetSelectedGameObject(null);
        EventSystem.SetSelectedGameObject(EventSystem.firstSelectedGameObject);
        StartButton.Select();
    }


    public void StartGame()
    {
        if (GameStarted)
        {
            LoadGame();
        }
    }


    private void LoadGame()
    {
        GameController.Singleton.LoadScene("Ilhas");
    }

    public void GoToGallery()
    {
        GameController.Singleton.LoadScene("3_Galeria");
    }
    private void LoadGallery()
    {
        SceneManager.LoadScene(3);
    }



    // ===== Controle de volume ======================================================

    public void SetVolumeBGM(float volume)
    {
        MixerGeral.SetFloat("VolumeTrilha", volume);
        GameController.Singleton.BGM_Volume = volume;
    }


    public void SetVolumeSFX(float volume)
    {
        MixerGeral.SetFloat("VolumeEffects", volume);
        GameController.Singleton.SFX_Volume = volume;
    }


    public void HighlightedSFX()
    {
        SFX_AudioSource.PlayOneShot(ButtonMouseOverSFX);
    }

    public void StartGameSFX()
    {
        SFX_AudioSource.PlayOneShot(StartSFX);
    }


    private void SetVolume()
    {

        float BGM_Volume = GameController.Singleton.BGM_Volume;
        float SFX_Volume = GameController.Singleton.SFX_Volume;

        BGM_Slider.value = BGM_Volume;
        SFX_Slider.value = SFX_Volume;
    }



    // ===== Controle de Resolução ======================================================

    public void SetResolution()
    {

        resolutionsList = new List<Vector2>() {

                                                    new Vector2(Screen.width, Screen.height),
                                                    new Vector2(640, 480),
                                                    new Vector2(800, 600),
                                                    new Vector2(854, 480),
                                                    new Vector2(1024, 768),
                                                    new Vector2(1152, 864),
                                                    new Vector2(1176, 664),
                                                    new Vector2(1280, 720),
                                                    new Vector2(1280, 960),
                                                    new Vector2(1280, 800),
                                                    new Vector2(1360, 768),
                                                    new Vector2(1680, 1150),
                                                    new Vector2(1920, 1080),
                                                    new Vector2(2560, 1440),
                                                    new Vector2(3840, 2160)
        };

        //ResolutionIndex = 2;
        //Screen.SetResolution((int)resolutionsList[ResolutionIndex].x, (int)resolutionsList[ResolutionIndex].y, true);
        resol.text = resolutionsList[ResolutionIndex].x.ToString() + " x " + resolutionsList[ResolutionIndex].y.ToString();
    }


    public void NextResolution()
    {

        if (ResolutionIndex != (resolutionsList.Count - 1))
        {
            ResolutionIndex++;
            Screen.SetResolution((int)resolutionsList[ResolutionIndex].x, (int)resolutionsList[ResolutionIndex].y, true);
            resol.text = resolutionsList[ResolutionIndex].x.ToString() + " x " + resolutionsList[ResolutionIndex].y.ToString();
        }
        else
        {
            ResolutionIndex = 0;
            Screen.SetResolution((int)resolutionsList[ResolutionIndex].x, (int)resolutionsList[ResolutionIndex].y, true);
            resol.text = resolutionsList[ResolutionIndex].x.ToString() + " x " + resolutionsList[ResolutionIndex].y.ToString();
        }
    }


    public void PreviousResolution()
    {

        if (ResolutionIndex != 0)
        {

            ResolutionIndex--;
            Screen.SetResolution((int)resolutionsList[ResolutionIndex].x, (int)resolutionsList[ResolutionIndex].y, true);
            resol.text = resolutionsList[ResolutionIndex].x.ToString() + " x " + resolutionsList[ResolutionIndex].y.ToString();
        }
        else
        {
            ResolutionIndex = resolutionsList.Count - 1;
            Screen.SetResolution((int)resolutionsList[ResolutionIndex].x, (int)resolutionsList[ResolutionIndex].y, true);
            resol.text = resolutionsList[ResolutionIndex].x.ToString() + " x " + resolutionsList[ResolutionIndex].y.ToString();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


    // ===== Exit ======================================================
    public void ClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                              Application.Quit();
#endif
    }

    Vector3 lastMouseCoordinate = Vector3.zero;
    Vector2 HotSpot;
    void Update()
    {
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartButton.Select();
        }

        //if (!Cursor.visible)
        //{
        if (mouseDelta.x < 0)
        {
            //LastButton = EventSystem.currentSelectedGameObject;
            Cursor.SetCursor(MouseImage, Vector2.zero, CursorMode.Auto);
            Cursor.visible = true;
            EventSystem.firstSelectedGameObject = EventSystem.currentSelectedGameObject;
            EventSystem.SetSelectedGameObject(null);
        }
        //}

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
                    StartButton.Select();

                EventSystem.firstSelectedGameObject = EventSystem.currentSelectedGameObject;
            }
        }

        lastMouseCoordinate = Input.mousePosition;
    }

    public void ChangeOrder()
    {
        GameController.Singleton.ChangePlayers();
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
    /*
    private void SetResolution1() {


        resolutions = Screen.resolutions;

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);            

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        listTotal = options.Count - 1;
        currentResolutionValue = currentResolutionIndex;
        resol.text = options[currentResolutionIndex].ToString();
    }


    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    
    public void NextResolution1() {

        if(currentResolutionValue != listTotal) {
            currentResolutionValue++;
            Resolution resolution = resolutions[currentResolutionValue];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            resol.text = resolution.width + " x " + resolution.height;
        } else {
            currentResolutionValue = 0;
            Resolution resolution = resolutions[currentResolutionValue];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            resol.text = resolution.width + " x " + resolution.height;
        }

    }


    public void PreviousResolution1() {

        if(currentResolutionValue != 0) {
            currentResolutionValue--;
            Resolution resolution = resolutions[currentResolutionValue];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            resol.text = resolution.width + " x " + resolution.height;
        } else {
            currentResolutionValue = listTotal;
            Resolution resolution = resolutions[currentResolutionValue];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            resol.text = resolution.width + " x " + resolution.height;
        }
    }
    */
}