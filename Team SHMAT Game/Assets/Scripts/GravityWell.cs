using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    public float strength;
    public float influenceRadius;
    public float deadzoneRadius;
    public AnimationCurve distCurve;

    public Mesh ringMesh;
    public int gizmoCurveLineCount = 20;

    private SphereCollider trigger;

    void Start()
    {
        trigger = GetComponent<SphereCollider>();
    }

    [ExecuteAlways]
    void Update()
    {
        trigger.radius = influenceRadius;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Vector3 toBall = other.transform.position - transform.position;
            float distToDeadzone = toBall.magnitude - deadzoneRadius;
            if (distToDeadzone > 0)
            {
                float f = distToDeadzone / (influenceRadius - deadzoneRadius);
                float ballInfluence = distCurve.Evaluate(f) * strength;

                other.attachedRigidbody.AddForce(-toBall.normalized * ballInfluence, ForceMode.Acceleration);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireMesh(ringMesh, transform.position, Quaternion.identity, Vector3.one * influenceRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireMesh(ringMesh, transform.position, Quaternion.identity, Vector3.one * deadzoneRadius);

        Gizmos.color = Color.green;
        for (int i = 0; i < gizmoCurveLineCount; i++)
        {
            float f = (float)i / gizmoCurveLineCount;
            float xPos = (influenceRadius - deadzoneRadius) * f + deadzoneRadius;
            Vector3 from = transform.position + new Vector3(xPos, 0, 0);
            Vector3 to = transform.position + new Vector3(xPos, distCurve.Evaluate(f) * strength, 0);
            Gizmos.DrawLine(from, to);
        }
    }
}
