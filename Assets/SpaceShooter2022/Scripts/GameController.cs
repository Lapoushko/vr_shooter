using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    [SerializeField] private float gameTime;
    private float sliderCurrentFillAmount = 1f;

    [Header("Score Components")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreMultiplierText;

    [Header("Game Over Components")]
    [SerializeField] private GameObject gameOverScreen;

    [Header("High Score Components")]
    [SerializeField] private TextMeshProUGUI highScoreText;
    private int highScore;

    [Header("Gameplay audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] gameplayAudio;

    [Header("Score Mulltiply")]
    [SerializeField] private int maxMultiplier;
    [SerializeField] private int currMultiplier;
    [SerializeField] private float maxTimeMultiplier;
    [SerializeField] private float currTimeMultiplier;

    private int playerScore;

    public enum GameState
    {
        Waiting,
        Playing,
        GameOver
    }
    public static GameState currentGameStatus;

    private void Awake()
    {
        currentGameStatus = GameState.Waiting;

        if(PlayerPrefs.HasKey("HighScore"))
        {
            highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        }
    }

    private void Update()
    {
        if(currentGameStatus == GameState.Playing)
            AdjustTimer();
        UpdateTimeScore();
    }

    private void AdjustTimer()
    {
        timerImage.fillAmount = sliderCurrentFillAmount - (Time.deltaTime / gameTime);

        sliderCurrentFillAmount = timerImage.fillAmount;
        if(sliderCurrentFillAmount <= 0f)
        {
            GameOver();
        }      
    }

    private void UpdateTimeScore()
    {
        if (currTimeMultiplier > 0f)
        {
            currTimeMultiplier -= Time.deltaTime;
        }     
        else
        {
            currTimeMultiplier = 0;
            currMultiplier = 0;
            scoreMultiplierText.text = ((int)Math.Pow(2, currMultiplier)).ToString() + "X Combo";
        }
    }

    public void UpdatePlayerScore(int asteroidHitPoints)
    {
        Debug.Log(playerScore);
        if (currentGameStatus != GameState.Playing)
            return;

        currTimeMultiplier = maxTimeMultiplier;
        if (currMultiplier < maxMultiplier)
        {
            currMultiplier++;
        }
        playerScore += asteroidHitPoints * (int)Math.Pow(2,currMultiplier);
        scoreMultiplierText.text = ((int)Math.Pow(2, currMultiplier)).ToString() + "X Combo";
        scoreText.text = playerScore.ToString();
    }

    public void StartGame()
    {
        currentGameStatus = GameState.Playing;
        PlayGameAudio(gameplayAudio[1], true);
    }
    private void GameOver()
    {
        currentGameStatus = GameState.GameOver;

        //show the Game over screen.
        gameOverScreen.SetActive(true);

        //check the high score...
        if(playerScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
            highScoreText.text = playerScore.ToString();
        }

        //change the audio
        PlayGameAudio(gameplayAudio[2], false);
    }
    public void ResetGame()
    {
        currentGameStatus = GameState.Waiting;

        //put timer to 1
        sliderCurrentFillAmount = 1f;
        timerImage.fillAmount = 1f;

        //reset the score
        playerScore = 0;
        scoreText.text = "0";

        //play intro music
        PlayGameAudio(gameplayAudio[0], true);
    }

    private void PlayGameAudio(AudioClip clipToPlay, bool shouldLoop)
    {
        audioSource.clip = clipToPlay;
        audioSource.loop = shouldLoop;
        audioSource.Play();
    }
}
