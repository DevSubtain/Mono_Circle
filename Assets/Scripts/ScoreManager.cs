using System.Globalization;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text scoreText, roundsText, gameOverText, instructionsText;
    public float Score;

    private void Awake()
    {
        Instance = this;
        roundsText.text = "";
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarted += () =>
        {
            instructionsText.text = "";
        };
        GameManager.Instance.OnGameOver += () =>
        {
            gameOverText.text = "SCORE " + Score.ToString("F2", CultureInfo.InvariantCulture) + "\nGAME OVER\nPRESS ENTER TO RESTART";
            gameOverText.gameObject.SetActive(true);
            GameManager.Instance.SaveScore(Score.ToString("F2", CultureInfo.InvariantCulture));
        };

        GameManager.Instance.OnResetGame += () =>
        {
            instructionsText.text = "PRESS ↑ OR SPACE TO START";
            gameOverText.gameObject.SetActive(false);
        };

        GameManager.Instance.OnUpdateDisplay += (round, totalRounds) =>
        {
            if (totalRounds > -1)
                roundsText.text = $"ROUND {round + 1}/{totalRounds}";
            else
                roundsText.text = "";
        };
    }

    private void Update()
    {
        Score = PlayerController.Instance.passedPipes + PlayerController.Instance.traveledDistance / 10f;
        scoreText.text = "SCORE " + Score.ToString("F2", CultureInfo.InvariantCulture);
    }
}