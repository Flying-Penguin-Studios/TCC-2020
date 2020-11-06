using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Gallery : MonoBehaviour
{
    public GameObject item;
    public Button backButton;
    public Camera cam;
    bool pressedL;
    bool pressedR;
    public float speedCursor = 0.025f;
    public float speedJoystick = 2.5f;
    bool canRotate = false;
    public Sprite ButtonBase;
    bool PanelActive = false;
    EventSystem EventSystem;
    public Texture2D MouseImage;
    public Button StartButton;
    Button LastButton;


    private void Start()
    {
        HotSpot = new Vector2(Screen.width, Screen.height);
        SetCursor();
        SelectButton();
    }

    private void OnEnable()
    {
        EventSystem = EventSystem.current;
    }

    void FixedUpdate()
    {
        //Rotação feita ao pressionar os botões da HUD. Ambos bool estão relacionados a funções "LeftPress" e "RightPress" abaixo. 
        if (pressedL) 
            RotateLeft();
        if (pressedR)
            RotateRight();

        if (canRotate) //Checa se um objeto da galeria foi selecionado, para assim poder rotacionar. Para não haver mistura entre rotação com cursor e rotação com botões da HUD, a rotação por botão HUD está fora desse if.
        {
            //Rotação com cursor
            if (Input.GetMouseButton(0))
            {
                CursorRotate();
            }

            //Rotação com controle pressionando RB e LB (TESTAR BRUNO)
            if (Input.GetButton("P1_LB") || Input.GetButton("P2_LB"))
            {
                RotateLeft();
            }
            if (Input.GetButton("P1_RB") || Input.GetButton("P2_RB"))
            {
                RotateRight();
            }

            //Rotação com controle usando Joystick Direito (ME AJUDA NESSE BRUNO)
            //JoysticRotate();

            if (Input.GetAxis("P1_R_Joystick_Vertical") > 0 || Input.GetAxis("P2_R_Joystick_Vertical") > 0)
            {
                ZoomInCamera();
            }

            if (Input.GetAxis("P1_R_Joystick_Vertical") < 0 || Input.GetAxis("P2_R_Joystick_Vertical") < 0)
            {
                ZoomOutCamera();
            }
        }
        
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

    private void RotateLeft()
    {
        item.transform.Rotate(0 , 2.5f, 0);
    }
    private void RotateRight()
    {
        item.transform.Rotate(0, -2.5f, 0);
    }

    //Funções usadas nos botões da HUD "RotateLeft" e "RotateRight"
    public void LeftPress()
    {
        pressedL = !pressedL;
        CanRotate();
    }
    public void RightPress()
    {
        pressedR = !pressedR;
        CanRotate();
    }
    //

    //Função usada no botão que seleciona um objeto na galeria.
    public void CanRotate()
    {
        canRotate = !canRotate;
    }

    void CursorRotate()
    {
        float xRotation = (Input.mousePosition.x - (Screen.width/2)) * speedCursor;
        item.transform.Rotate(Vector3.down, xRotation);
    }

    void JoysticRotate() //Completar com nome do joystick right (TESTAR BRUNO)
    {
        float xRotation = (Input.GetAxis("")) * speedJoystick;
        item.transform.Rotate(Vector3.down, xRotation);
    }

    public void BackToMenu()
    {
        GameController.Singleton.LoadScene("1_Menu");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("1_Menu");
    }

    public void DeactivateAllObjects()
    {

        for(int i = 0; i <= item.transform.childCount - 1; i++)
        {
            item.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void DeactivateAllPanels(GameObject itempanel)
    {

        for (int i = 0; i <= itempanel.transform.childCount - 1; i++)
        {
            itempanel.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ZoomInCamera()
    {
        if(cam.fieldOfView > 15)
        {
            cam.fieldOfView = cam.fieldOfView - 1;
        } 
    }
    public void ZoomOutCamera()
    {
        if (cam.fieldOfView < 80)
        {
            cam.fieldOfView = cam.fieldOfView + 1;
        }
    }

    public void ResetZoomCamera()
    {
        cam.fieldOfView = 60;
    }

    public void BackButton()
    {
        backButton.onClick.Invoke();
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
}
