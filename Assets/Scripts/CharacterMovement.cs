using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Parameters
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

    // Internal components
    private Rigidbody2D rgbd;
    private Animator animator;

    // State
    private bool isOnGround = false;
    private bool isNearWall = false;
    private bool doubleJump = false;

    // Data
    private float wallJumpX;
    private int platformMask;

    // Start is called before the first frame update
    void Start()
    {
        platformMask = LayerMask.NameToLayer("Platform");
        rgbd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public Vector2 MoveOnPlatform(float direction, Vector2 velocity)
    {
        // Where do we want to move
        float desiredSpeed = baseSpeed * acceleratingFactor;
        float desiredAcceleration;
        float desiredVelocity = direction * desiredSpeed;
        // Are we moving and not at max speed
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
        // Character slides if it moves against the wall
        if (direction == -wallJumpX && velocity.y < 0f) {
            velocity.y = -1f * Time.deltaTime;
            direction = 0f;
        }
        // If not sliding, fall normally anyway
        return MoveInAir(direction, velocity);
    }

    public Vector2 MoveInAir(float direction, Vector2 velocity)
    {
        // Where do we want to move
        float desiredSpeed = baseSpeed * airControl;
        float desiredAcceleration;
        float desiredVelocity = direction * desiredSpeed;
        // Are we moving and not at max speed
        bool needToAccelerate = (
            desiredVelocity < 0f && velocity.x > desiredVelocity
            ||
            desiredVelocity > 0f && velocity.x < desiredVelocity
        );
        // Acceleration of the character
        if (needToAccelerate)
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
        // Derived from the following (y=jumpHeightDesired; t->time; v(t)->speed; j->jumpForce; g->gravity; h->currentHeight)
        // v(0)=j; v(t)=j-g.t; y=0 <=> t=0; y=h(t) <=> v(t)=0 <=> j-g.t=0 <=> t=j/g
        // vavg=(vstart+vend)/2 (average speed is the mathematical average of initial speed and final speed)
        // vavg(t)=(v(0)+v(t))/2 <=> vavg(t)=(j+j-g.t)/2 <=> vavg(t)=j-g.t/2
        // distance=speed.time <=> h(t)=vavg(t).t; <=> h(t)=j.t-g.t.t/2
        // y=h(t) <=> t=j/g <=> y=j.j/g-g.j/g.j/g/2 <=> y=j.j/g/2 <=> j.j=2.y.g
        // <=> j = sqrt(2.g.y); or sqrt(2.-g.y) when g < 0 (like here)
        float jumpImpulse = Mathf.Sqrt(2f * -Physics2D.gravity.y * jumpHeight);


        // Simple jump
        if (isOnGround)
        {
            velocity.y = jumpImpulse;
            animator.Play("Jump");
        }
        // Wall jump
        else if (isNearWall)
        {
            velocity.x = wallJumpX * baseSpeed;
            velocity.y = jumpImpulse;
            animator.Play("Jump");
        }
        // Double jump
        else if (doubleJump)
        {
            velocity.y = jumpImpulse;
            doubleJump = false;
            animator.Play("Jump");
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
        // Jumping while next to a wall doesn't count as leaving the tilemap collider, so we need to double check
        bool tmpIsOnGround = false;
        // Loop through all tiles in collision to determine the state with the normal
        Vector2 normal;
        for(int i=0; i<collision.contactCount; i++)
        {
            normal = collision.GetContact(i).normal;
            // Character is on the ground (collision from below)
            if (normal.y >= 0.9f)
            {
                tmpIsOnGround = true;
                doubleJump = true;
            }
            // Character is hugging a wall (collision from the side)
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
        // Character is leaving the tileset as a whole, so it's not touching anything
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
