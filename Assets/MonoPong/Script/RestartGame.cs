/*
this script restarts the game
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestartGame : MonoBehaviour
{

    private Mono_GameManager gameManager;

    public static RestartGame Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public Sprite ResetSprite;

    public Text text;
    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<Mono_GameManager>();

    }



    private void Update()
    {


        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButtonUp(0)) && text.text == "Press Mouse or Space Button To Start")
        {
            
            Press_Btns();
        }


        if (Input.GetKey(KeyCode.Return) && text.text != "Press Mouse or Space Button To Start")
        {


            Press_Btns();


        }
    }

     void Press_Btns()
    {
        text.gameObject.SetActive(false);
        if (gameManager.State != Mono_GameManager.GameStates.PLAYING)
        {
            gameManager.SetState(Mono_GameManager.GameStates.PLAYING);

        }
    }


    public void AssignText()
    {
        text.text = "Score " + ScoreScript.Instance.Score + "\nGame Over\n" + "Press Enter To Start Game";
        text.gameObject.SetActive(true);
    }
    //method used when the button is pressed
    public void Press()
    {
        if (gameManager.State != Mono_GameManager.GameStates.PLAYING)
        {
            gameManager.SetState(Mono_GameManager.GameStates.PLAYING);
            Invoke("ChangeButtonImage", 1f);
        }
    }

    //changes the image from the play icon to the restart icon
    private void ChangeButtonImage()
    {
        gameObject.GetComponent<Image>().sprite = ResetSprite;
    }
}
