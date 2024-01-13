using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMovement : MonoBehaviour
{
    // Parameters for movement
    [SerializeField, Range(0f, 10f)]
    private float speed = 4f;
    [SerializeField, Range(0f, 10f)]
    private float jumpHeight = 2f;
    [SerializeField, Range(0f, 1f)]
    private float airControl = 0.5f;
    [SerializeField]
    private Rigidbody2D rgbd;

    // State allowing movement
    private bool isOnGround = false;
    private bool isOnWall = false;
    private bool doubleJump = false;

    // Data indicating how to move
    private float wallJumpX;
    private Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Prepare to change velocity
        velocity = rgbd.velocity;

        // Move left or right
        if (isOnGround)
        {
            float playerInput = Input.GetAxis("Horizontal");
            velocity.x = Mathf.Clamp(velocity.x + playerInput * speed, -speed, speed);
        }
        else if (!isOnWall)
        {
            float playerInput = Input.GetAxis("Horizontal");
            velocity.x = Mathf.Clamp(velocity.x + playerInput * speed * airControl, -speed*airControl, speed*airControl);
        }

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Update velocity
        Debug.Log(velocity.x);
        rgbd.velocity = velocity;
    }

    private void Jump()
    {
        // Simple jump
        if (isOnGround)
        {
            velocity.y = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            Debug.Log("Simple Jump");
        }
        // Wall jump
        else if (isOnWall)
        {
            velocity.x = wallJumpX * 10f;
            velocity.y = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            Debug.Log("Wall Jump");
        }
        // Double jump
        else if (doubleJump)
        {
            velocity.y = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            doubleJump = false;
            Debug.Log("Double Jump");
        }
        Debug.Log(velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision2D collision)
    {
        int col_layer = collision.gameObject.layer;
        // Player is on the ground
        if (col_layer == LayerMask.NameToLayer("Ground"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.GetContact(i).normal.y >= 0.9f)
                {
                    isOnGround |= true;
                    doubleJump |= true;
                }
            }
        }
        // Player is hugging a wall
        else if (col_layer == LayerMask.NameToLayer("Wall"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.GetContact(i).normal.y <= 0.1f)
                {
                    isOnWall |= true;
                    velocity.x = 0f;
                    wallJumpX = collision.GetContact(i).normal.x;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        int col_layer = collision.gameObject.layer;
        // Player left the ground
        if (col_layer == LayerMask.NameToLayer("Ground"))
        {
            isOnGround = false;
        }
        // Player detached from a wall
        else if (col_layer == LayerMask.NameToLayer("Wall"))
        {
            isOnWall = false;
        }
    }
}
