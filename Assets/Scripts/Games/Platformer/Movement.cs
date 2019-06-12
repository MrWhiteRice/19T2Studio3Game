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
	bool facingRight = true;
	int horizontal;
	int lastDir;

	Vector2 inputDir = Vector2.zero;
	Vector2 lastInputDir = Vector2.zero;

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

		Collider2D[] collisions = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround);

		for(int x = 0; x < collisions.Length; x++)
		{
			if(collisions[x].gameObject != gameObject)
			{
				grounded = true;
			}
		}
	}

	void GetInput()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			jump = true;
		}

		horizontal = 0;

		inputDir.x = Input.GetKey(KeyCode.A) ? 1 : 0;
		inputDir.y = Input.GetKey(KeyCode.D) ? 1 : 0;

		//detect new movement input
		if(inputDir != lastInputDir)
		{
			if(inputDir == Vector2.one)
			{
				//use last
				print("use last");
				return;
			}
			else if(inputDir != Vector2.zero)
			{
				//use new
				if(inputDir.x == 1)
				{
					horizontal = -1;
				}
				else if(inputDir.y == 1)
				{
					horizontal = 1;
				}

				print("new input");
			}
		}
		//continue movement
		else
		{
			if(inputDir.x == 1)
			{
				horizontal = -1;
			}
			else if(inputDir.y == 1)
			{
				horizontal = 1;
			}
		}

		//save input for next frame
		lastInputDir = inputDir;
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
		if(horizontal != 0)
		{
			facingRight = horizontal < 0 ? false : true;
		}

		spr.flipX = facingRight;
	}
}