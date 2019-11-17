using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BunnyController : MonoBehaviour
{
    public GameObject explosionPrefab;
    public GameObject rewardTextPrefab;
    public float Force = 5; //the minimum relative velocity that destroys the bunny
    public float moveSpeed = 6;
    public float rewardTextOffsetX = 0;
    public float rewardTextOffsetY = 2;

    private float walkDirection = 1;
    private Animator BunnyAnimator;
    private Transform BunnyTransform;
    private bool enableCollision;
    private TextMesh rewardTextMesh;
    private Vector3 rewardTextOffset;

    [HideInInspector]
    public bool rotateBunny;
    [HideInInspector]
    public bool bunnyIsGrabbed;
    [HideInInspector]
    public bool bunnyIsDropped;

    bool checkForGroundCollision;
    bool dontDamage;

    [HideInInspector]
    public bool bunnyInScoringArea;

    // Start is called before the first frame update
    void Start()
    {
        BunnyAnimator = GetComponent<Animator>();
        BunnyAnimator.enabled = true;
        BunnyTransform = GetComponent<Transform>();
        rewardTextMesh = rewardTextPrefab.GetComponent<TextMesh>();
        rewardTextOffset = new Vector3(rewardTextOffsetX, rewardTextOffsetY, 0);

        dontDamage = true;
        makeInvincibleForXSeconds(2.0f);
    }

    private void Update()
    {
        if (rotateBunny == true)
        {
            if (this.transform.rotation != Quaternion.identity)
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.identity, Time.deltaTime * 30.0f);
            else
                rotateBunny = false;
        }

        if (bunnyIsGrabbed == true)
        {
            BunnyAnimator.SetBool("moving", true);
            BunnyAnimator.SetBool("isTurn", false);
            bunnyIsGrabbed = false;
        }

        if (bunnyIsDropped == true)
        {
            rotateBunny = true;
            BunnyAnimator.SetBool("moving", false);
            bunnyIsDropped = false;
            checkForGroundCollision = true;
        }

    }

    void FixedUpdate()
    {
        enableCollision = true;
        if (SceneManager.GetActiveScene().name.Contains("Score") && bunnyIsGrabbed == false)
        {
            BunnyAnimator.SetBool("moving", false);
            //BunnyAnimator.enabled = false;
        }
        if (SceneManager.GetActiveScene().name.Contains("Disaster"))
        {
            BunnyAnimator.SetBool("moving", true);
            BunnyTransform.Translate(
                new Vector3(walkDirection * moveSpeed * Time.fixedDeltaTime, 0, 0));
            BunnyAnimator.SetBool("isTurn", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Bunny collided with " + other.collider.name);

        if (enableCollision == true && other.gameObject.tag == "Block")
        {
            if (other.relativeVelocity.magnitude > Force)
            {
                Damage();
            }
            if (SceneManager.GetActiveScene().name.Contains("Disaster"))
            {
                BunnyAnimator.SetBool("isTurn", true);
                walkDirection *= -1;
            }
            enableCollision = false;
        }

        if (checkForGroundCollision && (other.collider.tag == "Ground" || other.collider.tag == "Base" || other.collider.tag == "Block"))
        {
            BunnyAnimator.SetTrigger("hitGround");
            checkForGroundCollision = false;
        }
    }

    public void RewardTextSpawn()
    {
        rewardTextMesh.text = "+" + GameMaster.EachBunnyReward;
        Instantiate(rewardTextPrefab, this.transform.position + rewardTextOffset, Quaternion.identity);
    }

    public void Damage()
    {
        if (dontDamage)
            return;

        Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }

    IEnumerator makeInvincibleForXSeconds(float x)
    {
        yield return new WaitForSeconds(x);
        dontDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Base")
            bunnyInScoringArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Base")
            bunnyInScoringArea = false;
    }
}
