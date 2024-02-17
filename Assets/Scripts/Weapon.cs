using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private DistanceJoint2D joint;
    [SerializeField]
    private float length;
    [SerializeField]
    private LineRenderer line;

    private Vector2 grapplePoint;

    // Start is called before the first frame update
    private void Start()
    {
        joint.enabled = false;
        joint.distance = length;
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity
            );
            if (hit.collider != null)
            {
                grapplePoint = hit.point;
                joint.connectedAnchor = grapplePoint;
                joint.enabled = true;
                line.enabled = true;
            }
        }

        if (line.enabled)
        {
            line.SetPosition(0, gameObject.transform.position);
            line.SetPosition(1, grapplePoint);
        }

        if (Input.GetMouseButtonUp(0))
        {
            joint.enabled = false;
            line.enabled = false;
        }
    }
}
