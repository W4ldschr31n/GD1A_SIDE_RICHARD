using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageWhenNearby : MonoBehaviour
{
    public string message;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<OverheadText>().ShowMessage(message);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent <OverheadText>().Hide();
    }
}
