using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMovement : MonoBehaviour
{
    // Parameters for movement
    [SerializeField, Range(0f, 10f)]
    private float maxAcceleration = 10f;
    [SerializeField, Range(0f, 10f)]
    private float maxSpeed = 10f;
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
    private float wallJumpTimer = 0.3f;
    private float wallJumpRemainingTime = 0f;

    // Data indicating how to move
    private float wallJumpX;
    private Vector2 velocity;
    private float desiredVelocity;

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
        float direction = Input.GetKey(KeyCode.LeftArrow) ? -1f : (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);
        float desiredSpeed;
        float desiredAcceleration;
        // Moving on the ground
        if (isOnGround)
        {
            desiredSpeed = maxSpeed;
            desiredAcceleration = maxAcceleration;
        }
        // Moving on a wall
        else if (isOnWall && wallJumpRemainingTime <= 0f)
        {
            if (direction == -wallJumpX)
            {
                desiredSpeed = 0f;
                desiredAcceleration = 0f;
                velocity.y = -1f * Time.deltaTime;
            }
            else
            {
                desiredSpeed = maxSpeed * airControl;
                desiredAcceleration = maxAcceleration * airControl;
            }
        }
        // Moving in the air
        else
        {
            if (direction == 0f)
            {
                direction = rgbd.velocity.x >= 0f ? 1f : -1f;
                desiredSpeed = direction * rgbd.velocity.x;
            }
            else
            {
                desiredSpeed = maxSpeed * airControl;
            }
            desiredAcceleration = maxAcceleration * airControl;// * Time.deltaTime;
        }
        desiredVelocity = direction * desiredSpeed;
        if (wallJumpRemainingTime > 0f)
        {
            wallJumpRemainingTime -= Time.deltaTime;
        }
        else if (
            desiredVelocity <= 0f && velocity.x > desiredVelocity 
            ||
            desiredVelocity >= 0f && velocity.x < desiredVelocity)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity, desiredAcceleration);
        }

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Update velocity
        rgbd.velocity = velocity;

        if (Input.GetKeyDown(KeyCode.R))
            transform.position = Vector2.zero;
        
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
            velocity.x = wallJumpX * maxSpeed;
            velocity.y = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            wallJumpRemainingTime = wallJumpTimer;
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

    private void OnCollisionStay2D(Collision2D collision)
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
            if (collision.gameObject.CompareTag("Ice"))
            {
                maxAcceleration = maxSpeed / 2;
            }
            else
            {
                maxAcceleration = maxSpeed;
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
