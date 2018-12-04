using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

  public static GameManager Instance { set; get; }

  private bool isGameStarted = false;
  private PenguinMotor motor;

  public Text scoreText, coinText, modifierText;
  private float score, coinScore, modifierScore;

  private void Awake()
  {
        Instance = this;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PenguinMotor>();
        modifierScore = 1;
        UpdateScores();
    }

  private void Update()
  {
        if (MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
        }

        if (isGameStarted)
        {
            //Handle score
            score += (Time.deltaTime * modifierScore);
            scoreText.text = score.ToString("0");
        }
  }

  public void UpdateScores()
  {
        scoreText.text = score.ToString();
        coinText.text = coinScore.ToString();
        modifierText.text = "x" + modifierScore.ToString("0.0");
  }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        UpdateScores();
    }
}
