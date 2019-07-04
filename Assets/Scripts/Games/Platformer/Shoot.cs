using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
	float originalX;
	public Transform gun;
	Movement move;
	public GameObject bullet;
	public GameObject grenade;

	public enum Gun
	{
		Class,
		Grenade,
		Utility,
		Special,
		Melee
	}

	public Gun selectedWeapon;

	void Start()
	{
		move = GetComponent<Movement>();
		originalX = gun.transform.localPosition.x;
	}

	void Update()
    {
		InputWeapon();

		Aim();
		TryShoot();
	}

	void InputWeapon()
	{
		switch(Input.inputString)
		{
			//Class Gun
			case "1":
				selectedWeapon = Gun.Class;
				break;

			//Grenade
			case "2":
				selectedWeapon = Gun.Grenade;
				break;

			//Utility Item
			case "3":
				selectedWeapon = Gun.Utility;
				break;

			//Special Weapon
			case "4":
				selectedWeapon = Gun.Special;
				break;

			//Melee
			case "5":
				selectedWeapon = Gun.Melee;
				break;
		}
	}

	void Aim()
	{
		int flip = move.facingRight ? 1 : -1;

		Vector3 pos = gun.transform.localPosition;
		pos.x = originalX * -flip;
		gun.transform.localPosition = pos;
	}

	void TryShoot()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			int flip = move.facingRight ? -1 : 1;

			//TODO: UPDATE TO MP
			if(!GetComponent<PlayerDataSP>())
			{
				GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdShoot(flip, gun.transform.position);
			}
			else
			{
				GameObject b;

				//Test which weapon
				switch(selectedWeapon)
				{
					case Gun.Class:
						b = Instantiate(bullet);

						b.transform.position = gun.transform.position;
						b.GetComponent<Bullet>().dir = flip;
						break;

					case Gun.Grenade:
						b = Instantiate(grenade);

						b.transform.position = gun.transform.position;
						//b.GetComponent<Grenade>().dir = flip;
						b.GetComponent<Rigidbody>().velocity = b.transform.right * 5;
						break;

					case Gun.Utility:
						break;

					case Gun.Special:
						break;

					case Gun.Melee:
						break;
				}
			}
		}
	}
}