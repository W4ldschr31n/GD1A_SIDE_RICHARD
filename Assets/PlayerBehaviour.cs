using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Parameters for behaviour
    [SerializeField]
    private Rigidbody2D rgbd;
    [SerializeField]
    private SpriteRenderer sprite;

    // States allowing behaviour
    private bool canGetHit = true;

    // Data indicating how to behave
    private float iFrames = 1.5f;
    private float remainingIFrames;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        TickTimers();

        if (Input.GetKeyDown(KeyCode.F) && canGetHit)
        {
            GetHit(1);
        }
    }

    private void TickTimers()
    {
        // Tick down all timers
        if (remainingIFrames > 0)
        {
            remainingIFrames -= Time.deltaTime;
        }
        // Handle timers that are done
        if (remainingIFrames <= 0)
        {
            canGetHit = true;
            sprite.color = Color.green;
        }
    }

    private void GetHit(int damage)
    {
        Debug.Log("GTE HIT " + damage);
        if(damage > 1)
        {
            Die();
        }
        else
        {
            canGetHit = false;
            remainingIFrames = iFrames;
            rgbd.velocity = new Vector2(-10, 10);
            sprite.color = Color.gray;
        }
    }

    private void Die()
    {
        transform.position = Vector2.zero;
    }
}
