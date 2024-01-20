using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{

    protected override void Move()
    {
        // Prepare to move the body
        velocity = rgbd.velocity;

        float direction = Input.GetKey(KeyCode.LeftArrow) ? -1f : (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);
        // Move on a platform
        if (isOnGround)
        {
            MoveOnPlatform(direction);
        }
        // Move near a wall
        else if (isNearWall)
        {
            MoveNearWall(direction);
        }
        // Move in the air
        else
        {
            MoveInAir(direction);
        }
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Reset player position
        if (Input.GetKeyDown(KeyCode.R))
            rgbd.transform.position = Vector2.zero;

        // Effectively move the body
        rgbd.velocity = velocity;
    }
}
