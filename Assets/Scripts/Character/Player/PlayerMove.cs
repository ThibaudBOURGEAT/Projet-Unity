﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speedWalking = 15.0f;
    public float speedRunning = 25.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private float yaw = 0;
    private Vector3 _moveDirection = Vector3.zero;
    private float _distToGround;

    void Start()
    {
        _distToGround = GetComponent<Collider>().bounds.extents.y;
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
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround);
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
        }
    }
}
