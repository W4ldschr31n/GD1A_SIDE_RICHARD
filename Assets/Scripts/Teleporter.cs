using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Transform exitLocation;
    [SerializeField]
    private Rigidbody2D playerBody;

    private bool canInteract = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract && Input.GetButtonDown("Fire1")){
            playerBody.transform.position = exitLocation.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canInteract = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;
    }
}
