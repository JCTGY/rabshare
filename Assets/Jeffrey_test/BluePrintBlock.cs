using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrintBlock : MonoBehaviour
{
    public bool inBluePrint;
    Collider2D InBlock;
    Collider2D mCollider;

    private void Start()
    {
        inBluePrint = false;
        InBlock = null;
        mCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        InBlock = collision;
        GameObject obj = collision.gameObject;
        if (obj.tag == "Block")
        {
            float objX = obj.transform.position.x;
            float objY = obj.transform.position.y;
            if (Mathf.Abs(objX - this.transform.position.x) < 0.3f && Mathf.Abs(objY - this.transform.position.y) < 0.3f)
                inBluePrint = true;
            else if (Mathf.Abs(objX - this.transform.position.x) > 0.3f && Mathf.Abs(objY - this.transform.position.y) > 0.3f)
                inBluePrint = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Block")
            inBluePrint = false;
    }
    private void Update()
    {
        if (InBlock != null && mCollider.bounds.Intersects(InBlock.bounds) == false && InBlock.tag == "Block")
            inBluePrint = false;
    }
}