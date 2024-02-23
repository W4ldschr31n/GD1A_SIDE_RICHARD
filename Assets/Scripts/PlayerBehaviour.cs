using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PlayerBehaviour : MonoBehaviour
{
    // Internal components
    private Rigidbody2D rgbd;
    private SpriteRenderer sprite;
    private Animator animator;
    private PlayerHealthBar healthBar;
    private PlayerMovement playerMovement;
    [SerializeField]
    private DisplayableTextPanel overheadTextPanel, gameStateTextPanel;

    // External components
    [SerializeField]
    private Text writingsFoundText;
    [SerializeField]
    private Whip whip;

    // State
    private bool canGetHit = true;
    private bool hasWhip = false;

    // Data
    private float iFrames = 1.5f;
    private float remainingIFrames;
    private Vector2 respawnPosition;
    private int maxHealth = 3;
    private int health;

    private List<Item> inventory = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        rgbd = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthBar = GetComponent<PlayerHealthBar>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        TickDownTimer();

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
        if (Input.GetButtonDown("Restart"))
            Respawn();
    }

    private void TickDownTimer()
    {
        // Tick down timer with ellapsed time
        if (remainingIFrames > 0)
        {
            remainingIFrames -= Time.deltaTime;
        }
        // Handle timed out action
        else
        {
            // Player sprite and collision return to normal state
            canGetHit = true;
            sprite.color = Color.white;
            // Remove enemy layer from excluded
            rgbd.excludeLayers &= ~LayerMask.GetMask("Enemy");
        }
    }

    private void Respawn()
    {
        // Hide any open message
        HideOverheadMessage();
        HideGameStateMessage();
        // Relocate player to store position with no movement
        rgbd.transform.position = respawnPosition;
        rgbd.velocity = Vector2.zero;
        // Make sure player is healthy
        animator.SetBool("Alive", true);
        FullHeal();
    }

    public void GetHit(Vector2 normal)
    {
        if (canGetHit)
        {
            LoseHealth();
            canGetHit = false;
            // Add enemy layer to excluded
            rgbd.excludeLayers |= LayerMask.GetMask("Enemy");
            // Edit sprite for invulerability frames
            remainingIFrames = iFrames;
            sprite.color = Color.gray;
            // Get bumped in the opposite direction
            rgbd.velocity = new Vector2(normal.x * -6, 6);
        }
    }

    public void Die()
    {
        healthBar.Die();
        animator.Play("Die");
        animator.SetBool("Alive", false);
        GetComponent<Grappin>().Reset(); // Grappin is still work in progress
        ShowGameStateMessage("Vous êtes mort, appuyez sur R/Start pour recommencer au dernier checkpoint.");
        // To make sure we have no health in case of instant death
        health = 0;

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
        string messageToDisplay = $"{item.flavorText} <b>~{item.letter}</b>";
        ShowOverheadMessage(messageToDisplay, 3);
        writingsFoundText.text = $"Écrits trouvés : {inventory.Count}";
    }

    public void PickUpWhip()
    {
        hasWhip = true;
        ShowOverheadMessage("Vous avez trouvé le fouet. Click gauche pour attaquer !", 4);
    }

    public void SetRespawnPosition(Vector2 newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
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

    public void EndGame()
    {
        string indices = "aucun";
        if(inventory.Count > 0)
        {
            // Get all letters of the items found
            indices = $"({inventory.Select(x => x.letter).Aggregate((current, next) => current + "," + next)})";
        }
        string progression = $"Écrits trouvés : {inventory.Count}/8\nIndices rassemblés : {indices}";
        string result;
        // "Win" condition
        if (inventory.Count >= 8)
        {
            result = "Vous avez trouvé la clé. ENOCHIAN. Le rituel peut s'accomplir. Vous avez gagné.";
        }
        else
        {
            result = "Il vous manque des indices. Vous ne pouvez pas accomplir le rituel";
        }
        ShowGameStateMessage(
            $"{progression}\n{result}", 5
        );

    }

}
