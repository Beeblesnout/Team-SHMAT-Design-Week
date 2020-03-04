using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO implement rotations
// TODO implement constraints

/// <summary>
/// Camera rig controller with high functionality
/// </summary>
[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    // Public Variables
    [Header("Main Values")]
    [SerializeField]
    public Vector3 position;
    [SerializeField]
    public Vector2 rotation;
    [SerializeField]
    public float zoomDistance;
    [SerializeField]
    public float trackDuration;
    [SerializeField]
    public float trackSpeed;

    // Constraints
    [Header("Constraints")]
    [SerializeField]
    public bool lockPosX;
    [SerializeField]
    public bool lockPosY;
    [SerializeField]
    public bool lockPosZ;
    [SerializeField]
    public bool lockRotX;
    [SerializeField]
    public bool lockRotY;

    // Anchors
    [Header("Ball Anchor")]
    [Header("Anchors")]
    [Space]
    [SerializeField]
    public Transform ballAnchor;
    [SerializeField]
    public float ballAnchorWeight;
    [SerializeField]
    [Range(0,1)]
    public float ballAnchorOpacity;
    
    [Header("Player 1 Anchor")]
    [SerializeField]
    public Transform player1Anchor;
    [SerializeField]
    public float player1AnchorWeight;
    [SerializeField]
    [Range(0,1)]
    public float player1AnchorOpacity;
    
    [Header("Player 2 Anchor")]
    [SerializeField]
    public Transform player2Anchor;
    [SerializeField]
    public float player2AnchorWeight;
    [SerializeField]
    [Range(0,1)]
    public float player2AnchorOpacity;

    // Private Variables
    private Transform cameraPivot;
    private new Camera camera;
    private Vector3 targetPos;

    // ----Start Methods-------------------------------------------------------------------

    void Awake()
    {
        cameraPivot = transform.GetChild(0);
        camera = cameraPivot.GetComponentInChildren<Camera>();
    }

    void Start()
    {
        
    }

    // ----Runtime Methods-----------------------------------------------------------------

    float changeTime;
    float trackProg;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, .1f);
        // float 
        //var prevTargetPos = targetPos;
        //targetPos = CalcAnchorPoint();
        //if (targetPos != prevTargetPos)
        //{
        //    changeTime = Time.time;
        //}

        ////Debug.Log(Time.time - changeTime);
        //if (Time.time - changeTime < trackDuration)
        //{
        //    Debug.Log("moving");
        //    trackProg = Time.time - changeTime / trackDuration;
        //    if (trackProg < 1)
        //    {
        //        //Debug.Log(targetPos);
        //        transform.position = Vector3.MoveTowards(transform.position, targetPos, trackSpeed);
        //    }
        //}
        //else
        //{
        //    Debug.Log("not moving");
        //}
    }

    // ----Helper Methods------------------------------------------------------------------

    public Vector3 CalcAnchorPoint()
    {
        Vector3 anchorPoint = new Vector3();
        float d = ballAnchorWeight * ballAnchorOpacity
             + player1AnchorWeight * player1AnchorOpacity
             + player2AnchorWeight * player2AnchorOpacity;

        anchorPoint += ballAnchor.position * (ballAnchorWeight / d) * ballAnchorOpacity;
        anchorPoint += player1Anchor.position * (player1AnchorWeight / d) * player1AnchorOpacity;
        anchorPoint += player2Anchor.position * (player2AnchorWeight / d) * player2AnchorOpacity;
        
        return anchorPoint;
    }

    /// <summary>
    /// Rotates the camera pivot, aligned to the world up axis.
    /// </summary>
    /// <param name="x">The yaw rotation of the camera rig</param>
    /// <param name="y">The pitch rotation of the camera rig</param>
    public void PivotCamera(float x, float y)
    {
        // rotate left/right
        cameraPivot.Rotate(Vector3.up, x, Space.World);
        // rotate up/down
        cameraPivot.Rotate(Vector3.right, y, Space.Self);
    }
    
    /// <summary>
    /// Rotates the camera pivot, aligned to the world up axis.
    /// </summary>
    /// <param name="angle">Vector representing the yaw and pitch</param>
    public void PivotCamera(Vector2 angles)
    {
        // rotate left/right
        cameraPivot.Rotate(Vector3.up, angles.x, Space.World);
        // rotate up/down
        cameraPivot.Rotate(Vector3.right, angles.y, Space.Self);
    }

    /// <summary>
    /// Set the rotation of the camera pivot, aligned to the world up axis.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetCameraPivot(float x, float y)
    {
        // reset the camera's rotation
        cameraPivot.rotation = Quaternion.identity;
        // pivot camera normally
        PivotCamera(x, y);
    }

    /// <summary>
    /// Set the rotation of the camera pivot, aligned to the world up axis.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetCameraPivot(Vector2 angles)
    {
        // reset the camera's rotation
        cameraPivot.rotation = Quaternion.identity;
        // pivot camera normally
        PivotCamera(angles);
    }
}
