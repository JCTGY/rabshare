using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//handles the vertical movement of the crane's arm
public class ArmMoveEvent : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float yMax = 2.65f;
    public float yMin = -0.55f;
    private UnityAction armMoveUpListsener;
    private UnityAction armMoveDownListsener;
    private Transform armTransform;
    private Vector3 clampedPosition;
    private float moveStep;

    void Awake ()
    {
        armMoveUpListsener = new UnityAction (armMoveUp);
        armMoveDownListsener = new UnityAction (armMoveDown);
        armTransform = this.GetComponent<Transform>();
        moveStep = moveSpeed * Time.deltaTime;
    }

    void OnEnable()
    {
        EventManager.StartListening ("ArmMoveUp", armMoveUpListsener);
        EventManager.StartListening ("ArmMoveDown", armMoveDownListsener);
    }

    void OnDisable()
    {
        EventManager.StopListening ("ArmMoveUp", armMoveUpListsener);
        EventManager.StopListening ("ArmMoveDown", armMoveDownListsener);
    }

    void armMoveUp()
    {
        if (armTransform.localPosition.y + moveStep < yMax)
            armTransform.Translate(new Vector3(0, moveStep, 0));
    }

    void armMoveDown()
    {
        if (armTransform.localPosition.y - moveStep > yMin)
            armTransform.Translate(new Vector3(0, -moveStep, 0));
    }
}
