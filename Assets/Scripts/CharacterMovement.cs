using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
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
    [SerializeField]
    private Animator animator;

    // State allowing movement
    private bool isOnGround = false;
    private bool isNearWall = false;
    private bool doubleJump = false;

    // Data indicating how to move
    private float wallJumpX;
    private int platformMask;

    // Start is called before the first frame update
    void Start()
    {
        platformMask = LayerMask.NameToLayer("Platform");
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public Vector2 MoveOnPlatform(float direction, Vector2 velocity)
    {
        float desiredSpeed = baseSpeed * acceleratingFactor;
        float desiredAcceleration;
        float desiredVelocity = direction * desiredSpeed;

        bool needToAccelerate = (
            desiredVelocity < 0f && velocity.x > desiredVelocity && velocity.x < 0f
            ||
            desiredVelocity > 0f && velocity.x < desiredVelocity && velocity.x > 0f
        );
        // Acceleration of the character
        if (needToAccelerate)
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

        return velocity;
    }

    public Vector2 MoveNearWall(float direction, Vector2 velocity)
    {
        if (direction == -wallJumpX && velocity.y < 0f) {
            velocity.y = -1f * Time.deltaTime;
            direction = 0f;
        }
        return MoveInAir(direction, velocity);
    }

    public Vector2 MoveInAir(float direction, Vector2 velocity)
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

        return velocity;
    }

    public Vector2 Jump(Vector2 velocity)
    {
        float jumpImpulse = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
        // Simple jump
        if (isOnGround)
        {
            velocity.y = jumpImpulse;
            animator.SetTrigger("Jumping");
        }
        // Wall jump
        else if (isNearWall)
        {
            velocity.x = wallJumpX * baseSpeed;
            velocity.y = jumpImpulse;
            animator.SetTrigger("Jumping");
        }
        // Double jump
        else if (doubleJump)
        {
            velocity.y = jumpImpulse;
            doubleJump = false;
        }

        return velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == platformMask)
        {
            HandlePlatformCollision(collision);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {        
        if (collision.gameObject.layer == platformMask)
        {
            HandlePlatformCollision(collision);
        }
    }

    private void HandlePlatformCollision(Collision2D collision)
    {
        // Loop through all tiles in collision to determine the state
        Vector2 normal;
        // Jumping while next to a wall doesn't count as leaving the tilemap collider, so we need to double check
        bool tmpIsOnGround = false;
        for(int i=0; i<collision.contactCount; i++)
        {
            normal = collision.GetContact(i).normal;
            // Character is on the ground
            if (normal.y >= 0.9f)
            {
                tmpIsOnGround = true;
                doubleJump = true;
            }
            // Character is hugging a wall
            else if (Mathf.Abs(normal.y) <= 0.1f)
            {
                isNearWall = true;
                wallJumpX = normal.x;
            }
        }
        isOnGround = tmpIsOnGround; // Only change the value if needed
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == platformMask)
        {
            HandlePlatformCollisionExit();
        }
    }

    private void HandlePlatformCollisionExit()
    {
        isNearWall = false;
        isOnGround = false;
    }

    public float GetAcceleratingFactor()
    {
        return acceleratingFactor;
    }

    public float GetDeceleratingFactor()
    {
        return deceleratingFactor;
    }

    public void SetAcceleratingFactor(float newAcceleratingFactor)
    {
        acceleratingFactor = newAcceleratingFactor;
    }

    public void SetDeceleratingFactor(float newDeceleratingFactor)
    {
        deceleratingFactor = newDeceleratingFactor;
    }

    public bool GetIsOnGround()
    {
        return isOnGround;
    }
    public bool GetIsNearWall()
    {
        return isNearWall;
    }


}
