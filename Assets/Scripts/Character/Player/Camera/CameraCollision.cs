using UnityEngine;

/* 
 * Script volé ici : https://www.youtube.com/watch?v=LbDQHv9z-F0
 * (Voir description de la vidéo, il y a le lien)
 */

public class CameraCollision : MonoBehaviour
{
	public EventCameraMaxDistance eventCameraMaxDistance;

	public float minDistance = 1.0f;
	public float maxDistance = 20f;
	public float cameraDistance = 20f;
	public float offsetApplyToDistance = 0.87f;

	public float smooth = 100f;
	Vector3 dollyDir;

	public float scrollSpeed = 5f;
	public bool debugLinecast = false;

	private float distance;

	// Use this for initialization
	void Awake()
	{
		dollyDir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;

		eventCameraMaxDistance.AddListener(ChangeCameraMaxDistance);
	}

	void LateUpdate()
	{
		var mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

		if (mouseScrollWheel > 0f && (cameraDistance > minDistance)) // backward
		{
			cameraDistance += -mouseScrollWheel * scrollSpeed;
		}
		else if (mouseScrollWheel < 0f && (cameraDistance < maxDistance))
		{
			cameraDistance += -mouseScrollWheel * scrollSpeed;
		}

		Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * cameraDistance);

		if (debugLinecast)
		{
			Debug.DrawLine(transform.parent.position, desiredCameraPos, Color.green);
		}

		if (Physics.Linecast(transform.parent.position, desiredCameraPos, out RaycastHit hit))
		{
			if (!hit.collider.gameObject.CompareTag("Player"))
			{
				distance = Mathf.Clamp(hit.distance * offsetApplyToDistance, minDistance, maxDistance);
			}
		}
		else
		{
			distance = cameraDistance * offsetApplyToDistance;
		}

		transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);

	}

	public void ChangeCameraMaxDistance(float newDistance)
	{
		Debug.Log("ChangeCameraMaxDistance");
		if (newDistance == 0)
		{
			newDistance = minDistance;
		}

		cameraDistance = newDistance;
	}
}