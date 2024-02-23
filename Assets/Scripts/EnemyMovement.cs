using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Internal components
    private Rigidbody2D rgbd;
    private SpriteRenderer sprite;
    private CharacterMovement characterMovement;
    [SerializeField]
    private Transform patrolPointLeft, patrolPointRight;

    // State
    private bool goForth = true; // Go toward the right of the screen

    // Data
    private Vector2 velocity;
    private Transform targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        targetPoint = patrolPointRight;
        rgbd = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        // Prepare to move the body
        velocity = rgbd.velocity;
        float direction = goForth ? 1f : -1f;

        // Still far away from the current patrol point
        if (Mathf.Abs(rgbd.transform.position.x - targetPoint.position.x) > 0.5f)
        { 
            velocity = characterMovement.MoveOnPlatform(direction, velocity);
        }
        // Has to turn back
        else
        {
            goForth = !goForth;
            sprite.flipX = !sprite.flipX;
            targetPoint = goForth ? patrolPointRight : patrolPointLeft;
            // Enemy will turn around instantly on normal ground, but intertia will occur on slippy ground
            if (characterMovement.GetDeceleratingFactor() >= 1f)
            {
                velocity.x = 0f;
            }
        }
        // Effectively move the body
        rgbd.velocity = velocity;
    }


}
