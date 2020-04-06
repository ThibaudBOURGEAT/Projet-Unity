using UnityEngine;
using UnityEngine.Events;

/* 
 * Script volé ici : https://www.youtube.com/watch?v=254geR2-w5c 
 * (Voir description de la vidéo, il y a le lien)
 */

[System.Serializable]
public class EventObjectHeld : UnityEvent<int> { }

public class PlayerDragDrop : MonoBehaviour
{
	public GameObject playerCamera;

	public float grabDistance = 10f;
	public float distanceFromPlayer = 10f;
	public float maxDistanceGrab = 15f;
	public float massDivider = 2;
	public float grabHeight = 2f;
	public float rotationSpeed = 500f;

	public bool activateRayDebug = false;

	private GameObject objectHeld;
	private bool isObjectHeld;
	private bool isObjectOnRotation;

	public EventObjectHeld eventObjectHeld;

	void Start()
	{
		isObjectHeld = false;
		isObjectOnRotation = false;
		objectHeld = null;
	}

	void FixedUpdate()
	{
		if (Input.GetButton("Fire1"))
		{
			if (!isObjectHeld)
			{
				TryPickObject();
			}
			else
			{
				HoldObject();
			}
		}
		else if (isObjectHeld)
		{
			DropObject();
		}
	}

	void Update()
	{
		// GetKeyUp obligatoire en Update sinon pas fired
		if (Input.GetKeyUp(KeyCode.E) && isObjectOnRotation)
		{
			isObjectOnRotation = false;
			playerCamera.gameObject.GetComponent<CameraFollow>().disableCameraMouvement = false;

			// S'il tient encore l'object cet abrutit, on remet la pression Physique du moteur
			//if (objectHeld)
			//{
			//objectHeld.GetComponent<Rigidbody>().isKinematic = false;
			//}
		}
	}

	private void TryPickObject()
	{
		var cameraDirection = playerCamera.transform.forward * grabDistance;
		var playerAim = new Ray(this.transform.position + this.transform.up * grabHeight, cameraDirection);

		if (activateRayDebug)
		{
			Debug.DrawRay(this.transform.position + this.transform.up * grabHeight, cameraDirection, Color.green);
		}

		if (Physics.Raycast(playerAim, out RaycastHit hit, grabDistance))
		{
			if (!hit.collider.gameObject.CompareTag("Player") && hit.collider.gameObject.GetComponent<Rigidbody>())
			{
				objectHeld = hit.collider.gameObject;
				isObjectHeld = true;
				objectHeld.GetComponent<Rigidbody>().useGravity = false;

				eventObjectHeld.Invoke(objectHeld.GetInstanceID());
			}
		}
	}

	private void HoldObject()
	{
		var cameraDirection = playerCamera.transform.forward * grabDistance;
		var playerAim = new Ray(this.transform.position + (this.transform.up * grabHeight), cameraDirection);

		Vector3 nextPos = this.transform.position + playerAim.direction * distanceFromPlayer;
		Vector3 currentPos = objectHeld.transform.position;

		Vector3 nextVelocity = (nextPos - currentPos) * (10 - (objectHeld.GetComponent<Rigidbody>().mass / massDivider));

		if (Input.GetKey(KeyCode.E))
		{
			isObjectOnRotation = true;
			playerCamera.gameObject.GetComponent<CameraFollow>().disableCameraMouvement = true;
			//objectHeld.GetComponent<Rigidbody>().isKinematic = true;
			objectHeld.GetComponent<Rigidbody>().useGravity = false;
			objectHeld.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);

		}

		objectHeld.GetComponent<Rigidbody>().velocity = nextVelocity;


		if (Vector3.Distance(objectHeld.transform.position, this.transform.position) > maxDistanceGrab)
		{
			DropObject();
		}
	}

	private void DropObject()
	{
		isObjectHeld = false;
		//objectHeld.GetComponent<Rigidbody>().isKinematic = false;
		objectHeld.GetComponent<Rigidbody>().useGravity = true;
		objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		objectHeld = null;
		eventObjectHeld.Invoke(0);
	}
}