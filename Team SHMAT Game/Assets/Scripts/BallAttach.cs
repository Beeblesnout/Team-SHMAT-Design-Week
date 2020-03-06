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
    public bool player2;

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
    private float maxVel = 40;
    public float overspeedDrag = .2f;

    private MeshRenderer mRenderer;
    public Material defaultMat;
    public Material p1Mat;
    public Material p2Mat;

    private TrailRenderer line;
    public Material defaultLineMat;
    public Material p1LineMat;
    public Material p2LineMat;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        audioManagerScript = FindObjectOfType<AudioManager>();

        root = transform.parent;
        rb = root.GetComponent<Rigidbody>(); 
        mRenderer = transform.parent.GetComponentInChildren<MeshRenderer>();
        line = transform.parent.GetComponentInChildren<TrailRenderer>();
    }

    void Update()
    {
        if (lastHost != null)
        {
            PlayerMovement p = lastHost.GetComponent<PlayerMovement>();
            player2 = p.playerNum == 1;
            mRenderer.material = player2 ? p2Mat : p1Mat;
            line.material = player2 ? p1LineMat : p2LineMat;
        }
        else
        {
            mRenderer.material = defaultMat;
            line.material = defaultLineMat;
        }

        // if (rb.velocity.magnitude > 0)
        // {
        //     float sign = Mathf.Sign(Vector3.Dot(Vector3.right, rb.velocity));
        //     float angle = Vector3.Angle(Vector3.right, rb.velocity);
        // }
    }

    void FixedUpdate()
    {
        float mag = rb.velocity.magnitude;
        if (mag < minVel) //if ball is moving too slow
        {
            Vector3 dir = rb.velocity.normalized;
            mag = minVel; //hard set the ball to min vel
            rb.velocity = dir * mag; 
        }
        else if (mag > maxVel)
        {
            Vector3 dir = rb.velocity.normalized;
            mag = Mathf.Lerp(mag, maxVel, overspeedDrag);
            rb.velocity = dir * mag; 
        }
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
