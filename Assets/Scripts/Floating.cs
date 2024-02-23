using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    // Parameters
    [SerializeField]
    private float amplitude;

    // Data
    private float originalY;

    // Start is called before the first frame update
    void Start()
    {
        // Don't forget where we started
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Ride along the sine wave
        float newY = originalY + Mathf.Sin(Time.time) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
