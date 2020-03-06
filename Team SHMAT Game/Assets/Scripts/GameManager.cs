using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameManager : SingletonBase<GameManager>
{
    public int player1Points;
    public int player2Points;
    private int victoryPoints = 300; //players win upon reaching 300 points 
    private int winner;
    private SceneController sceneManager; 

    /*private float maxTime = 300; //game ends after 5 minutes
    private float currentTime; */

    public Text p1ScoreText;
    public Text p2ScoreText;
    public GameObject goalMessage; 

    public int comboCount = 0; //keeps track of how many times a ball has bounced in a roll

    void Start()
    {
        sceneManager = FindObjectOfType<SceneController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        CheckWinCondition();

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "MainGame")
        {
            p1ScoreText.text = player1Points.ToString();
            p2ScoreText.text = player2Points.ToString();
        }

        //currentTime = maxTime - Time.time; 
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

    private void CheckWinCondition()
    {
        if (player1Points >= victoryPoints)
        {
            winner = 1;
            SceneController.Instance.SetWinner(1);
            SceneController.Instance.SwitchSceneTo("WinScreen"); 
            return; 
        }
        if (player2Points >= victoryPoints)
        {
            winner = 2;
            SceneController.Instance.SetWinner(2);
            SceneController.Instance.SwitchSceneTo("WinScreen");
            return;
        }

        //check time remaining
        /*if(currentTime <= 0)
        {
            if(player1Points > player2Points)
            {
                winner = 1;
                //win scene switch here
            }
            else
            {
                winner = 2;
                //win scene switch here
            }
        }*/
    }

    public void GoalPause(GameObject targetBall)
    {
        goalMessage.SetActive(true);
        targetBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        Time.timeScale = 0.2f; //pauses the game when ball hits goal  
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
