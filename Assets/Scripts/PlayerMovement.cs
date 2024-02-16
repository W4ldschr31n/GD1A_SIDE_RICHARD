using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Parameters for movement
    [SerializeField]
    private Rigidbody2D rgbd;
    [SerializeField]
    private SpriteRenderer sprite;
    [SerializeField]
    private Animator animator;

    // Data for movement
    private Vector2 velocity;

    // External script that implements physical movement
    [SerializeField]
    private CharacterMovement characterMovement;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        Move();
        UpdateAnimation();
    }
    private void Move()
    {
        // Prepare to move the body
        velocity = rgbd.velocity;

        float direction = Input.GetKey(KeyCode.LeftArrow) ? -1f : (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);

        // Flip as we're changing direction, else keep previous orientation
        if(direction > 0f)
        {
            sprite.flipX = false;
        } else if(direction < 0f)
        {
            sprite.flipX = true;
        }
        // Move on a platform
        if (characterMovement.GetIsOnGround())
        {
            velocity = characterMovement.MoveOnPlatform(direction, velocity);
        }
        // Move near a wall
        else if (characterMovement.GetIsNearWall())
        {
            velocity = characterMovement.MoveNearWall(direction, velocity);
        }
        // Move in the air
        else
        {
            velocity = characterMovement.MoveInAir(direction, velocity);
        }
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            velocity = characterMovement.Jump(velocity);
        }

        // Reset player position
        if (Input.GetKeyDown(KeyCode.R))
            rgbd.transform.position = Vector2.zero;

        // Effectively move the body
        rgbd.velocity = velocity;
    }

    private void UpdateAnimation()
    {
        if (characterMovement.GetIsOnGround())
        {
            animator.SetBool("Walking", velocity.x != 0f);
        }
        // Is near a wall
        else if (characterMovement.GetIsNearWall())
        {
            // animator.SetBool("WallHug", true);
        }
        // Is in the air
        else
        {
            // animator.SetBool("InAir", true);
        }
    }

}
