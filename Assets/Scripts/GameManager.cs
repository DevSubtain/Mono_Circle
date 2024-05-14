using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnGameStarted;
    public event Action OnGameOver;
    public event Action OnResetGame;
    public event Action<int, int> OnUpdateDisplay;

    public static GameManager Instance;

    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public bool gameStarted = false;

    private int round = -1;
    private int totalRounds = -1;
    private string username;
    private List<KeyCode> keyCodes;

    private static string secretKey;

    [DllImport("__Internal")]
    private static extern void SaveScore(string username, string score, string key);
    [DllImport("__Internal")]
    private static extern void RoundTrigger(string username, int roundNumber, string key);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadData();

#if UNITY_EDITOR
        if (totalRounds == -1)
            SetSettings(JsonUtility.ToJson(new SettingsSchema() { secretKey = "test", totalRounds = 2, username = "momo" }));
#endif
    }

    private void Update()
    {
        if (gameOver)
            if (Input.GetKey(KeyCode.Return))
                ResetGame();

        if (!gameStarted && !gameOver)
            foreach (KeyCode key in keyCodes)
                if (Input.GetKey(key))
                    StartGame();
    }

    //

    public void StartGame()
    {
        gameStarted = true;

        if (totalRounds > 0)
        {
            round++;
            SaveData();
        }
        OnGameStarted?.Invoke();

#if UNITY_WEBGL && !UNITY_EDITOR
        RoundTrigger(username, round, secretKey);
#endif

        // Debug.Log("StartGame()");
    }

    public void GameOver()
    {
        gameOver = true;
        gameStarted = false;
        OnGameOver?.Invoke();
        // Debug.Log("GameOver()");

        if (round == totalRounds && totalRounds > -1)
        {
            totalRounds = -1;
            SaveData();
        }
    }

    public void ResetGame()
    {
        gameOver = false;
        OnUpdateDisplay?.Invoke(round, totalRounds);
        OnResetGame?.Invoke();
        // Debug.Log("ResetGame()");
    }

    public void SetStartingKeys(List<KeyCode> keyCodes)
    {
        this.keyCodes = keyCodes;
        // Debug.Log("SetStartingKeys()");
    }

    public void SaveScore(string score)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveScore(username, score, secretKey);
#endif
        // Debug.Log("Saving score - username: " + username + " - score: " + score);
    }

    // To be called from JavaScript
    public void SetSettings(string json)
    {
        SettingsSchema settingsSchema = JsonUtility.FromJson<SettingsSchema>(json);

        username = settingsSchema.username;
        totalRounds = settingsSchema.totalRounds;
        secretKey = settingsSchema.secretKey;

        if (totalRounds > 0) round = 0;
        SaveData();
        OnUpdateDisplay?.Invoke(round, totalRounds);
    }

    //

    private void SaveData()
    {
        PlayerPrefs.SetInt(SAVED_KEYS.ROUND.ToString(), round);
        PlayerPrefs.SetInt(SAVED_KEYS.TOTAL_ROUNDS.ToString(), totalRounds);
        PlayerPrefs.SetString(SAVED_KEYS.USERNAME.ToString(), username);
    }

    private void LoadData()
    {
        round = PlayerPrefs.GetInt(SAVED_KEYS.ROUND.ToString(), -1);
        totalRounds = PlayerPrefs.GetInt(SAVED_KEYS.TOTAL_ROUNDS.ToString(), -1);
        username = PlayerPrefs.GetString(SAVED_KEYS.USERNAME.ToString(), "");

        OnUpdateDisplay?.Invoke(round, totalRounds);
    }
}

enum SAVED_KEYS { ROUND, TOTAL_ROUNDS, USERNAME }
public class SettingsSchema { public string username; public string secretKey; public int totalRounds; }