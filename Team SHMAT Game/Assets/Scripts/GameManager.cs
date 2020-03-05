using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameManager : SingletonBase<GameManager>
{
    public int player1Points;
    public int player2Points;

    public Text p1ScoreText;
    public Text p2ScoreText;
    public GameObject goalMessage; 

    public int comboCount = 0; //keeps track of how many times a ball has bounced in a roll

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        p1ScoreText.text = player1Points.ToString();
        p2ScoreText.text = player2Points.ToString();

    }

    public void AwardPointsToPlayer(int points, int playerNum)
    {
        if (playerNum == 1)
        {
            player1Points += points; 
        }
        else
        {
            player2Points += points;
        }
    }

    public void GoalPause(GameObject targetBall)
    {
        goalMessage.SetActive(true);
        Time.timeScale = 0f; //pauses the game when ball hits goal  
        StartCoroutine(ResetBall(targetBall)); 
    }

    public IEnumerator ResetBall(GameObject targetBall)
    {
        yield return new WaitForSecondsRealtime(2f); 
        //set camera position codes here 
        BallReset resetScript = targetBall.GetComponent<BallReset>();
        resetScript.ResetBallPos();
        goalMessage.SetActive(false);
        Time.timeScale = 1f; //resume game upon resetting ball position 
    }

    public void ResetCombo()
    {
        comboCount = 0;
    }

    public void IncreaseCombo(int amount)
    {
        comboCount += amount;
    }
}
