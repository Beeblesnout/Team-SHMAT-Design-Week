using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttach : MonoBehaviour
{
    /// <summary>
    /// Keeps track of which player is carrying the ball.
    /// </summary>
    public GameObject host;
    /// <summary>
    /// Last player that carried the ball.
    /// </summary>
    public GameObject lastHost;

    /// <summary>
    /// Determines how far the ball checks for attachable players.
    /// </summary>
    public float attachRadius = 1f;
    /// <summary>
    /// Determines how far ahead of player the ball is when attached to player.
    /// </summary>
    public float aheadDistance = 3f;

    private bool collided = false;
    private GameManager manager;
    private AudioManager audioManagerScript;

    [SerializeField]
    private Transform root;
    private Rigidbody rb;

    private float minVel = 25;
    private float maxVel = 45;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        audioManagerScript = FindObjectOfType<AudioManager>();

        root = transform.parent;
        rb = root.GetComponent<Rigidbody>(); 
    }

    // Collider[] player;
    void FixedUpdate()
    {
        float mag = rb.velocity.magnitude;
        if (mag < minVel || mag > maxVel) //if ball's velocity exceeds limits 
        {
            Vector3 dir = rb.velocity.normalized;
            mag = Mathf.Clamp(mag, minVel, maxVel);

            rb.velocity = dir * mag; 
        }

        //prevents multiple collision events from occuring
        //ball cannot be grabbed normally if already attached to a specific player 
        // if (!collided && transform.parent)
        // {
        //     if (Physics.OverlapSphereNonAlloc(transform.position, 1, player, LayerMask.GetMask("Player")) != 0)
        //     {
        //         collided = true;
        //         AttachTo(player[0].gameObject);
        //     }
        //     else
        //     {
        //         collided = false;
        //     }
        // }

        // if (collided) return; //prevents multiple collision events from occuring 
        // if (transform.parent != null) return; //ball cannot be grabbed normally if already attached to a specific player 

        // if (other.gameObject.tag == "Player")
        // {
        //     AttachTo(other.gameObject);

        //     collided = true;
        // }
    }

    /// <summary>
    /// Attach to a new player.
    /// </summary>
    /// <param name="player">Player to attach to.</param>
    public void AttachTo(GameObject player)
    {
        rb.velocity = Vector3.zero; //stop ball from rolling 
        rb.isKinematic = true; 

        Vector3 newPos = player.transform.position; 
        newPos += player.transform.forward * aheadDistance; //move ball to player's front
        root.position = newPos; 
        root.parent = player.transform;
        // Physics.IgnoreCollision();

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
        lastHost = host;
        manager.ResetCombo(); //reset ball's combo count whenever it is picked up
    }

    public void KickBallWithForce(Vector3 direction, float forceAmount)
    {
        root.parent = null; //ball is released from player 
        host = null; 

        rb.isKinematic = false;
        rb.AddForce(direction * forceAmount, ForceMode.Impulse);
    }

    public void HitBallWithForce(Vector3 direction, float forceAmount)
    {
        root.parent = null; //ball is released from player 
        host = null;
        rb.isKinematic = false;
        //rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
        //rb.constraints &= ~RigidbodyConstraints.FreezePositionZ; //unfreeze ball's horizontal movement 

        rb.AddForce(direction * forceAmount, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name); 

        if (other.CompareTag("Player"))
        {
            // Debug.Log("ping");
            if (collided) return; //prevents multiple collision events from occuring 
            if (host != null) return; //ball cannot be grabbed normally if already attached to a specific player 
            if (other.gameObject.tag == "Player")
            {
                audioManagerScript.PlaySound("GrabBall");
                AttachTo(other.gameObject);

                collided = true;
            }
        }

        if (other.CompareTag("Wall"))
        {
            Debug.Log(other.name);
            audioManagerScript.PlaySound("BouncingWall"); 
        }

        if (other.CompareTag("Blocker"))
        {
            audioManagerScript.PlaySound("PadHit");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && host == null) //if released by player
        {
            StopAllCoroutines();
            StartCoroutine(DelayResetCollision()); 
        }
    }

    IEnumerator DelayResetCollision()
    {
        yield return new WaitForSeconds(0.25f);
        collided = false; //reset collision events
    }
}
