using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPosition;
    public float parallaxFactor;
    public float repeatDistance; // Set to zero to base on image width
    [SerializeField]
    private Camera mainCamera;

    // Pixels per unit
    private const int PPU = 32;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position.x;
        if(repeatDistance <= 0)
        {
            repeatDistance = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move relatively to the camera, inv. proportionnaly to the parallax factor (0 = follow camera, 1 = don't move)
        float distance = mainCamera.transform.position.x * parallaxFactor;
        Vector3 newPosition = new Vector3(startPosition + distance, transform.position.y, transform.position.z);
        transform.position = PixelPerfectClamp(newPosition);

        // Move the sprite when it reaches the middle of screen
        float temp = mainCamera.transform.position.x * (1 - parallaxFactor);
        if (temp > startPosition + (repeatDistance / 2))
        {
            startPosition += repeatDistance;
        }
        else if (temp < startPosition - (repeatDistance / 2))
        {
            startPosition -= repeatDistance;
        }
    }


    private Vector3 PixelPerfectClamp(Vector3 vectorToClamp)
    {
        Vector3 vectorInPixels = new Vector3(
            Mathf.CeilToInt(vectorToClamp.x * PPU),
            Mathf.CeilToInt(vectorToClamp.y * PPU),
            Mathf.CeilToInt(vectorToClamp.z * PPU)
        );
        return vectorInPixels / PPU;
    }
}
