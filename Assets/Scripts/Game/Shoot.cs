using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
	public Transform gun;
	Movement move;
	public GameObject shoot;
	public GameObject grenade;
	public GameObject melee;

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
	}

	void Update()
    {
		InputWeapon();
		AnimWeapon();

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
			/*case "3":
				selectedWeapon = Gun.Utility;
				break;

			//Special Weapon
			case "4":
				selectedWeapon = Gun.Special;
				break;*/

			//Melee
			case "5":
				selectedWeapon = Gun.Melee;
				break;
		}
	}

	void AnimWeapon()
	{
		if(selectedWeapon == Gun.Melee)
		{
			GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Idle_Sprites, SpriteAnim.State.Idle, true);
		}
		else
		{
			GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Aim, SpriteAnim.State.Aim, true);
		}
	}

	void Aim()
	{
		//get mouse direction from player
		Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

		//check if dir is right or left, 1 = right, -1 = left
		int flip = dir.x > 0 ? 1 : -1;

		//convert gunpos to screenpos && zero out z axis
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0;
		Vector3 objectPos = Camera.main.WorldToScreenPoint(gun.transform.position);
		objectPos.z = 0;

		//calc angle between mouse and gun
		float angle = Mathf.Atan2(mousePos.y - objectPos.y, mousePos.x - objectPos.x) * Mathf.Rad2Deg;

		//apply rotation
		gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

	void TryShoot()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			int flip = move.facingRight ? -1 : 1;

			//TODO: UPDATE TO MP
			if(!GetComponent<PlayerDataSP>())
			{
				//GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdShoot(flip, gun.transform.position);
			}
			else
			{
				GameObject b;

				//Test which weapon
				switch(selectedWeapon)
				{
					case Gun.Class:
						b = Instantiate(shoot);

						b.transform.position = gun.GetChild(0).transform.position;
						b.transform.rotation = gun.transform.rotation;
						b.GetComponent<Shooter>().Init(3, 0.1f, 5, 2, 50);
						break;

					case Gun.Grenade:
						b = Instantiate(grenade);

						b.transform.position = gun.transform.GetChild(0).transform.position;
						b.transform.rotation = gun.transform.rotation;
						b.GetComponent<Rigidbody>().velocity = b.transform.right * 5;
						break;

					case Gun.Utility:
						break;

					case Gun.Special:
						break;

					case Gun.Melee:
						b = Instantiate(melee);

						b.transform.position = gun.transform.GetChild(0).transform.position;
						b.transform.rotation = gun.transform.rotation;

						GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Melee_Sprites, SpriteAnim.State.Melee, false);
						break;
				}
			}

			FindObjectOfType<GameManager>().phase++;
			enabled = false;
		}
	}
}