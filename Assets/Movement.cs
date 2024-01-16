using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 10f;
    [SerializeField]
    private Rigidbody2D rgbd;
    private Vector3 velocity, desiredVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // On récupère l'utilisation des flèches directionnelles ou du joystick
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        // On convertit les entrées en vecteur de direction, avec une longueur max de 1
        Vector3 direction = Vector3.ClampMagnitude(playerInput, 1f);
        desiredVelocity = direction * maxSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = rgbd.velocity;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
        rgbd.velocity = velocity;
    }
}
