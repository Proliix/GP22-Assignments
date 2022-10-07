using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public TMP_Text timerTextObj;
    public TMP_Text p1LivesTextObj;
    public TMP_Text p1BombsTextObj;
    public TMP_Text p1SizeTextObj;
    public TMP_Text p2LivesTextObj;
    public TMP_Text p2BombsTextObj;
    public TMP_Text p2SizeTextObj;

    [Header("Game Over")]
    public GameObject gameoverObj;
    public TMP_Text playerWinText;
    public Button restartButton;
    public Button exitButton;



    [Tooltip("Timer in seconds")]
    public float timer = 300;


    private int p1Lives, p2Lives;
    private int p1Bombs, p2Bombs;
    private int p1Size, p2Size;

    void Start()
    {
        Time.timeScale = 1;
        gameoverObj.SetActive(false);
    }

    public void GameIsOver(string winnerName, bool isDraw = false)
    {
        Time.timeScale = 0;
        if (!isDraw)
        {
            playerWinText.text = winnerName + " Wins";
        }
        else if(isDraw)
        {
            playerWinText.text = "DRAW";
        }

        gameoverObj.SetActive(true);
    }

    public string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdatePlayerLives(int PlayerNum, int lives)
    {
        if (PlayerNum == 0)
        {
            p1Lives = lives;
            p1LivesTextObj.text = "x " + lives;
        }
        else if (PlayerNum == 1)
        {
            p2Lives = lives;
            p2LivesTextObj.text = "x " + lives;
        }

    }

    public void UpdatePlayerStats(int PlayerNum, int bombs, int size)
    {
        if (PlayerNum == 0)
        {
            p1Bombs = bombs;
            p1Size = size;
            p1BombsTextObj.text = "x " + bombs;
            p1SizeTextObj.text = "x " + size;

        }
        else if (PlayerNum == 1)
        {
            p2Bombs = bombs;
            p2Size = size;
            p2BombsTextObj.text = "x " + bombs;
            p2SizeTextObj.text = "x " + size;

        }

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        timerTextObj.text = FormatTime(timer);
    }
}
