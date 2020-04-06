using UnityEngine;

/* 
 * Script volé ici : https://www.youtube.com/watch?v=LbDQHv9z-F0
 * (Voir description de la vidéo, il y a le lien)
 */

public class CameraFollow : MonoBehaviour
{
	public float CameraMoveSpeed = 120.0f;
	public GameObject CameraFollowObj;
	public float clampAngle = 90f;
	public float mouseX;
	public float mouseY;

	public bool disableCameraMouvement = false;

	public float mouseSensitivity = 125f;
	public GameObject PlayerObj;
	float xRotation = 0f;

	// Use this for initialization
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		CameraUpdater();
	}

	void LateUpdate()
	{
		if (!disableCameraMouvement)
		{
			CamControl();
		}
	}

	void CamControl()
	{
		mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);

		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		PlayerObj.transform.Rotate(Vector3.up * mouseX);
	}

	void CameraUpdater()
	{
		// set the target object to follow
		Transform target = CameraFollowObj.transform;

		//move towards the game object that is the target
		float step = CameraMoveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.position, step);
	}
}