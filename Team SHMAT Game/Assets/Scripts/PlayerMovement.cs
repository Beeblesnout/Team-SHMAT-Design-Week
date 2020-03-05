using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[AddComponentMenu("Control Script / PlayerMovement")]

public abstract class PlayerMovement : MonoBehaviour
{
    private AudioManager audioManagerScript;

    [SerializeField]
    private float speed = 15.0f;
    protected Vector3 moveDirection;
    protected Vector3 lookDirection; 
    //private float gravity = -9.8f;
    private Rigidbody rb;

    protected bool isCarryingBall = false;
    private GameObject ballCarried;
    [SerializeField]
    private float kickForce = 30f;

    private bool stolen = false; //prevent multiple steals triggered by one collision

    public int playerNum; //used to award score to the correct player; set as 1 or 2 on inspector 

    // Start is called before the first frame update
    protected virtual void Start()
    {
        audioManagerScript = FindObjectOfType<AudioManager>();

        rb = GetComponent<Rigidbody>();

        lookDirection = transform.forward;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetInput(); 
        Move();
    }

    private void Move()
    {
        if(moveDirection != Vector3.zero) //allows player to retain rotation when not moving 
        {
            lookDirection = moveDirection; 
            Quaternion _rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = _rotation; //turn player object to direction of movement 
        }

        rb.velocity = moveDirection.normalized * speed; //move normally 
        

        if (!isCarryingBall)
        {
            rb.velocity = moveDirection.normalized * speed; //move normally 
        }
        else
        {
            rb.velocity = moveDirection.normalized * speed * 0.75f; //move at reduced speed when carrying ball 
        }
    }

    public void OnDrawGizmos() //marks which way player object is facing before models are imported
    {
        Gizmos.color = Color.cyan; 
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    protected virtual void GetInput()
    {
        //left empty for child classes to override
    }

    public void SetBall(GameObject ball) //records the ball GameObject this player is carrying 
    {
        ballCarried = ball; 
    }

    protected void ShootBall()
    {
        BallAttach ballScript = ballCarried.GetComponent<BallAttach>(); 
        if(ballScript != null)
        {
            ballScript.HitBallWithForce(transform.forward, kickForce);
            audioManagerScript.PlaySound("ShootBall");
        }
        else
        {
            Debug.Log("Error: Ball cannot be released because its BallAttach script was not found."); 
        }
        ReleaseBall();
    }

    public void SetCarryingStateTo(bool state) //call to set whether player is carrying ball 
    {
        isCarryingBall = state; 
    }

    public void SetTrueStolen()
    {
        stolen = true;
    }

    private void OnCollisionEnter(Collision other)
    { 
        if (other.gameObject.tag == "Player")   
        {
            CheckBallIntercept(other.gameObject);
            //interception only occurs when the other player is charging
        }

        //if(other.gameObject.tag == "Ball")
        //{
        //    BallAttach ballScript = other.transform.GetChild(0).GetComponent<BallAttach>();
        //    CheckBallIntercept(ballScript.host);
        //    //if (ballScript.host == null) return; 
        //    //if collided with ball while charging, first get reference to the ball's carrier/host
        //}

        if (isCarryingBall) //ignore collision with ball while carrying it
        {
            if(other.gameObject.tag == "Ball")
            {
                Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            BallAttach ballScript = other.transform.GetComponent<BallAttach>();
            CheckBallIntercept(ballScript.host);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            stolen = false; //reset ability to steal once players are no longer colliding
        }
    }

    private void CheckBallIntercept(GameObject otherPlayer)
    {
        if (stolen) return; //prevents multiple steals from occuring in one collision

        if (otherPlayer == this.gameObject)
        {
            return; //Prevents a glitch where a player intercepts the ball from himself
        }

        if (otherPlayer == null)
        {
            return; //in the case of collision with ball, does not activate interception if the ball doesn't have a host
        }

        PlayerMovement otherScript = otherPlayer.GetComponent<PlayerMovement>();

        if (otherScript == null) 
        {
            return;
        }

        //in this case, we are sure its the other player

        if (otherScript.ballCarried == null) //does not intercept if the other player is not carrying a ball 
        {
            return; 
        }

        //they have the ball

        SetTrueStolen();
        otherScript.SetTrueStolen(); //set stolen variable to true for both players to prevent either from steal until OnCollisionExit

        BallAttach ballScript = otherScript.ballCarried.GetComponent<BallAttach>(); 
        if (ballScript != null) //get reference to the ball carried by other player
        {
            audioManagerScript.PlaySound("Steal");
            ballScript.AttachTo(this.gameObject);
        }

        otherScript.ReleaseBall(); 
    }

    public void ReleaseBall()
    {
        Collider ballCollider = ballCarried.GetComponent<Collider>();
        Physics.IgnoreCollision(ballCollider, GetComponent<Collider>(), false); //re-enable collision with ball upon release

        isCarryingBall = false; //reset 
        ballCarried = null;
    }
}
