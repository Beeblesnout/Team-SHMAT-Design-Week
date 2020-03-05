using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    GameManager manager; 
    public bool player2;

    private int goalPoints = 20; //amount of points awarded for scoring 

    private void Start()
    {
        manager = FindObjectOfType<GameManager>(); 
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if (player2) //ball enters player2's goal 
            {
                manager.AwardPointsToPlayer(goalPoints, 1); //award 5 pts to Player1
            }
            else
            {
                manager.AwardPointsToPlayer(goalPoints, 2);
            }
        }
    }
}
