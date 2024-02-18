using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour
{
    [SerializeField]
    private Collider2D hitbox;
    [SerializeField]
    private SpriteRenderer sprite;

    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Transform attackSpot)
    {
        if (canAttack)
        {
            transform.position = attackSpot.position;
            hitbox.enabled = true;
            sprite.enabled = true;
            canAttack = false;
            // Animation
        }
    }

    public void Reset()
    {
        hitbox.enabled = false;
        sprite.enabled = false;
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<EnemyBehaviour>().Die();
    }
}
