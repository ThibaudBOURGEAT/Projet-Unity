using UnityEngine;

/* 
 * Script volé ici : https://www.youtube.com/watch?v=LbDQHv9z-F0
 * (Voir description de la vidéo, il y a le lien)
 */

public class CameraCollision : MonoBehaviour
{
	public EventCameraMaxDistance eventCameraMaxDistance;

	public float minDistance = 1.0f;
	public float maxDistance = 20;
	public float smooth = 10.0f;
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

	// Update is called once per frame
	void Update()
	{
		var mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

		if (mouseScrollWheel > 0f && (maxDistance > minDistance)) // backward
		{
			maxDistance += -mouseScrollWheel * scrollSpeed;
		}
		else if (mouseScrollWheel < 0f)
		{
			maxDistance += -mouseScrollWheel * scrollSpeed;
		}

		Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);

		if (debugLinecast)
		{
			Debug.DrawLine(transform.parent.position, desiredCameraPos, Color.green);
		}

		if (Physics.Linecast(transform.parent.position, desiredCameraPos, out RaycastHit hit))
		{
			if (!hit.collider.gameObject.CompareTag("Player"))
			{
				distance = Mathf.Clamp(hit.distance * 0.87f, minDistance, maxDistance);
			}
		}
		else
		{
			distance = maxDistance;
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

		maxDistance = newDistance;
	}
}