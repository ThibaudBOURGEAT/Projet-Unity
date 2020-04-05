using UnityEngine;

/* 
 * Script volé ici : https://www.youtube.com/watch?v=LbDQHv9z-F0
 * (Voir description de la vidéo, il y a le lien)
 */

public class CameraCollision : MonoBehaviour
{

	public float minDistance = 1.0f;
	public float maxDistance = 20;
	public float smooth = 10.0f;
	Vector3 dollyDir;
	public Vector3 dollyDirAdjusted;

	public float scrollSpeed = 5f;

	private float distance;

	// Use this for initialization
	void Awake()
	{
		dollyDir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}

	// Update is called once per frame
	void Update()
	{

		Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
		RaycastHit hit;

		if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
		{
			distance = Mathf.Clamp((hit.distance * 0.87f), minDistance, maxDistance);
		}
		else
		{
			distance = maxDistance;
		}

		transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);

		var mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

		if (mouseScrollWheel > 0f) // forward
		{
			maxDistance += mouseScrollWheel * scrollSpeed;
		}
		else if (mouseScrollWheel < 0f && (maxDistance > minDistance))
		{
			maxDistance += mouseScrollWheel * scrollSpeed;
		}

	}
}