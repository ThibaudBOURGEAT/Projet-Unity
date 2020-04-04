﻿using UnityEngine;

/* 
 * Script volé ici : https://www.youtube.com/watch?v=254geR2-w5c 
 * (Voir description de la vidéo, il y a le lien)
 */

public class PlayerDragDrop : MonoBehaviour
{
	public GameObject playerCamera;

	public float grabDistance = 10f;
	public float distanceFromPlayer = 10f;
	public float maxDistanceGrab = 15f;
	public float massDivider = 2;
	public float grabHeight = 2f;

	public bool activateRayDebug = false;

	private GameObject objectHeld;
	private bool isObjectHeld;

	void Start()
	{
		isObjectHeld = false;
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
			}
		}
	}

	private void HoldObject()
	{
		var cameraDirection = playerCamera.transform.forward * grabDistance;
		var playerAim = new Ray(this.transform.position + this.transform.up * grabHeight, cameraDirection);

		Vector3 nextPos = this.transform.position + playerAim.direction * distanceFromPlayer;
		Vector3 currPos = objectHeld.transform.position;

		objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * (10 - (objectHeld.GetComponent<Rigidbody>().mass / massDivider));

		if (Vector3.Distance(objectHeld.transform.position, this.transform.position) > maxDistanceGrab)
		{
			DropObject();
		}
	}

	private void DropObject()
	{
		isObjectHeld = false;
		objectHeld.GetComponent<Rigidbody>().useGravity = true;
		objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		objectHeld = null;
	}
}
