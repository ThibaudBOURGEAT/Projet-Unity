using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    public bool activateRayDebug = false;

    public EventCameraMaxDistance eventCameraMaxDistance;

    public GameObject playerCamera;
    public float pickUpDistance = 10f;
    Vector3 dollyDir;

    // Use this for initialization
    void Awake()
    {
        dollyDir = transform.localPosition.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Use"))
        {
            TryLookObject();
        }
    }

    void TryLookObject()
    {
        var cameraDirection = playerCamera.transform.forward * pickUpDistance;
        var playerAim = new Ray(this.transform.position + this.transform.up, cameraDirection);

        if (activateRayDebug)
        {
            Debug.DrawRay(playerCamera.transform.position + playerCamera.transform.up, cameraDirection, Color.blue);
        }

        if (Physics.Raycast(playerAim, out RaycastHit hit, pickUpDistance))
        {
            if (hit.collider.gameObject.GetComponent<Interactable>())
            {
                var objectHeld = hit.collider.gameObject;

                Debug.Log(objectHeld);

                eventCameraMaxDistance.Invoke(0);
            }
        }
    }
}
