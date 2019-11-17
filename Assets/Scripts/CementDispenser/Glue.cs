using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Glue : MonoBehaviour
{
    public string controlScheme = "Dispenser";
    private string glueButton;
    private int particleCount = 0;
    public Transform cementSpawnLoc;
    public GameObject particleParent;
    private List<GameObject> glueList;

    public float glueMass;
    public float glueLinearDrag;
    public float glueAngularDrag;
    public float timeUntilDry = 4.0f;

    public Sprite cementSprite;
    public PhysicsMaterial2D cementPhysicsMaterial;

    public GameObject glueCharges;
    private BatteryBar glueChargesScript;

    private void Start()
    {
        if (controlScheme == "Dispenser")
            glueButton = "Magnet";
        else if (controlScheme == "UFO")
        {
            glueButton = "UFOGlue";
            glueChargesScript = glueCharges.GetComponent<BatteryBar>();
        }

        Debug.Log("controlscheme: " + glueButton);

        glueList = new List<GameObject>();
    }

    private void createGlue()
    {
        if (controlScheme == "Dispenser" || glueChargesScript.empty == false)
        {
            Debug.Log("Do we get here");
            GameObject glue = new GameObject("Cement Particle" + particleCount);
            glue.AddComponent<SpriteRenderer>();
            glue.GetComponent<SpriteRenderer>().sprite = cementSprite;
            glue.GetComponent<SpriteRenderer>().sortingOrder = -1;
            Rigidbody2D rb = glue.AddComponent<Rigidbody2D>();
            rb.sharedMaterial = cementPhysicsMaterial;
            rb.mass = glueMass;
            rb.drag = glueLinearDrag;
            rb.angularDrag = glueAngularDrag;
            PolygonCollider2D pc = glue.AddComponent<PolygonCollider2D>();
            pc.sharedMaterial = cementPhysicsMaterial;
            glue.transform.position = cementSpawnLoc.transform.position;
            Vector3 scale = glue.transform.localScale;
            scale.x = 0.3f;
            scale.y = 0.3f;
            glue.transform.localScale = scale;
            glue.transform.SetParent(particleParent.transform);
            glue.layer = LayerMask.NameToLayer("Glue");
            glue.tag = "Glue";
            glueList.Add(glue);
            StartCoroutine(waitForCementDry(glue));
            particleCount++;

            if (controlScheme == "UFO" && glueCharges != null)
                glueChargesScript.useCharge();
        }
    }

    IEnumerator waitForCementDry(GameObject cement)
    {
        yield return new WaitForSeconds(timeUntilDry);

        cement.AddComponent<Cement>();
        cement.GetComponent<Cement>().particleParent = particleParent;
    }

    private UnityAction UseToolListener;

    void Awake()
    {
        UseToolListener = new UnityAction(GlueEvent);
    }

    void OnEnable()
    {
        if (controlScheme == "UFO")
            EventManager.StartListening("UFOGlue", UseToolListener);
        else if (controlScheme == "Dispenser")
            EventManager.StartListening("Use", UseToolListener);
    }

    void OnDisable()
    {
        if (controlScheme == "UFO")
            EventManager.StopListening("UFOGlue", UseToolListener);
        else if (controlScheme == "Dispenser")
            EventManager.StopListening("Use", UseToolListener);
    }

    private void GlueEvent()
    {
        createGlue();
    }
}
