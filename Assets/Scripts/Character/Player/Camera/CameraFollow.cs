using UnityEngine;

/* 
 * Script volé ici : https://www.youtube.com/watch?v=LbDQHv9z-F0
 * (Voir description de la vidéo, il y a le lien)
 */

public class CameraFollow : MonoBehaviour
{

	public float CameraMoveSpeed = 120.0f;
	public GameObject CameraFollowObj;
	public float clampAngle = 80.0f;
	public float inputSensitivity = 150.0f;
	public GameObject CameraObj;
	public GameObject PlayerObj;
	public float camDistanceXToPlayer;
	public float camDistanceYToPlayer;
	public float camDistanceZToPlayer;
	public float mouseX;
	public float mouseY;
	public float finalInputX;
	public float finalInputZ;
	public float smoothX;
	public float smoothY;

	public bool disableCameraMouvement = false;

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
		mouseX += Input.GetAxis("Mouse X") * 5f;
		mouseY -= Input.GetAxis("Mouse Y") * 5f;
		mouseY = Mathf.Clamp(mouseY, -60, 60);
		transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
		PlayerObj.transform.rotation = Quaternion.Euler(0, mouseX, 0);
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