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
        UpdateScores();
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PenguinMotor>();
  }

  private void Update()
  {
        if (MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
        }
  }

  public void UpdateScores()
  {
        scoreText.text = score.ToString();
        coinText.text = coinScore.ToString();
        modifierText.text = modifierScore.ToString();
  }
}
