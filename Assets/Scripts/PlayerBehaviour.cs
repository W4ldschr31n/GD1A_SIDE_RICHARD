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
    [SerializeField]
    private Animator animator;

    // Composition
    [SerializeField]
    private PlayerHealthBar healthBar;
    [SerializeField]
    private Text writingsFoundText;
    [SerializeField]
    private DisplayableTextPanel overheadTextPanel, gameStateTextPanel;
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private Whip whip;

    // States allowing behaviour
    private bool canGetHit = true;
    private bool hasWhip = true;

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
        // Tick down timers
        TickTimers();

        // Update behaviour with inputs
        if (IsAlive())
        {
            playerMovement.HandleInputs();
            playerMovement.UpdateAnimation();
            if (hasWhip)
            {
                whip.HandleInputs();
            }
        }
        // Reset player position
        if (Input.GetKeyDown(KeyCode.R))
            Respawn();
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
            // Remove enemy layer from excluded
            rgbd.excludeLayers &= ~LayerMask.GetMask("Enemy");
        }
    }

    private void Respawn()
    {
        HideGameStateMessage();
        rgbd.transform.position = Vector2.zero;
        rgbd.velocity = Vector2.zero;
        animator.SetBool("Alive", true);
        FullHeal();
    }

    public void GetHit(Vector2 normal)
    {
        if (canGetHit)
        {
            canGetHit = false;
            // Add enemy layer to excluded
            rgbd.excludeLayers |= LayerMask.GetMask("Enemy");
            remainingIFrames = iFrames;
            sprite.color = Color.gray;
            // Get bumped in the opposite direction
            rgbd.velocity = new Vector2(normal.x * -6, 6);
            LoseHealth();
        }
    }

    public void Die()
    {
        health = 0; // To make sure we have no health in case of instant death
        healthBar.Die();
        animator.SetTrigger("Die");
        animator.SetBool("Alive", false);
        GetComponent<Grappin>().Reset();
        ShowGameStateMessage("Vous êtes mort, appuyez sur R/Start pour recommencer au dernier checkpoint.");

    }

    public void FullHeal()
    {
        // Don't play animation if already full life
        if (health < maxHealth)
        {
            health = maxHealth;
            healthBar.GainHealth();
        }
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
        ShowOverheadMessage(item.flavorText, 3);
        writingsFoundText.text = "Écrits trouvés : " + inventory.Count;
    }

    public void PickUpWhip()
    {
        hasWhip = true;
        ShowOverheadMessage("Vous avez trouvé le fouet. Click gauche pour attaquer !", 4);
    }

    public void ShowOverheadMessage(string message, float duration = 0)
    {
        overheadTextPanel.ShowMessage(message, duration);
    }

    public void HideOverheadMessage()
    {
        overheadTextPanel.Hide();
    }

    public void ShowGameStateMessage(string message, float duration = 0)
    {
        gameStateTextPanel.ShowMessage(message, duration);
    }

    public void HideGameStateMessage()
    {
        gameStateTextPanel.Hide();
    }

}
