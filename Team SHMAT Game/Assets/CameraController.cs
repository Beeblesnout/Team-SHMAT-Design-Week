using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : SingletonBase<CameraController>
{
    // Public Variables
    public Transform ballAnchor;
    public float ballAnchorWeight;
    public Transform player1Anchor;
    public float player1AnchorWeight;
    public Transform player2Anchor;
    public float player2AnchorWeight;

    // Private Variables
    private Transform cameraPivot;
    private new Camera camera;

    // ----Start Methods-------------------------------------------------------------------

    public override void Awake()
    {
        cameraPivot = transform.GetChild(0);
        camera = cameraPivot.GetComponentInChildren<Camera>();
    }

    void Start()
    {
        
    }

    // ----Runtime Methods-----------------------------------------------------------------

    void Update()
    {
        
    }

    // ----Helper Methods------------------------------------------------------------------

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
