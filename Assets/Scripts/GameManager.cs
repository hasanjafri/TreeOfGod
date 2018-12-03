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
    //UpdateScores();
  }

  private void Update()
  {
    
  }

  public void UpdateScores()
  {
    scoreText.text = score.ToString();
    coinText.text = coinScore.ToString();
    modifierText.text = modifierScore.ToString();
  }
}
