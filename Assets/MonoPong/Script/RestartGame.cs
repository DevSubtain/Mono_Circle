/*
this script restarts the game
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestartGame : MonoBehaviour
{
    public bool isGameOver = true;
    private GameManager gameManager;

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
        gameManager = GameManager.instance;


    }



    private void Update()
    {
        if (SplashScreen.instance.splash_Obj.activeInHierarchy)
            return;



        if ((Input.GetButtonUp("Jump") || Input.GetMouseButtonUp(0)) && text.text == "Press Mouse or Space Button To Start" && isGameOver)
        {
            isGameOver = false;
            Press_Btns();
        }


        if (Input.GetButtonUp("Submit") && text.text != "Press Mouse or Space Button To Start" && isGameOver)
        {
            isGameOver = false;
            gameManager.OnUpdateDisplay();
            Press_Btns();


        }
    }






    void Press_Btns()
    {
        if (gameManager.totalRounds > 0)
        {
            gameManager.round++;
            gameManager.SaveData();
        }
        gameManager.Round_To_Trigger();

        text.gameObject.SetActive(false);
        if (gameManager.State != GameManager.GameStates.PLAYING)
        {
            gameManager.SetState(GameManager.GameStates.PLAYING);

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
        if (gameManager.State != GameManager.GameStates.PLAYING)
        {
            gameManager.SetState(GameManager.GameStates.PLAYING);
            Invoke("ChangeButtonImage", 1f);
        }
    }

    //changes the image from the play icon to the restart icon
    private void ChangeButtonImage()
    {
        gameObject.GetComponent<Image>().sprite = ResetSprite;
    }
}
