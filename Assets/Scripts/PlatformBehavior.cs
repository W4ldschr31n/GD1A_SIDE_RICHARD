using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    [SerializeField]
    private float acceleratingFactor = 1f, deceleratingFactor = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When a character gets on a platform, its movement is regulated by the type of the platform
        collision.gameObject.SendMessage("SetAcceleratingFactor", acceleratingFactor);
        collision.gameObject.SendMessage("SetDeceleratingFactor", deceleratingFactor);
    }
}
