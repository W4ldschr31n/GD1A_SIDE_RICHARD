using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour
{
    [SerializeField]
    private Collider2D hitbox;
    [SerializeField]
    private Transform attackSpotLeft, attackSpotRight;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer playerSprite;

    public bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        hitbox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Align the weapon with the direction the player is facing
        transform.position = playerSprite.flipX ? attackSpotLeft.position : attackSpotRight.position;
    }

    public void HandleInputs()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            animator.SetTrigger("Attack");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<EnemyBehaviour>().Die();
    }
}
