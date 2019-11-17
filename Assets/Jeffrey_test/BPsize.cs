using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPsize : MonoBehaviour
{
    // Start is called before the first frame update

    Collider2D m_Collider;
    Collider2D InBlock;
    Vector3 m_Size;
    Vector2 pointStart;
    Vector2 pointEnd;

    void Start()
    {
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<Collider2D>();
        float widthCollider = m_Collider.GetComponent<SpriteRenderer>().bounds.size.x;
        float heightCollider = m_Collider.GetComponent<SpriteRenderer>().bounds.size.y;
        pointStart = new Vector2(m_Collider.gameObject.transform.position.x - widthCollider / 2, m_Collider.gameObject.transform.position.y - heightCollider / 2);
        pointEnd = new Vector2(m_Collider.gameObject.transform.position.x + widthCollider / 2, m_Collider.gameObject.transform.position.y + heightCollider / 2);
        m_Size = m_Collider.bounds.size;


        //Output to the console the size of the Collider volume
        // Debug.Log("Collider Size : " + m_Size);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        InBlock = collision;
        float width = collision.GetComponent<SpriteRenderer>().bounds.size.x;
        float height = collision.GetComponent<SpriteRenderer>().bounds.size.y;
        //Debug.Log("Are : " + (width * height));

    }

    // Update is called once per frame
    void Update()
    {
        //if (InBlock)
        //Debug.Log("intersects" + m_Collider.bounds.Intersects(InBlock.bounds));
        var area = Physics2D.OverlapArea(pointStart, pointEnd);
        Debug.Log("area : " + area.bounds.size);
        Debug.Log("original : " + InBlock.bounds.size);
    }
}
