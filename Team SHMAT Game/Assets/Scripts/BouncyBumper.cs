using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBumper : MonoBehaviour
{
    private float bumperForce = 7f; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            //other.rigidbody.AddExplosionForce(bumperForce, transform.position, 2, 0, ForceMode.Impulse);
            //other.rigidbody.AddForce(other.contacts[0].normal * bumperForce, ForceMode.Impulse);

            Vector3 dir = other.rigidbody.velocity.normalized;
            float mag = other.rigidbody.velocity.magnitude;

            dir = Vector3.Reflect(dir, other.contacts[0].normal);
            mag += bumperForce; 

            other.rigidbody.velocity = dir * mag;
        }
    }
}
