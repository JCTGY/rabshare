using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardTextController : MonoBehaviour
{
    public Color textColor;
    public float duration = 1;
    public float floatUpSpeed = 0.01f;
    public float transparentSpeed = 2;

    private TextMesh textMesh;
    private Transform textTransform;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        textMesh.color = textColor;
        textTransform = GetComponent<Transform>();
        Destroy(this.gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        textTransform.Translate(0, floatUpSpeed, 0);
        textColor.a -= transparentSpeed * Time.deltaTime;
        textMesh.color = textColor;
    }
}
