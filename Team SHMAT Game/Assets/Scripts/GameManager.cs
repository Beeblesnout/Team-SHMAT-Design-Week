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
}
