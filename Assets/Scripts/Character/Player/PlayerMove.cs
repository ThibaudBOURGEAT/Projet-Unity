using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speedWalking = 15.0f;
    public float speedRunning = 25.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private Vector3 _moveDirection = Vector3.zero;
    private float _distToGround;
    private Animator anim;

    public EventObjectHeld eventObjectHeld;
    private int objectHeldID;

    void Start()
    {
        _distToGround = GetComponent<Collider>().bounds.extents.y;
        anim = gameObject.GetComponentInChildren<Animator>();

        eventObjectHeld.AddListener(ObjectHeldInfo);
    }
    void Update() 
    {
        if (!IsGrounded())
        {
            anim.SetInteger("AnimationJump", 0);
        }
            float directionvertical = Input.GetAxis ("Vertical");
        if (directionvertical > 0 && Input.GetAxis ("Horizontal")==0) 
        {
            anim.SetInteger("AnimationPar", 1);
        }
        else
        {
            anim.SetInteger("AnimationPar", 0);
       }

        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
        {
            anim.SetInteger("AnimationQ", 1);

        }
        else
        {
            anim.SetInteger("AnimationQ", 0);
        }

        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
        { 
            anim.SetInteger("AnimationD", 1);
        }
        else
        {
            
            anim.SetInteger("AnimationD", 0);

        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            anim.SetInteger("AnimationQ", 1);

        }
        else
        {
            anim.SetInteger("AnimationQ", 0);
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            anim.SetInteger("AnimationD", 1);
        }
        else
        {

            anim.SetInteger("AnimationD", 0);

        }

    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Move(speedRunning);
        }
        else
        {
            Move(speedWalking);
        }
        if (Input.GetButton("Jump"))
        {
            Jump();
        }
        this.transform.Translate(_moveDirection * Time.deltaTime);
    }

    bool IsGrounded()
    {
        bool isGrounded = Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hitInfo, _distToGround);

        if (objectHeldID != 0 && hitInfo.collider && hitInfo.collider.gameObject.GetInstanceID() == objectHeldID)
        {
            isGrounded = false;
        }

        return isGrounded;
    }

    public void Move(float speed)
    {
        if (IsGrounded())
        {
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            _moveDirection *= speed;
        }
        else
        {
            _moveDirection.y -= gravity * Time.deltaTime;
        }
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            _moveDirection.y = jumpSpeed;
            anim.SetInteger("AnimationJump", 1);
        }
    }
    public void ObjectHeldInfo(int objectHeldID)
    {
        this.objectHeldID = objectHeldID;
    }
}
