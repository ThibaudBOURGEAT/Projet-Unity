using UnityEngine;

/* 
 * Script volé ici : https://www.youtube.com/watch?v=LbDQHv9z-F0
 * (Voir description de la vidéo, il y a le lien)
 */

public class CameraCollision : MonoBehaviour
{
	public EventCameraMaxDistance eventCameraMaxDistance;

	public float minScrollDistance = 1.0f;
	public float maxScrollDistance = 20f;

	public float smooth = 100f;
	public float scrollSpeed = 5f;

	[Tooltip("En gros, pour limiter la chance que la caméra clip un mur,\non applique un % décroissant à partir du point d'impact de la caméra")]
	public float offsetApplyToCameraDistance = 0.87f;
	public bool debugLinecast = false;

	private float distance;
	private float cameraDistance = 20f;
	private Vector3 dollyDir;

	/**
	 * Distance minimal stricte.
	 * Important si on veut que la caméra se rapproche le plus près du joueur
	 * Si la valeur est plus haute, la caméra va aller dans le mur !
	 */
	private float minDistance = 1f;

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

		if (mouseScrollWheel > 0f && (cameraDistance > minScrollDistance)) // backward
		{
			cameraDistance += -mouseScrollWheel * scrollSpeed;
		}
		else if (mouseScrollWheel < 0f && (cameraDistance < maxScrollDistance))
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
				distance = Mathf.Clamp(hit.distance * offsetApplyToCameraDistance, minDistance, maxScrollDistance);
			}
		}
		else
		{
			distance = cameraDistance * offsetApplyToCameraDistance;
		}

		transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);

	}

	public void ChangeCameraMaxDistance(float newDistance)
	{
		Debug.Log("ChangeCameraMaxDistance");
		if (newDistance == 0)
		{
			newDistance = minScrollDistance;
		}

		cameraDistance = newDistance;
	}
}