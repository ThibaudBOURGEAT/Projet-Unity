using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player, target;
    public float mouseSpeed = 5.0f;
    private float mouseX = 0;
    private float mouseY = 0;

    public Transform obstruction;
    public float zommSpeed = 15f;
    private bool _isCollided = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        CamControl();
        ViewObstructed();
    }

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSpeed;
        mouseY = Mathf.Clamp(mouseY, -20, 60);
        target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        player.rotation = Quaternion.Euler(0, mouseX, 0);
    }

    void ViewObstructed()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                if (hit.distance >= 1f && Vector3.Distance(transform.position, target.position) <= 10f)
                {
                    transform.Translate(Vector3.back * zommSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            if(Vector3.Distance(transform.position, target.position) <= 10f && !_isCollided)
            {
                transform.Translate(Vector3.back * zommSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        _isCollided = true;
        if (other.gameObject.tag != "Player")
        {
            obstruction = other.transform;
            if (Vector3.Distance(transform.position, target.position) >= 1.5f)
            {
                transform.Translate(Vector3.forward * zommSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isCollided = false;
    }
}