using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float Force = 5;
    public int MaxHp = 3;
    public Sprite FineSprite;
    public Sprite Damagedprite;
    bool dontDamage;

    [HideInInspector]
    public bool beenPickedUp;

    private int hp;
    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        hp = MaxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = FineSprite;
        dontDamage = true;
        StartCoroutine(makeInvincibleForXSeconds(2.0f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > Force && 
            collision.collider.tag != "Magnet")
            Damage();
    }

    public void Damage()
    {
        if (dontDamage)
            return;

        Debug.Log("Block Damage, " + this.name + ", HP: " + this.hp);
        hp--;
        if (hp == 1)
            spriteRenderer.sprite = Damagedprite;
        if (hp == 0)
            Destroy(this.gameObject);
    }

    IEnumerator makeInvincibleForXSeconds(float x)
    {
        yield return new WaitForSeconds(x);
        dontDamage = false;
    }
}
