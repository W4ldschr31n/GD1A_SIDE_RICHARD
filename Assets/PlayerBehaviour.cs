using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Parameters for behaviour
    [SerializeField]
    private Rigidbody2D rgbd;
    [SerializeField]
    private Collider2D hitbox;
    [SerializeField]
    private SpriteRenderer sprite;

    // States allowing behaviour
    private bool canGetHit = true;

    // Data indicating how to behave
    private float iFrames = 1.5f;
    private float remainingIFrames;
    private int maxHealth = 3;
    [SerializeField]
    private int health = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        TickTimers();

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

    public void GetHit(int damage, Vector2 normal)
    {
        if (canGetHit)
        {
            canGetHit = false;
            remainingIFrames = iFrames;
            sprite.color = Color.gray;
            rgbd.velocity = new Vector2(normal.x * -10, 10);
            LoseHealth(damage);
        }
    }

    private void Die()
    {
        transform.position = Vector2.zero;
        health = maxHealth;
    }

    public void GainHealth(int heal)
    {
        health = Mathf.Min(maxHealth, health + heal);
    }

    public void LoseHealth(int damage)
    {
        health = Mathf.Max(health-damage, 0);
        if (!IsAlive())
        {
            Die();
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }
}
