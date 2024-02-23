using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // External components
    [SerializeField]
    private Transform exitLocation;
    [SerializeField]
    private Rigidbody2D playerBody;

    // State
    private bool isPlayerNearby = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNearby && Input.GetButtonDown("Use")){
            playerBody.transform.position = exitLocation.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerNearby = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerNearby = false;
    }
}
