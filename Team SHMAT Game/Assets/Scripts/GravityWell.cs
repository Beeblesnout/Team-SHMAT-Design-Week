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

    void Start()
    {
        
    }

    float ballInfluence;
    Collider[] cols;
    void FixedUpdate()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, influenceRadius, cols, LayerMask.GetMask("Ball")) > 0)
        {
            Debug.Log("aa");
            Vector3 toBall = cols[0].transform.position - transform.position;
            float distToDeadzone = toBall.magnitude - deadzoneRadius;
            if (distToDeadzone < influenceRadius)
            {
                Debug.Log("in range");
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
