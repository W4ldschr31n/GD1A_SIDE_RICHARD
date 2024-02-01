using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Parameters for movement
    [SerializeField]
    private Rigidbody2D rgbd;
    [SerializeField]
    private Transform patrolPointStart, patrolPointEnd;
    private Transform targetPoint;

    // State allowing movement
    private bool goForth = true;

    // Data for movement
    private Vector2 velocity;

    // External script that implements physical movement
    [SerializeField]
    private CharacterMovement characterMovement;

    // Start is called before the first frame update
    void Start()
    {
        targetPoint = patrolPointStart;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        velocity = rgbd.velocity;

        float direction = rgbd.transform.position.x < targetPoint.position.x ? 1f : -1f;
        if (Mathf.Abs(rgbd.transform.position.x - targetPoint.position.x) > 0.5f)
        {
            velocity = characterMovement.MoveOnPlatform(direction, velocity);
        }
        else
        {
            goForth = !goForth;
            targetPoint = goForth ? patrolPointStart : patrolPointEnd;
            // Enemy will turn around instantly on normal ground
            if (characterMovement.GetDeceleratingFactor() >= 1f)
            {
                velocity.x = 0f;
            }
        }

        rgbd.velocity = velocity;
    }


}
