using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    // Components
    [SerializeField]
    private Rigidbody2D rgbd;
    [SerializeField]
    private SpriteRenderer sprite;

    // Composition
    [SerializeField]
    private PlayerHealthBar healthBar;
    [SerializeField]
    private Text writingsFoundText;
    [SerializeField]
    private OverheadText overheadText;

    // States allowing behaviour
    private bool canGetHit = true;

    // Data indicating how to behave
    private float iFrames = 1.5f;
    private float remainingIFrames;
    private int maxHealth = 3;
    [SerializeField]
    private int health = 3;

    private List<Item> inventory = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        TickTimers();
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Inventaire :");
            foreach (var i in inventory)
            {
                Debug.Log(i.letter);
                Debug.Log(i.flavorText);
            }
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
            sprite.color = Color.white;
            rgbd.excludeLayers = LayerMask.GetMask("Nothing");
        }
    }

    public void GetHit(Vector2 normal)
    {
        if (canGetHit)
        {
            canGetHit = false;
            rgbd.excludeLayers = LayerMask.GetMask("Enemy");
            remainingIFrames = iFrames;
            sprite.color = Color.gray;
            rgbd.velocity = new Vector2(normal.x * -6, 6);
            LoseHealth();
        }
    }

    public void Die()
    {
        healthBar.Die();
        transform.position = Vector2.zero;
        health = maxHealth;
    }

    public void FullHeal()
    {
        health = maxHealth;
        healthBar.GainHealth();
    }

    public void LoseHealth()
    {
        health = Mathf.Max(health - 1, 0);
        if (IsAlive())
        {
            healthBar.LoseHealth();
        }
        else
        {
            Die();
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void AddItem(Item item)
    {
        inventory.Add(item);
        overheadText.ShowMessage(item.flavorText, 3);
        writingsFoundText.text = "Écrits trouvés : " + inventory.Count;
    }
}
