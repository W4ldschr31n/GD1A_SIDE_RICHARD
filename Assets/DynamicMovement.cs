using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    private float speed = 4f;
    [SerializeField, Range(0f, 100f)]
    private float jumpHeight = 10f;
    [SerializeField]
    private Rigidbody2D rgbd;

    private bool doubleJump = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = 0;
        rgbd.AddForce(playerInput * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (rgbd.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                rgbd.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                doubleJump = true;
            }else if (doubleJump)
            {
                rgbd.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                doubleJump = false;
            }
        }
    }
}
