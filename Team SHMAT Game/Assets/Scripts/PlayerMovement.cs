using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[AddComponentMenu("Control Script / PlayerMovement")]

public abstract class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 15.0f;
    protected Vector3 moveDirection;
    protected Vector3 lookDirection; 
    //private float gravity = -9.8f;
    private Rigidbody rb;

    private Vector3 chargeDirection;
    Coroutine lastRoutine = null; //used to keep track of which coroutine to stop 
    private bool isCharging = false;
    [SerializeField]
    private float chargeTime = 0.25f;
    [SerializeField]
    private float chargeSpeed = 40.0f;

    protected bool isCarryingBall = false;
    private GameObject ballCarried;
    [SerializeField]
    private float kickForce = 30f; 

    public Material defaultColor;
    public Material chargeColor; //use to indicate whether character is charging
    private MeshRenderer renderer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();

        lookDirection = transform.forward;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetInput(); 
        Move();
        HandleMat(); 
    }

    private void Move()
    {
        if(moveDirection != Vector3.zero) //allows player to retain rotation when not moving 
        {
            lookDirection = moveDirection; 
            Quaternion _rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = _rotation; //turn player object to direction of movement 
        }

        if (isCharging)
        {
            rb.velocity = chargeDirection.normalized * chargeSpeed; //charge forward at higher speed 
            return; 
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

    private void HandleMat()
    {
        if (!isCharging)
        {
            renderer.material = defaultColor;  
        }
        else
        {
            renderer.material = chargeColor; //turn orange while charging  
        }
    }

    protected virtual void GetInput()
    {
        //left empty for child classes to override
    }

    protected void Charge()
    {
        if (isCharging) //only triggers effect if player is not already charging 
        {
            return; 
        }

        isCharging = true;
        chargeDirection = moveDirection; //set charge direction so player cannot change it during charge 

        if (chargeDirection == Vector3.zero) //if the player does not currently have move inputs 
        {
            chargeDirection = lookDirection; //set to charge the way player is facing
        }

        lastRoutine = StartCoroutine(TimeChargeDuration());
    }

    private IEnumerator TimeChargeDuration() 
    {
        yield return new WaitForSeconds(chargeTime); //character charges for the duration of chargeTime
        isCharging = false; 
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

    private void OnCollisionEnter(Collision other)
    {
        if (isCharging)
        {
            if (other.gameObject.tag == "Player")
            {
                CheckBallIntercept(other.gameObject); 
                //interception only occurs when the other player is charging
            }

            if(other.gameObject.tag == "Ball")
            {
                BallAttach ballScript = other.gameObject.GetComponent<BallAttach>();
                CheckBallIntercept(ballScript.host); 
                //if collided with ball while charging, first get reference to the ball's carrier/host
            }

            if (lastRoutine != null)
            {
                StopCoroutine(lastRoutine); //prevents previously triggered coroutines from resetting charge duration
            }

            isCharging = false; //stops charge upon hitting anything 
            rb.velocity = new Vector3(0, 0, 0);
        }

        if (isCarryingBall) //ignore collision with ball while carrying it
        {
            if(other.gameObject.tag == "Ball")
            {
                Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
            }
        }
    }

    private void CheckBallIntercept(GameObject otherPlayer)
    {
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

        if (otherScript.ballCarried == null) //does not intercept if the other player is not carrying a ball 
        {
            return; 
        }

        BallAttach ballScript = otherScript.ballCarried.GetComponent<BallAttach>(); 
        if (ballScript != null) //get reference to the ball carried by other player
        {
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
