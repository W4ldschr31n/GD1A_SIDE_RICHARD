using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappin : MonoBehaviour
{
    // IMPLEMENTATION IS NOT FINISHED
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
        joint.distance = length;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        HandleGrapplinInput();
        if (line.enabled)
        {
            line.SetPosition(0, gameObject.transform.position);
            line.SetPosition(1, grapplePoint);
        }

        
    }

    public void Reset()
    {
        joint.enabled = false;
        line.enabled = false;
    }

    private void HandleGrapplinInput()
    {
        // Launch grapplin
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Platform")
            );
            if (hit.collider != null)
            {
                grapplePoint = hit.point;
                joint.connectedAnchor = grapplePoint;
                joint.enabled = true;
                line.enabled = true;
            }
        }
        // Remove grapplin
        if (Input.GetMouseButtonUp(1))
        {
            Reset();
        }
    }
}
