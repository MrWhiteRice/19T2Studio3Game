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

	int playerNum;
	bool attack;
	Vector2 conDir;

	public enum Gun
	{
		Class = 1,
		Grenade = 2,
		Utility = 4,
		Special = 5,
		Melee = 3,
	}

	public Gun selectedWeapon;
	public GameObject character;

	void Start()
	{
		move = GetComponent<Movement>();
		playerNum = (int)GetComponent<PlayerDataSP>().team + 1;
	}

	private void OnEnable()
	{
		gun.gameObject.SetActive(true);
	}

	private void OnDisable()
	{
		gun.gameObject.SetActive(false);
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
		int selWep = (int)selectedWeapon;
		if(Input.GetKeyDown("joystick " + playerNum + " button 4"))//LB
		{
			print("left");
			selWep--;
		}
		else if(Input.GetKeyDown("joystick " + playerNum + " button 5"))//RB
		{
			print("right");
			selWep++;
		}

		//switch(Input.inputString)
		switch(selWep.ToString())
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
			case "3":
				selectedWeapon = Gun.Melee;
				break;
		}
	}

	void AnimWeapon()
	{
		if(attack)
		{
			return;
		}

		GetComponent<SpriteAnim>().weaponSlot.sprite = null;

		if(selectedWeapon == Gun.Melee)
		{
			GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Idle_Sprites, SpriteAnim.State.Idle, true);
		}
		else if(selectedWeapon == Gun.Class)
		{
			GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Aim, SpriteAnim.State.Aim, true);
			GetComponent<SpriteAnim>().weaponSlot.sprite = GetComponent<SpriteAnim>().weapon;
		}
		else if(selectedWeapon == Gun.Grenade)
		{
			GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Grenade_Sprites, SpriteAnim.State.Grenade, false, true);
		}
	}

	void Aim()
	{
		Vector2 testDir = new Vector2(Input.GetAxis("P" + playerNum + "X"), Input.GetAxis("P" + playerNum + "Y"));
		if(testDir != Vector2.zero)
		{
			conDir = testDir;
		}

		//get mouse direction from player
		Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

		//check if dir is right or left, 1 = right, -1 = left
		bool flip = dir.x < 0 ? true : false;

		//move gun sprite depending on face'd direction
		Vector3 tra = GetComponent<SpriteAnim>().weaponSlot.transform.localPosition;
		tra.x = dir.x > 0 ? -0.075f : 0.075f;
		GetComponent<SpriteAnim>().weaponSlot.transform.localPosition = tra;

		//set movement dir
		foreach(SpriteRenderer spr in character.GetComponentsInChildren<SpriteRenderer>())
		{
			spr.flipX = flip;
		}

		//convert gunpos to screenpos && zero out z axis
		//Vector3 mousePos = Input.mousePosition;
		//mousePos.z = 0;
		//Vector3 objectPos = Camera.main.WorldToScreenPoint(gun.transform.position);
		//objectPos.z = 0;

		Vector3 mousePos = gun.transform.position + (Vector3)conDir;
		mousePos.z = 0;
		Vector3 objectPos = gun.transform.position;
		objectPos.z = 0;

		//calc angle between mouse and gun
		float angle = Mathf.Atan2(mousePos.y - objectPos.y, mousePos.x - objectPos.x) * Mathf.Rad2Deg;

		//apply rotation
		gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

	void TryShoot()
	{
		//if(Input.GetKeyDown(KeyCode.Space))
		if(Input.GetMouseButtonDown(0) || Input.GetAxis("P" + playerNum + "T") > 0)
		{
			attack = true;

			//TODO: UPDATE TO MP
			if(!GetComponent<PlayerDataSP>())
			{
				//GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdShoot(flip, gun.transform.position);
			}
			else
			{
				GameObject b;
				GetComponent<SpriteAnim>().state = SpriteAnim.State.Null;

				//Test which weapon
				switch(selectedWeapon)
				{
					case Gun.Class:
						b = Instantiate(shoot);

						b.transform.position = gun.GetChild(0).transform.position;
						b.transform.rotation = gun.transform.rotation;
						b.GetComponent<Shooter>().Init(3, 0.1f, 10, 2, 50);
						break;

					case Gun.Grenade:
						ThrowGrenade();
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

	void ThrowGrenade()
	{
		GetComponent<SpriteAnim>().state = SpriteAnim.State.Null;
		GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Grenade_Sprites, SpriteAnim.State.Grenade, false);

		GameObject b = Instantiate(grenade);

		b.transform.position = gun.transform.GetChild(0).transform.position;
		b.transform.rotation = gun.transform.rotation;
		b.GetComponent<Rigidbody>().velocity = b.transform.right * 5;
	}
}