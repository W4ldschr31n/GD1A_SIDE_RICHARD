using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autel : MonoBehaviour
{
    // External components
    [SerializeField]
    private PlayerBehaviour player;

    // State
    private bool playerIsNear = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNear && Input.GetButtonDown("Use"))
        {
            player.EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerIsNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerIsNear = false;
    }
}
