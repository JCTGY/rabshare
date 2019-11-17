using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    private bool holding = false;
    private HingeJoint2D hingeJoint;

    // Start is called before the first frame update
    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && holding == false)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "Block")
            {
                holding = true;
                Debug.Log(hit.collider.gameObject.name);
                hingeJoint.enabled = true;
                hingeJoint.connectedBody = hit.collider.attachedRigidbody;
                Vector2 anchorPoint;
                anchorPoint.x = mousePos2D.x - hit.collider.transform.localPosition.x;
                anchorPoint.y = mousePos2D.y - hit.collider.transform.localPosition.y;
                anchorPoint.x /= hit.collider.transform.localScale.x;
                anchorPoint.y /= hit.collider.transform.localScale.y;
                anchorPoint = hit.collider.transform.rotation * anchorPoint;
                /*if (blockIsFlipped(hit.collider.gameObject))
                {
                    anchorPoint.x = -anchorPoint.x;
                    anchorPoint.y = -anchorPoint.y;
                }*/
                hingeJoint.connectedAnchor = anchorPoint;
            }
        }
        else if (Input.GetMouseButtonDown(0) && holding)
        {
            hingeJoint.enabled = false;
            holding = false;
        }

        if (holding)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            this.gameObject.transform.position = mousePos;
        }
    }

    private bool blockIsFlipped(GameObject block)
    {
        Vector3 angles = block.transform.rotation.eulerAngles;
        if (angles.z >= 180.0f && angles.z < 360.0f)
            return true;
        return false;
    }
}
