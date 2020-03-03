using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttach : MonoBehaviour
{
    public GameObject host; //keeps track of which player is carrying the ball 

    public float aheadDistance = 3f; //determines how far ahead of player the ball is when attached to player
    private bool collided = false;

    private Rigidbody rb; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttachTo(GameObject player)
    {
        rb.velocity = Vector3.zero; //stop ball from rolling 
        rb.isKinematic = true; 
        //rb.constraints = RigidbodyConstraints.FreezePosition;

        Vector3 newPos = player.transform.position; 
        newPos += player.transform.forward * aheadDistance; //move ball to player's front
        transform.position = newPos; 
        transform.parent = player.transform;

        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
        if (playerScript != null)
        {
            playerScript.SetCarryingStateTo(true); //update that player is now carrying ball
            playerScript.SetBall(this.gameObject); //plug this object in as the ball player is carrying  
        }
        else
        {
            Debug.Log("Player object does not have PlayerMovement script attached.");
        }

        host = player; 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (collided == true)
        {
            return; //prevents multiple collision events from occuring 
        }

        if (transform.parent != null) //ball cannot be grabbed normally if already attached to a specific player 
        {
            return; 
        }

        if (other.gameObject.tag == "Player")
        {
            AttachTo(other.gameObject);

            collided = true;
        }
    }

    public void KickBallWithForce(Vector3 direction, float forceAmount)
    {
        transform.SetParent(null); //ball is released from player 
        host = null;
        rb.isKinematic = false;
        //rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
        //rb.constraints &= ~RigidbodyConstraints.FreezePositionZ; //unfreeze ball's horizontal movement 

        rb.AddForce(direction * forceAmount, ForceMode.Impulse);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player" && host == null) //if released by player
        {
            collided = false; //reset collision events
        }
    }
}
