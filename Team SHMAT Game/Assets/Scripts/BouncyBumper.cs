﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBumper : MonoBehaviour
{
    [SerializeField]
    private float bumperForce = 30f; 

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
            other.rigidbody.AddExplosionForce(bumperForce, transform.position, 2, 0, ForceMode.Impulse);
        }
    }
}