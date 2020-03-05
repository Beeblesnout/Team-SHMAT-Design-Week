using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBumper : MonoBehaviour
{
    GameManager manager;
    private float bumperForce = 7f;
    private int comboStageHits = 4; //how many bounces are required for combo to advance to the next point-awarding stage

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
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
            int points = DetermineScore();
            manager.AwardPointsToPlayer(points, playerNum); //award points to player for each bumper hit 
        }
    }

    private int DetermineScore() //decided how many points to give out depending on combo count 
    {
        int combo = manager.comboCount; 
        if(combo < comboStageHits) 
        {
            return 1; 
        }

        if (combo < comboStageHits * 2)
        {
            return 2;
        }

        if (combo < comboStageHits * 3)
        {
            return 3;
        }

        if (combo < comboStageHits * 4)
        {
            return 4;
        }

        return 5;
    }
}
