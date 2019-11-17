using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed = 4;
    public float smashSpeed = 1;

    private Transform monsterTransform;
    private float walkDirection = -1;
    private Animator monsterAnimator;
    private bool isAngry;
    private bool isCounterObject;
    private float timer;

    private BlockController blockController;
    private BunnyController bunnyController;

    // Start is called before the first frame update
    void Start()
    {
        isAngry = false;
        monsterTransform = GetComponent<Transform>();
        monsterAnimator = GetComponent<Animator>();
        timer = 1 / smashSpeed;

        // Increasing disaster level
        smashSpeed += smashSpeed *
            GameMaster.DisasterIncreaseRatio * GameMaster.CurrentDisasterLevel;
        moveSpeed += moveSpeed *
            GameMaster.DisasterIncreaseRatio * GameMaster.CurrentDisasterLevel;
    }

    // Update is called once per frame
    void Update()
    {
        monsterAnimator.SetBool("isAngry", isAngry);
        if (isCounterObject == false)
            monsterTransform.Translate(
                new Vector3(walkDirection * moveSpeed * Time.fixedDeltaTime, 0, 0));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Bunny")
        {
            isCounterObject = true;
            timer += Time.deltaTime;
            if (timer > 1 / smashSpeed)
            {
                isAngry = true;
                timer = 0;
                // Damage victims
                if (monsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("MonsterSmash"))
                {
                    monsterAnimator.speed = smashSpeed;
                    if (collision.gameObject.tag == "Block")
                    {
                        Debug.Log("Victim: " + collision.gameObject.name);
                        blockController = collision.gameObject.GetComponent<BlockController>();
                        blockController.Damage();
                    }
                    else
                    {
                        Debug.Log("Victim: " + collision.gameObject.name);
                        bunnyController = collision.gameObject.GetComponent<BunnyController>();
                        bunnyController.Damage();
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Bunny")
        {
            isCounterObject = false;
            isAngry = false;
        }
    }
}
