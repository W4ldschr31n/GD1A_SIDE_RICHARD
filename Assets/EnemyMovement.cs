using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    [SerializeField]
    private Transform patrolPointStart, patrolPointEnd;
    private Transform targetPoint;

    private bool goForth = true;

    // Start is called before the first frame update
    void Start()
    {
        targetPoint = patrolPointStart;
    }

    protected override void Move()
    {
        velocity = rgbd.velocity;

        float direction = rgbd.transform.position.x < targetPoint.position.x ? 1f : -1f;
        if (Mathf.Abs(rgbd.transform.position.x - targetPoint.position.x) > 0.5f)
        {
            base.MoveOnPlatform(direction);
        }
        else
        {
            goForth = !goForth;
            targetPoint = goForth ? patrolPointStart : patrolPointEnd;
        }

        rgbd.velocity = velocity;
    }


}
