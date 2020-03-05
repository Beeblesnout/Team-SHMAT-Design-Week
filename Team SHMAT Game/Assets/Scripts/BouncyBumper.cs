using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBumper : MonoBehaviour
{
    private GameManager manager;
    private AudioManager audioManagerScript;

    private float bumperForce = 7f;
    //private int comboStageHits = 5; //how many bounces are required for combo to advance to the next point-awarding stage

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        audioManagerScript = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            //other.rigidbody.AddExplosionForce(bumperForce, transform.position, 2, 0, ForceMode.Impulse);
            //other.rigidbody.AddForce(other.contacts[0].normal * bumperForce, ForceMode.Impulse);

            Vector3 dir = other.rigidbody.velocity.normalized;
            float mag = other.rigidbody.velocity.magnitude;

            dir = Vector3.Reflect(dir, other.contacts[0].normal);
            mag += bumperForce; 

            other.rigidbody.velocity = dir * mag;

            //award points to the player who shot the ball
            BallAttach ballScript = other.transform.GetChild(0).GetComponent<BallAttach>();
            GameObject playerAwarded = ballScript.lastHost; //find the ball's last host 
            PlayerMovement playerScript = playerAwarded.GetComponent<PlayerMovement>();
            int playerNum = playerScript.playerNum; //access the host's number

            manager.IncreaseCombo(1); 
            int points = DetermineScore();
            manager.AwardPointsToPlayer(points, playerNum); //award points to player for each bumper hit 
        }
    }

    private int DetermineScore() //decided how many points to give out depending on combo count 
    {
        int combo = manager.comboCount;
        //Debug.Log(combo + "COMBO!");
        if (combo < 5) 
        {
            audioManagerScript.PlaySound("Bumper1");
            return 1; 
        }

        //if (combo < comboStageHits * 2) //alternative combo stage calculation
        if (combo < 12)
        {
            audioManagerScript.PlaySound("Bumper2");
            return 2;
        }

        if (combo < 22)
        {
            audioManagerScript.PlaySound("Bumper3");
            return 3;
        }

        if (combo < 30)
        {
            audioManagerScript.PlaySound("Bumper4");
            return 4;
        }

        audioManagerScript.PlaySound("Bumper5");
        return 5;
    }
}
