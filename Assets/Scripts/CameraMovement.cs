using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // External components
    [SerializeField]
    private Transform target;

    // Data
    public float offsetY = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            target.position.x,
            target.position.y + offsetY,
            transform.position.z
        );
    }
}
