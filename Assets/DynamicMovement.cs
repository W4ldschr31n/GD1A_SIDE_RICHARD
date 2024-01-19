using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMovement : MonoBehaviour
{
    // Parameters for movement
    [SerializeField, Range(0f, 100f)]
    private float baseAcceleration = 20f;
    [SerializeField, Range(0f, 100f)]
    private float baseSpeed = 10f;
    [SerializeField, Range(0f, 10f)]
    private float jumpHeight = 2f;
    [SerializeField, Range(0f, 1f)]
    private float airControl = 0.5f;
    [SerializeField]
    private float acceleratingFactor = 1f;
    [SerializeField]
    private float deceleratingFactor = 1f;
    [SerializeField]
    private Rigidbody2D rgbd;

    // State allowing movement
    private bool isOnGround = false;
    private bool isNearWall = false;
    private bool doubleJump = false;

    // Data indicating how to move
    private float wallJumpX;
    private Vector2 velocity;
    private float currentAcceleration = 0f;
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

        // Update velocity
        rgbd.velocity = velocity;

        if (Input.GetKeyDown(KeyCode.R))
            transform.position = Vector2.zero;
        
    }

    private void MoveOnPlatform(float direction)
    {
        float desiredSpeed = baseSpeed * acceleratingFactor;
        float desiredAcceleration;
        float desiredVelocity = direction * desiredSpeed;

        bool wantToMove = direction != 0f;
        bool needToAccelerate = (
            desiredVelocity < 0f && velocity.x > desiredVelocity
            ||
            desiredVelocity > 0f && velocity.x < desiredVelocity
        );
        // Acceleration of the character
        if (wantToMove && needToAccelerate)
        {
            desiredAcceleration = baseAcceleration * acceleratingFactor;
        }
        // Deceleration of the character
        else
        {
            desiredAcceleration = baseAcceleration * deceleratingFactor;
        }
        // Move the character
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity, desiredAcceleration * Time.deltaTime);
        Debug.Log("velocity X platform =" + velocity.x);
    }

    private void MoveNearWall(float direction)
    {
        if (direction == -wallJumpX) {
            velocity.y = -1f * Time.deltaTime;
            direction = 0f;
        }
        MoveInAir(direction);
    }

    private void MoveInAir(float direction)
    {
        float desiredSpeed = baseSpeed * airControl;
        float desiredAcceleration;
        float desiredVelocity = direction * desiredSpeed;

        bool wantToMove = direction != 0f;
        bool needToAccelerate = (
            desiredVelocity < 0f && velocity.x > desiredVelocity
            ||
            desiredVelocity > 0f && velocity.x < desiredVelocity
        );
        // Acceleration of the character
        if (wantToMove && needToAccelerate)
        {
            desiredAcceleration = baseAcceleration * airControl;
        }
        // Full inertia in air
        else
        {
            desiredAcceleration = 0f;
        }
        // Move the character
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity, desiredAcceleration * Time.deltaTime);
        Debug.Log("velocity X air =" + velocity.x);
    }

    private void Jump()
    {
        float jumpImpulse = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
        // Simple jump
        if (isOnGround)
        {
            velocity.y = jumpImpulse;
            Debug.Log("Simple Jump");
        }
        // Wall jump
        else if (isNearWall)
        {
            velocity.x = wallJumpX * baseSpeed;
            velocity.y = jumpImpulse;
            Debug.Log("Wall Jump");
        }
        // Double jump
        else if (doubleJump)
        {
            velocity.y = jumpImpulse;
            doubleJump = false;
            Debug.Log("Double Jump");
        }
        Debug.Log("Velocity jump =" + velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision, true);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollision(collision, false);
    }

    private void HandleCollision(Collision2D collision, bool IsEnterCollision)
    {
        int col_layer = collision.gameObject.layer;
        // Player is on the ground
        if (col_layer == LayerMask.NameToLayer("Ground"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.GetContact(i).normal.y >= 0.9f)
                {
                    isOnGround = true;
                    doubleJump = true;
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
                    isNearWall = true;
                    wallJumpX = collision.GetContact(i).normal.x;
                    // Hitting a wall stops the movement
                    if (IsEnterCollision)
                    {
                        velocity.x = 0f;
                    }
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
            isNearWall = false;
        }
    }

    public void setAcceleratingFactor(float newAcceleratingFactor)
    {
        acceleratingFactor = newAcceleratingFactor;
    }

    public void setDeceleratingFactor(float newDeceleratingFactor)
    {
        deceleratingFactor = newDeceleratingFactor;
    }
}
