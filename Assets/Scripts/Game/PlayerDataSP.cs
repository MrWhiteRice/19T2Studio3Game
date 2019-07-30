using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSP : MonoBehaviour
{
	public float health = 100;
	public float stamina = 100;
	public int ID = 0;
	public SpawnPoint.Team team;

	UnityEngine.UI.Text healthText;
	UnityEngine.UI.Slider staminaSlider;

	RigidbodyConstraints rbc;

	public CharacterParty character;

	public SpriteRenderer[] sprites;
	public Sprite[] idle;
	public Sprite[] moving;

	public LayerMask player;
	public LayerMask ground;

	void Start()
	{
		if(ID < 3)
		{
			character = FindObjectOfType<GameManager>().data.party[ID];
		}
		else
		{
			character = FindObjectOfType<GameManager>().data.party[ID - 3];
		}

		healthText = GetComponentInChildren<UnityEngine.UI.Text>();
		staminaSlider = GetComponentInChildren<UnityEngine.UI.Slider>();

		if(team == SpawnPoint.Team.Red)
		{
			healthText.color = Color.red;
		}
		else
		{
			healthText.color = Color.blue;
		}

		//rigidbody constraints
		rbc = GetComponent<Rigidbody>().constraints;

		//find correct character to load
		string nameSearch = "";
		foreach(Actor a in Resources.LoadAll<Actor>("RiceStuff/Actors"))
		{
			if(character.playerID == a.ID)
			{
				nameSearch = a.CharacterName;
				print(nameSearch + "," + a.CharacterName);
			}
		}

		//load animator
		SpriteAnim anim = GetComponent<SpriteAnim>();

		//Loading Animations
		LoadSprites(anim.Idle_Sprites, nameSearch, "Default"); //Idle
		LoadSprites(anim.Walk_Sprites, nameSearch, "Walk"); //Walk
		LoadSprites(anim.Melee_Sprites, nameSearch, "Unarmed_Attack"); //Melee

		//Start idle anim
		anim.PlayAnimation(GetComponent<SpriteAnim>().Idle_Sprites, SpriteAnim.State.Idle, true);
	}

	void LoadSprites(SpriteList list, string charName, string animName)
	{
		//Access the correct anim in the path and load the anim to the character
		print("Characters/" + charName + "/" + animName + "/" + charName + "_" + animName);
		list.Torso = Resources.LoadAll<Sprite>("Characters/" + charName + "/" + animName + "/" + charName + "_" + animName + "_Torso");
		list.Legs = Resources.LoadAll<Sprite>("Characters/" + charName + "/" + animName + "/" + charName + "_" + animName + "_Legs");
		list.Left = Resources.LoadAll<Sprite>("Characters/" + charName + "/" + animName + "/" + charName + "_" + animName + "_FArm");
		list.Right = Resources.LoadAll<Sprite>("Characters/" + charName + "/" + animName + "/" + charName + "_" + animName + "_BArm");
	}

	private void Update()
	{
		//TODO: Add controller support
		//print(Input.GetKey("joystick button 0"));
		//1 b

		//TODO: update death to actual death
		if(health <= 0)
		{
			foreach(SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
			{
				spr.flipY = true;
			}
		}

		if(IsTurn())
		{
			switch(FindObjectOfType<GameManager>().phase)
			{
				case GameManager.TurnPhase.Move:
					MovePhase();
					break;

				case GameManager.TurnPhase.Shoot:
					ShootPhase();
					break;
			}

			if((int)FindObjectOfType<GameManager>().phase > 1)
			{
				GetComponent<Movement>().enabled = false;
				GetComponent<Shoot>().enabled = false;
			}

			//check death | skip turn
			if(health <= 0)
			{
				FindObjectOfType<GameManager>().NextTurn();
			}

			healthText.text = "Me: " + health.ToString("f0");
			staminaSlider.value = stamina;
			//GetComponent<Rigidbody>().constraints = rbc;
		}
		else
		{
			if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.End || FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.Damage)
			{
				GetComponent<Rigidbody>().constraints = rbc;
			}
			else
			{
				//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			}
			healthText.text = "" + health.ToString("f0");
			staminaSlider.gameObject.SetActive(false);
			//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		}
	}

	void MovePhase()
	{
		if(stamina <= 0)
		{
			FindObjectOfType<GameManager>().phase++;
		}

		SpriteAnim.State str = Input.GetAxisRaw("Horizontal") != 0 ? SpriteAnim.State.Walk : SpriteAnim.State.Idle;
		SpriteList spr = Input.GetAxisRaw("Horizontal") != 0 ? GetComponent<SpriteAnim>().Walk_Sprites : GetComponent<SpriteAnim>().Idle_Sprites;
		GetComponent<SpriteAnim>().PlayAnimation(spr, str);

		GetComponent<Rigidbody>().constraints = rbc;

		staminaSlider.gameObject.SetActive(true);
		GetComponent<Movement>().enabled = true;
		GetComponent<Shoot>().enabled = false;
	}

	void ShootPhase()
	{
		staminaSlider.gameObject.SetActive(false);
		GetComponent<Movement>().enabled = false;
		//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		GetComponent<Shoot>().enabled = true;
	}

	public void SetTeam(SpawnPoint.Team set)
	{
		team = set;
	}

	public bool IsTurn()
	{
		if(FindObjectOfType<GameManager>().turn == ID)
		{
			return true;
		}

		return false;
	}
}