using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Movement : PlayerMovement
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
        if (Input.GetKey(KeyCode.R))
        {
            moveDirection.z += 1;
        }
        if (Input.GetKey(KeyCode.F))
        {
            moveDirection.z -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection.x -= 1;
        }
        if (Input.GetKey(KeyCode.G))
        {
            moveDirection.x += 1;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.H))
        {
            if (isCarryingBall)
            {
                ShootBall();
            }
            else
            {
                Charge(); 
            }
        }
    }
}
