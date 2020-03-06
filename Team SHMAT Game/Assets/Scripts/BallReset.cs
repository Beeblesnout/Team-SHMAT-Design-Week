using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour
{
    private Rigidbody rb;

    public Transform resetPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Debug.Log(rb.velocity.magnitude); 
    }

    public void ResetBallPos() //move ball to reset point
    {
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        BallAttach ballScript = transform.GetChild(0).GetComponent<BallAttach>();
        if (ballScript.lastHost != null)
        {
            ballScript.lastHost = null; //encaptulate later after pull
        }

        transform.position = resetPoint.position; 
    }
}
