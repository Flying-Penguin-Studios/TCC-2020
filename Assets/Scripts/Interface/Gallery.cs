using UnityEngine;
using UnityEngine.SceneManagement;

public class Gallery : MonoBehaviour
{
    public GameObject item;
    bool pressedL;
    bool pressedR;
    public float speedCursor = 0.025f;
    public float speedJoystick = 2.5f;
    bool canRotate = false;

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

        }
        
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
        Time.timeScale = 1;
        GameController.Singleton.CanvasFadeOut();
        Invoke("LoadMenu", 3);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void DeactivateAllObjects()
    {
        //GameObject[] lista = item.GetComponentsInChildren<GameObject>();
        //foreach (GameObject x in lista)
        //{
        //    x.gameObject.SetActive(false);
        //}
    }
}
