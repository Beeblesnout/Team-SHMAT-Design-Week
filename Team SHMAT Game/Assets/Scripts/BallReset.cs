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

    public void ResetBallPos() //move ball to reset point
    {
        rb.velocity = Vector3.zero; 
        transform.position = resetPoint.position; 
    }
}
