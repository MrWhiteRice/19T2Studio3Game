using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public Transform groundCheck;
	public float groundCheckRadius = 0.2f;
	public LayerMask whatIsGround;

	Rigidbody rb;
	public GameObject playerSpr;

	public bool grounded;
	public bool jumping;
	bool jump;
	bool upJump;
	public bool facingRight = true;
	int horizontal;
	int lastDir;

	public float jumpForce = 400.0f;
	public float maxSpeed = 10.0f;

    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }

	void Update()
	{
		CheckGrounded();
		GetInput();
		Move();
	}
	
	void CheckGrounded()
	{
		grounded = false;

		//get all collisions
		Collider[] collisions = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, whatIsGround);

		for(int x = 0; x < collisions.Length; x++)
		{
			//check that its not us
			if(collisions[x].gameObject != gameObject)
			{
				grounded = true;
				upJump = false;
				jumping = false;
			}
		}
	}

	void GetInput()
	{
		int playerNum = (int)GetComponent<PlayerDataSP>().team + 1;

		//check jump
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown("joystick " + playerNum + " button 0") || Input.GetKeyDown("joystick " + playerNum + " button 1"))
		{
			upJump = true;
			jump = true;
		}
		else if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick " + playerNum + " button 3") || Input.GetKeyDown("joystick " + playerNum + " button 2"))
		{
			upJump = false;
			jump = true;
		}


		//get movement direction
		GetComponent<PlayerDataSP>().controllerMode = false;
		if(!GetComponent<PlayerDataSP>().controllerMode)
		{
			float push = Input.GetAxisRaw("P" + playerNum + "Horizontal");

			if(push != 0)
			{
				if(push < 0)
				{
					horizontal = -1;
				}
				else if(push > 0)
				{
					horizontal = 1;
				}
			}
			else
			{
				horizontal = 0;
			}
		}
		else
		{
			horizontal = (int)Input.GetAxisRaw("Horizontal");
		}
	}

	void Move()
	{
		if(grounded)
		{
			//move
			rb.velocity = new Vector2(horizontal * maxSpeed, rb.velocity.y);
		}

		//check should burn stamina
		if(rb.velocity.x != 0 && grounded)
		{
			GetComponent<PlayerDataSP>().stamina -= Time.deltaTime * 20;
		}

		//check if we need to flip sprite
		Flip();

		//check jump
		if(grounded && jump)
		{
			rb.velocity = Vector3.zero;

			GetComponent<PlayerDataSP>().stamina -= 20;
			grounded = false;

			int flipMod = facingRight ? -1 : 1;

			Vector3 up = upJump ?
				(Vector3.up * jumpForce) :
				(Vector3.up * jumpForce * 0.5f);

			Vector3 forward = upJump ?
				(Vector3.right * flipMod * (jumpForce) * 0.25f) :
				(Vector3.right * flipMod * (jumpForce));

			StartCoroutine(AddJumpForce(up, forward));
		}

		jump = false;
	}

	IEnumerator AddJumpForce(Vector3 up, Vector3 forward)
	{
		rb.AddForce(up);

		yield return new WaitForSeconds(0.05f);

		rb.AddForce(forward);

		yield return new WaitForEndOfFrame();

		jumping = true;
	}

	void Flip()
	{
		if(grounded)
		{
			//calc which dir we're facing based on movement speed
			if(horizontal != 0)
			{
				facingRight = horizontal < 0 ? true : false;
			}

			//set movement dir
			foreach(SpriteRenderer spr in playerSpr.GetComponentsInChildren<SpriteRenderer>())
			{
				spr.flipX = facingRight;
			}
		}
	}
}