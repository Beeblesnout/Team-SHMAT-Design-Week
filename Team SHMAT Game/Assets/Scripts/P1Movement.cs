using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Movement : PlayerMovement
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); 
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); 
    }

    protected override void GetInput()
    {
        moveDirection = Vector3.zero; // prevents stacking up values 
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection.z -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection.x += 1;
        }

        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
        {
            if (isCarryingBall)
            {
                ShootBall();
            }
        }
    }
}
