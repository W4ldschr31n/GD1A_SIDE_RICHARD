using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Parameters for movement
    [SerializeField]
    private Rigidbody2D rgbd;
    [SerializeField]
    private SpriteRenderer sprite;
    [SerializeField]
    private Transform patrolPointLeft, patrolPointRight;
    private Transform targetPoint;

    // State allowing movement
    private bool goForth = true; // Go toward the right of the screen

    // Data for movement
    private Vector2 velocity;

    // External script that implements physical movement
    [SerializeField]
    private CharacterMovement characterMovement;

    // Start is called before the first frame update
    void Start()
    {
        targetPoint = patrolPointRight;
    }

    void Update()
    {
        Move();
        sprite.flipX = !goForth;
    }

    private void Move()
    {
        velocity = rgbd.velocity;

        float direction = goForth ? 1f : -1f;
        if (Mathf.Abs(rgbd.transform.position.x - targetPoint.position.x) > 0.5f)
        {
            velocity = characterMovement.MoveOnPlatform(direction, velocity);
        }
        else
        {
            goForth = !goForth;
            targetPoint = goForth ? patrolPointRight : patrolPointLeft;
            // Enemy will turn around instantly on normal ground
            if (characterMovement.GetDeceleratingFactor() >= 1f)
            {
                velocity.x = 0f;
            }
        }
        rgbd.velocity = velocity;
    }


}
