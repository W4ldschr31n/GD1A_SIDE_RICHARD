using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private Collider2D hitbox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            ContactPoint2D contact = collision.GetContact(0);
            // If the player is above the enemy, the latter dies
            if (contact.normal.y <= -0.7)
            {
                Die();
            }
            // If the player is below or next to the enemy, the former gets hit
            else
            {
                collision.gameObject.GetComponent<PlayerBehaviour>().GetHit(contact.normal);
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
