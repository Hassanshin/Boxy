using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragHandler : MonoBehaviour
{
    //Initialize Variables
    GameObject getTarget;
    bool isMouseDragging;
    Vector3 offsetValue;
    Vector3 positionOfScreen;
    Vector3 firstPos;

    public LayerMask mask;
    public LayerMask planeMask;

    private PlayerUI PUi;
    private PlayerManager PManager;

    private void Start()
    {
        PUi = GetComponent<PlayerUI>();
        PManager = GetComponent<PlayerManager>();
    }

    void Update()
    {

        //Mouse Button Press Down
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            getTarget = ReturnClickedObject(out hitInfo);
            if (getTarget != null)
            {
                isMouseDragging = true;
                //Converting world position to screen position.
                positionOfScreen = Camera.main.WorldToScreenPoint(getTarget.transform.position);
                offsetValue = getTarget.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
            }
        }
        else
        //Mouse Button Up
        if (Input.GetMouseButtonUp(0))
        {
            if (getTarget == null)
                return;

            RaycastHit hitInfo;
            ReturnObjectSwap(out hitInfo);
            getTarget.layer = 9;

            // Done Moving Monsters

            PUi.UpdatePlayerUI();
            PManager.CountScore();

            isMouseDragging = false;
        }
        else
        //Is mouse Moving
        if (isMouseDragging)
        {
            //tracking mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z);

            RaycastHit hitInfoMouse;
            //converting screen position to world position with offset changes.
            Vector3 currentPosition = ReturnPosRaycast(out hitInfoMouse);

            //It will update target gameobject's current postion.
            getTarget.transform.position = currentPosition;
        }


    }

    void swapPosition(Transform a, Transform b)
    {
        Transform monsterA = a.GetChild(0);
        Transform monsterB = b.GetChild(0);

        monsterA.SetParent(b);
        monsterB.SetParent(a);

        monsterA.SetAsFirstSibling();
        monsterB.SetAsFirstSibling();

        monsterA.localPosition = Vector3.zero;
        monsterB.localPosition = Vector3.zero;

    }

    Transform ReturnObjectSwap(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit, Mathf.Infinity, mask))
        {
            swapPosition(hit.transform, getTarget.transform);
        }

        getTarget.transform.localPosition = firstPos;

        return getTarget.transform.parent;
    }

    Vector3 ReturnPosRaycast(out RaycastHit hit)
    {
        Vector3 point;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit, Mathf.Infinity, planeMask))
        {
            point = hit.point;
            
            return point;
            
        } else
        {
            return Vector3.zero;
        }

        
    }

    //Method to Return Clicked Object
    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit, Mathf.Infinity, mask))
        {
            firstPos = hit.collider.transform.position;
            target = hit.collider.gameObject;

            target.layer = 0;
        }

        return target;
    }
}
