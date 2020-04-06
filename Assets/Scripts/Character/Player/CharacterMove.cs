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
}
