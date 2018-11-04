﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnGameStarted;
    public static event PlayerDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;

    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown

    }

    int score = 0;
    bool gameOver = true;

    public bool GameOver { get { return gameOver; } }

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerDied += OnPlayerScored;

    }

    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
    }

    void OnCountdownFinished() {

        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }

    void OnPlayerDied() {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("Highscore");
        if (score > savedScore) {
            PlayerPrefs.SetInt("Highscore", score);

        }
        SetPageState(PageState.GameOver);
    }

    void OnPlayerScored() {

        score++;
        scoreText.text = score.ToString();

    }

    void SetPageState(PageState state)
    {
        switch (state)
        {

            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);

                break;

            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);

                break;

            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);

                break;

            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);

                break;


        }

    }

    public void ConfirmGameOver()
    {

        OnGameOverConfirmed();
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
    {
        SetPageState(PageState.Countdown);

    }
}
