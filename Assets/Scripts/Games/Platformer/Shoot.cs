using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
	float originalX;
	public Transform gun;
	Movement move;

	void Start()
	{
		move = GetComponent<Movement>();
		originalX = gun.transform.localPosition.x;
	}

	void Update()
    {
		if(!GetComponent<NetworkData>().IsMyTurn())
		{
			return;
		}

		Aim();
		TryShoot();
	}

	void Aim()
	{
		int flip = move.facingRight ? 1 : -1;

		Vector3 pos = gun.transform.localPosition;
		pos.x = originalX * flip;
		gun.transform.localPosition = pos;
	}

	void TryShoot()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			//GameObject b = Instantiate(bullet, gun.transform.position, Quaternion.identity);
			int flip = move.facingRight ? 1 : -1;
			//b.GetComponent<Rigidbody2D>().velocity = b.transform.right * flip * 5;
			GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdShoot(flip, gun.transform.position);
		}
	}
}