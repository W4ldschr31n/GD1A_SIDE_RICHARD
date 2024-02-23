using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // Internal components
    private Collider2D hitbox;
    private Rigidbody2D rgbd;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<Collider2D>();
        rgbd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Retrieve the direction of the collision to bump the player the other way
            ContactPoint2D contact = collision.GetContact(0);
            collision.gameObject.GetComponent<PlayerBehaviour>().GetHit(contact.normal);
        }
    }

    public void Die()
    {
        // Disable collisions and movement
        hitbox.enabled = false;
        rgbd.simulated = false;
        animator.Play("Die");
    }

    public void Disappear()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
