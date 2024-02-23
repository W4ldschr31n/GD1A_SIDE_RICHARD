using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Parameters
    [SerializeField]
    private float parallaxFactorX, parallaxFactorY;
    [SerializeField]
    private float repeatDistance; // Set to zero to base on image width

    // External components
    [SerializeField]
    private Camera mainCamera;

    // Data
    private Vector2 startPosition;
    private const int PPU = 32; // Pixels per unit
        
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        if(repeatDistance <= 0)
        {
            repeatDistance = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateParallaxX();
        
        if (parallaxFactorY >= 0)
        {
            UpdateParallaxY();
        }
    }

    private void UpdateParallaxX()
    {
        // Move relatively to the camera, inv. proportionaly to the parallax factor (1 = follow camera, 0 = don't move)
        float distance = mainCamera.transform.position.x * parallaxFactorX;
        Vector3 newPosition = new Vector3(startPosition.x + distance, transform.position.y, transform.position.z);
        transform.position = PixelPerfectClamp(newPosition);

        // Move the sprite when it reaches the middle of screen
        float breakPoint = mainCamera.transform.position.x * (1 - parallaxFactorX);
        if (breakPoint > startPosition.x + (repeatDistance / 2))
        {
            startPosition.x += repeatDistance;
        }
        else if (breakPoint < startPosition.x - (repeatDistance / 2))
        {
            startPosition.x -= repeatDistance;
        }
    }


    private void UpdateParallaxY()
    {
        float distance = mainCamera.transform.position.y * parallaxFactorY;
        Vector3 newPosition = new Vector3(transform.position.x, startPosition.y + distance, transform.position.z);
        transform.position = PixelPerfectClamp(newPosition);
        // Don't move the sprite if it leaves the screen
    }

    private Vector3 PixelPerfectClamp(Vector3 vectorToClamp)
    {
        // Prevent jittering by clamping the new position relatively to the PPU
        Vector3 vectorInPixels = new Vector3(
            Mathf.CeilToInt(vectorToClamp.x * PPU),
            Mathf.CeilToInt(vectorToClamp.y * PPU),
            Mathf.CeilToInt(vectorToClamp.z * PPU)
        );
        return vectorInPixels / PPU;
    }
}
