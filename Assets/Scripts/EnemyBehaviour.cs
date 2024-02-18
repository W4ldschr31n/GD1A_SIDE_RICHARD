using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private Collider2D hitbox;
    [SerializeField]
    private Rigidbody2D rgbd;

    [SerializeField]
    private Animator animator;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            collision.gameObject.GetComponent<PlayerBehaviour>().GetHit(contact.normal);
        }
    }

    public void Die()
    {
        hitbox.enabled = false;
        rgbd.simulated = false;
        animator.SetTrigger("Die");
    }

    public void Disappear()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
