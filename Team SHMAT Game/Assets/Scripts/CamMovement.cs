using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CamMovement : MonoBehaviour
{
    public List<Transform> targets;
    private Camera cam; 

    public Vector3 offset;
    private Vector3 velocity;
    [SerializeField]
    private float smoothTime = 0.2f;

    [SerializeField]
    private float minZoom = 75f;
    [SerializeField]
    private float maxZoom = 40f;
    [SerializeField]
    private float zoomSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 26.0f, 0);
        cam = GetComponent<Camera>(); 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (targets.Count == 0)
        {
            Debug.Log("No players referenced as camera targets");
            return;
        }

        FollowPlayers();
        CamZoom(); 
    }

    private void FollowPlayers()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 destination = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothTime); 
    }

    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position; //return the position of the only target 
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position); 
        }

        return bounds.center; 
    }

    private void CamZoom()
    {
        float newZoom = minZoom * GetGreatestDist() / 50; //50 is the length of the current map
        newZoom = Mathf.Clamp(newZoom, maxZoom, minZoom);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, zoomSpeed * Time.deltaTime);
    }

    private float GetGreatestDist() //the longest dist between two players furthest away (in case there are more than two player objects)
    {
        if (targets.Count == 1)
        {
            return 45;  
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x + 20; 
    }
}
