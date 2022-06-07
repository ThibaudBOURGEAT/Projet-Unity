using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Le sang de la veine
 * https://www.youtube.com/watch?v=cu39TdzirCE
 */
public class DisplayPickedItem : MonoBehaviour
{
    public EventItemPicked eventItemPicked;
    public float rotationSpeed = 500f;

    public Camera playerCamera;
    public Camera UICamera;

    public GameObject itemPickUpScene;

    private GameObject itemToLook;
    private bool wannaLookItem = false;

    public Text itemName;
    public Text itemDescription;

    /*
    private float distance;
    private Vector3 dollyDir;
    private float cameraDistance = 20f;
    private float minDistance = 1f;
    public float minScrollDistance = 1.0f;
    public float maxScrollDistance = 20f;
    public float scrollSpeed = 5f;
    public bool debugLinecast = false;
    public float offsetApplyToCameraDistance = 0.87f;
    public float smooth = 100f;
    */

    private float cameraDistance = 1.0f;

    public float minScrollDistance = 1.0f;
    public float maxScrollDistance = 20f;
    public float scrollSpeed = 5f;

    Vector3 pos;

    void Awake()
    {
        // Prevent the error "There are 2 audio listeners in the scene. Please ensure there is always exactly one audio listener in the scene."
        UICamera.gameObject.SetActive(false);

        pos = new Vector3();

    }

    // Start is called before the first frame update
    void Start()
    {
        eventItemPicked.AddListener(DiplayItemPicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (wannaLookItem)
        {
            itemToLook.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);
        }

        if (wannaLookItem && itemToLook)
        {
            float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

            if (mouseScrollWheel != 0)
            {
                pos.z -= mouseScrollWheel * scrollSpeed * Time.deltaTime * 100f;
                pos.x = UICamera.transform.position.x;
                pos.y = UICamera.transform.position.y;

                pos.z = Mathf.Clamp(pos.z, 1f, itemToLook.transform.position.z);

                UICamera.transform.position = pos;
            }
        }
    }

    public void DiplayItemPicked(GameObject item)
    {
        if (item)
        {
            string tempItemName = item.GetComponent<Interactable>().printName;
            string tempItemDescription = item.GetComponent<Interactable>().printDescription;

            itemToLook = Instantiate(item, UICamera.transform.position + UICamera.transform.forward * 10f, UICamera.transform.rotation);
            if (itemToLook)
            {
                itemToLook.transform.SetParent(itemPickUpScene.transform);

                // Layer Mask => UI
                foreach (Transform child in itemToLook.transform)
                {
                    child.gameObject.layer = 5;
                }

                // Apply Item Name & Description
                itemName.text = tempItemName;
                itemDescription.text = tempItemDescription;

                playerCamera.gameObject.SetActive(false);
                UICamera.gameObject.SetActive(true);

                wannaLookItem = true;
            }
        } else
        {
            // Lets quit

            if (itemToLook)
            {
                Destroy(itemToLook);
            }

            playerCamera.gameObject.SetActive(true);
            UICamera.gameObject.SetActive(false);

            // Reset item Name & Description
            itemName.text = "";
            itemDescription.text = "";

            wannaLookItem = false;
        }
    }
}
