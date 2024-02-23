using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    // External components
    [SerializeField]
    private GameObject gate;
    [SerializeField]
    private PlayerBehaviour player;

    // State
    private bool playerIsNearby = false;
    private bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsNearby && !isActivated && Input.GetButtonDown("Use"))
        {
            Destroy(gate);
            isActivated = true;
            GetComponent<SpriteRenderer>().flipY = true;
            player.ShowOverheadMessage("Un m�canisme se fait entendre. Une porte semble s'�tre ouverte.", 3);
            // Doesn't seem to work :(
            GetComponent<MessageWhenNearby>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerIsNearby = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerIsNearby = false;
    }
}
