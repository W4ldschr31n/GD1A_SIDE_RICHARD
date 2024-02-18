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
    [SerializeField]
    private Transform attackSpotLeft, attackSpotRight;
    [SerializeField]
    private SpriteRenderer playerSprite;
    [SerializeField]
    private Whip whip;

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
        HandleWhipInput();
        HandleGrapplinInput();
        if (line.enabled)
        {
            line.SetPosition(0, gameObject.transform.position);
            line.SetPosition(1, grapplePoint);
        }

        
    }

    private void HandleWhipInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            whip.Attack(playerSprite.flipX ? attackSpotLeft : attackSpotRight);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            whip.Reset();
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
