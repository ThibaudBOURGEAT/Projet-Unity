using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    public bool activateRayDebug = false;

    public EventCameraMaxDistance eventCameraMaxDistance;
    public EventItemPicked eventItemPicked;

    public GameObject playerCamera;
    public float pickUpDistance = 10f;
    public float rotationSpeed = 500f;

    public GameObject objectToUse;

    private bool eventInvoked = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Use"))
        {
            if (!objectToUse)
            {
                TryLookObject();
            } else
            {
                StopLookObject();
            }
        }

        if (objectToUse && !eventInvoked)
        {
            StartLookObject();
        }
    }

    void TryLookObject()
    {
        var cameraDirection = playerCamera.transform.forward * pickUpDistance;
        var playerAim = new Ray(playerCamera.transform.position + playerCamera.transform.up, cameraDirection);

        if (activateRayDebug)
        {
            Debug.DrawRay(playerCamera.transform.position + playerCamera.transform.up, cameraDirection, Color.blue);
        }

        if (Physics.Raycast(playerAim, out RaycastHit hit, pickUpDistance))
        {
            if (hit.collider.gameObject.GetComponent<Interactable>())
            {
                objectToUse = hit.collider.gameObject;
            }
        }
    }

    void StartLookObject()
    {
        eventItemPicked.Invoke(objectToUse);
        eventInvoked = true;
    }

    void StopLookObject()
    {
        objectToUse = null;
        eventItemPicked.Invoke(objectToUse);
        eventInvoked = false;
    }
}
