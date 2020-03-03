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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection.z += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection.z -= 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x += 1;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (!isCarryingBall)
            {
                Charge(); //charge when not carrying a ball
            }
            else
            {
                ShootBall(); //cannot charge when carrying ball - shoots ball instead
            }
        }
    }
}
