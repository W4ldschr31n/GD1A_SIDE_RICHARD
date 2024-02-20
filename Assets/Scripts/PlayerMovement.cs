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

    }
    public void HandleInputs()
    {
        // Prepare to move the body
        velocity = rgbd.velocity;
        float direction = Input.GetAxisRaw("Horizontal");

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

        // Effectively move the body
        rgbd.velocity = velocity;
    }

    public void UpdateAnimation()
    {
        if (characterMovement.GetIsOnGround())
        {
            animator.SetBool("Walking", velocity.x != 0f);
            animator.SetBool("WallHugging", false);
            animator.SetBool("InAir", false);
        }
        // Is near a wall
        else if (characterMovement.GetIsNearWall())
        {
            animator.SetBool("Walking", false);
            animator.SetBool("WallHugging", true);
            animator.SetBool("InAir", true);
        }
        // Is in the air
        else
        {
            animator.SetBool("Walking", false);
            animator.SetBool("WallHugging", false);
            animator.SetBool("InAir", true);
        }
    }

}
