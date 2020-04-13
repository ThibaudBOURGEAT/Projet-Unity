using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 30f;
    public float sprintSpeed = 60f;

    public float gravity = -100f;
    public float jumpHeight = 6f;

    public float pushPower = 2.0f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public float animationSmoothing;
    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        anim.SetFloat("Vertical_f", y);
        anim.SetFloat("Horizontal_f", x);

        Vector3 fowardMovement = transform.forward * y;
        Vector3 rightMovement = transform.right * x;

        var speedRate = speed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            speedRate = sprintSpeed;
        }
        characterController.Move(Vector3.ClampMagnitude(fowardMovement + rightMovement, 1f) * Time.deltaTime * speedRate);

        if (isGrounded && anim.GetBool("Jump"))
        {
            anim.SetBool("Jump", false);
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetBool("Jump", true);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }

    // ça vient de la doc : https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
}
