using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public Transform groundCheck;
	public float groundCheckRadius = 0.2f;
	public LayerMask whatIsGround;

	Rigidbody2D rb;
	SpriteRenderer spr;

	bool grounded;
	bool jump;
	public bool facingRight = true;
	int horizontal;
	int lastDir;

	public float jumpForce = 400.0f;
	public float maxSpeed = 10.0f;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		spr = GetComponentInChildren<SpriteRenderer>();
    }

	void FixedUpdate()
	{
		CheckGrounded();
		GetInput();
		Move();
	}
	
	void CheckGrounded()
	{
		grounded = false;

		//get all collisions
		Collider2D[] collisions = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround);

		for(int x = 0; x < collisions.Length; x++)
		{
			//check that its not us
			if(collisions[x].gameObject != gameObject)
			{
				grounded = true;
			}
		}
	}

	void GetInput()
	{
		//check jump
		if(Input.GetKeyDown(KeyCode.W))
		{
			jump = true;
		}

		//get movement direction
		horizontal = (int)Input.GetAxisRaw("Horizontal");
	}

	void Move()
	{
		//move
		rb.velocity = new Vector2(horizontal * maxSpeed, rb.velocity.y);

		//check if we need to flip sprite
		Flip();

		//check jump
		if(grounded && jump)
		{
			grounded = false;

			rb.AddForce(Vector2.up * jumpForce);
		}

		jump = false;
	}

	void Flip()
	{
		//calc which dir we're facing based on movement speed
		if(horizontal != 0)
		{
			facingRight = horizontal < 0 ? false : true;
		}

		//set movement dir
		spr.flipX = facingRight;
	}
}