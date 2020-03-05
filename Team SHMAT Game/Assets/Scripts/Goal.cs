using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    GameManager manager;
    private AudioManager audioManagerScript;
    public bool player2;

    private int goalPoints = 30; //amount of points awarded for scoring 

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        audioManagerScript = FindObjectOfType<AudioManager>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if (player2) //ball enters player2's goal 
            {
                manager.AwardPointsToPlayer(goalPoints, 1); //award 30 pts to Player1
            }
            else
            {
                manager.AwardPointsToPlayer(goalPoints, 2);
            }

            audioManagerScript.PlaySound("Bumper5");
            manager.GoalPause(other.gameObject);
        }
    }
}
