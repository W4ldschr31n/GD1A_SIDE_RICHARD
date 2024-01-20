using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
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
    protected Rigidbody2D rgbd;

    // State allowing movement
    protected bool isOnGround = false;
    protected bool isNearWall = false;
    protected bool doubleJump = false;

    // Data indicating how to move
    protected float wallJumpX;
    protected Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Prepare to change velocity
        velocity = rgbd.velocity;

        Move();

        // Update velocity
        rgbd.velocity = velocity;
    }

    protected abstract void Move();

    protected void MoveOnPlatform(float direction)
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
    }

    protected void MoveNearWall(float direction)
    {
        if (direction == -wallJumpX) {
            velocity.y = -1f * Time.deltaTime;
            direction = 0f;
        }
        MoveInAir(direction);
    }

    protected void MoveInAir(float direction)
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
    }

    protected void Jump()
    {
        float jumpImpulse = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
        // Simple jump
        if (isOnGround)
        {
            velocity.y = jumpImpulse;
        }
        // Wall jump
        else if (isNearWall)
        {
            velocity.x = wallJumpX * baseSpeed;
            velocity.y = jumpImpulse;
        }
        // Double jump
        else if (doubleJump)
        {
            velocity.y = jumpImpulse;
            doubleJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision, true);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollision(collision, false);
    }

    private void HandleCollision(Collision2D collision, bool isEnterCollision)
    {
        // Player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.GetContact(i).normal.y >= 0.9f)
                {
                    isOnGround = true;
                    doubleJump = true;
                    break;
                }
            }
        }
        // Player is hugging a wall
        else if (collision.gameObject.CompareTag("Wall"))
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (Mathf.Abs(collision.GetContact(i).normal.y) <= 0.1f)
                {
                    isNearWall = true;
                    wallJumpX = collision.GetContact(i).normal.x;
                    // Hitting a wall stops the movement
                    if (isEnterCollision)
                    {
                        velocity.x = 0f;
                    }
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Player left the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
        // Player detached from a wall
        else if (collision.gameObject.CompareTag("Wall"))
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
