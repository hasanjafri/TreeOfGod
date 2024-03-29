﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

  private const int COIN_SCORE_AMOUNT = 5;

  public static GameManager Instance { set; get; }

  public bool isDead { set; get; }
  private bool isGameStarted = false;
  private PenguinMotor motor;

  public Animator gameCanvas, menuAnim, diamondAnim;
  public Text scoreText, coinText, modifierText, hiscoreText;
  private float score, coinScore, modifierScore;
  private int lastScore;

  public Animator deathMenuAnim;
  public Text deadScoreText, deadCoinText;

  private void Awake()
  {
        Instance = this;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PenguinMotor>();
        modifierScore = 1;
        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = scoreText.text = score.ToString("0");
        hiscoreText.text = PlayerPrefs.GetInt("Hiscore").ToString();
    }

  private void Update()
  {
        if (MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().isMoving = true;
            gameCanvas.SetTrigger("Show");
            menuAnim.SetTrigger("Hide");
        }

        if (isGameStarted && !isDead)
        {
            //Handle score
            score += (Time.deltaTime * modifierScore);
            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
        }
  }

    public void GetCoin()
    {
        diamondAnim.SetTrigger("Collect");
        coinScore++;
        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = scoreText.text = score.ToString("0");
    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void onPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void onDeath()
    {
        isDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        gameCanvas.SetTrigger("Hide");

        //Check if this is a hiscore
        if (score > PlayerPrefs.GetInt("Hiscore"))
        {
            float s = score;
            if (s % 1 == 0)
            {
                s += 1;
            }
            PlayerPrefs.SetInt("Hiscore", (int)s);
        }
    }
}
