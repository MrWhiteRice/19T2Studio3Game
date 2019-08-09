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

	public LayerMask player;
	public LayerMask ground;

	public bool hurt;

	void Start()
	{
		character = FindObjectOfType<GameManager>().data.party[ID%3];

		foreach(Weapon w in Resources.LoadAll<Weapon>("RiceStuff/Weapons"))
		{
			if(w.ID == character.classID)
			{
				GetComponent<SpriteAnim>().weapon = w.Icon;
			}
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
				//REMOVE print(nameSearch + "," + a.CharacterName);
			}
		}

		//load animator
		SpriteAnim anim = GetComponent<SpriteAnim>();

		//Loading Animations
		LoadSprites(anim.Idle_Sprites, nameSearch, "Default", 1); //Idle
		LoadSprites(anim.Walk_Sprites, nameSearch, "Walk", 8); //Walk

		LoadSprites(anim.Melee_Sprites, nameSearch, "Unarmed_Attack", 5); //Melee
		LoadSprites(anim.Melee_Idle_Sprites, nameSearch, "Melee_Stance_1", 1); //Melee Idle

		LoadSprites(anim.Grenade_Sprites, nameSearch, "Grenade", 5); //Grenade

		LoadSprites(anim.Hurt_Sprites, nameSearch, "Hurt", 3); //Hurt
		LoadSprites(anim.Jump_Sprites, nameSearch, "Jump", 4); //Jump

		LoadSprites(anim.Aim, nameSearch, "Rifle_Aim_1", 1); //Gun Aim

		//Start idle anim
		anim.PlayAnimation(GetComponent<SpriteAnim>().Idle_Sprites, SpriteAnim.State.Idle, true);
	}

	void LoadSprites(SpriteList list, string charName, string animName, int frames)
	{
		//Access the correct anim in the path and load the anim to the character
		//REMOVE: print("Characters/" + charName + "/" + animName + "/" + charName + "_" + animName);
		list.Torso = PrepareSprites(Resources.Load<Texture2D>("Characters/" + charName + "/" + animName + "/" + charName + "_" + animName + "_Torso"), frames);
		list.Legs = PrepareSprites(Resources.Load<Texture2D>("Characters/" + charName + "/" + animName + "/" + charName + "_" + animName + "_Legs"), frames);
		list.Left = PrepareSprites(Resources.Load<Texture2D>("Characters/" + charName + "/" + animName + "/" + charName + "_" + animName + "_FArm"), frames);
		list.Right = PrepareSprites(Resources.Load<Texture2D>("Characters/" + charName + "/" + animName + "/" + charName + "_" + animName + "_BArm"), frames);
	}

	Sprite[] PrepareSprites(Texture2D spr, int frames)
	{
		//make temp array that we can return
		Sprite[] filtered = new Sprite[frames];

		//set filter mode to point so we dont get blurring
		spr.filterMode = FilterMode.Point;

		//slice sprite into frames
		for(int x = 0; x < frames; x++)
		{
			//create a sprite from the texture using the frame length provided
			filtered[x] = Sprite.Create(spr, new Rect(x * (spr.width / frames), 0, (spr.width / frames), spr.height), Vector2.one*0.5f);
		}

		return filtered;
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

			if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.Move)
			{
				Destroy(gameObject);
			}
		}

		//check if its my turn
		if(IsTurn())
		{
			//Check what phase we're in
			switch(FindObjectOfType<GameManager>().phase)
			{
				//movement phase
				case GameManager.TurnPhase.Move:
					MovePhase();
					break;

				//attack phase
				case GameManager.TurnPhase.Shoot:
					ShootPhase();
					break;
			}

			//disable if not in a "controllable" phase
			if((int)FindObjectOfType<GameManager>().phase > 1)
			{
				GetComponent<Movement>().enabled = false;
				GetComponent<Shoot>().enabled = false;
			}

			//check death || skip turn
			if(health <= 0)
			{
				FindObjectOfType<GameManager>().NextTurn();
			}

			healthText.text = "Me: " + health.ToString("f0");
			staminaSlider.value = stamina;
			//GetComponent<Rigidbody>().constraints = rbc;

			if((int)FindObjectOfType<GameManager>().phase > 2)
			{
				if(hurt)
				{
					GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Hurt_Sprites, SpriteAnim.State.Hurt, false, false, true);
				}
				else
				{
					GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Idle_Sprites, SpriteAnim.State.Idle);
				}
			}
		}
		else
		{
			GetComponent<SpriteAnim>().weaponSlot.sprite = null;

			if(hurt)
			{
				GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Hurt_Sprites, SpriteAnim.State.Hurt, false, false, true);
			}
			else
			{
				GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Idle_Sprites, SpriteAnim.State.Idle);
			}

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
		//force shoot phase
		if(stamina <= 0)
		{
			FindObjectOfType<GameManager>().phase++;
		}

		if(GetComponent<Movement>().grounded)
		{
			SpriteAnim.State str = Input.GetAxisRaw("Horizontal") != 0 ? SpriteAnim.State.Walk : SpriteAnim.State.Idle;
			SpriteList spr = Input.GetAxisRaw("Horizontal") != 0 ? GetComponent<SpriteAnim>().Walk_Sprites : GetComponent<SpriteAnim>().Idle_Sprites;
			GetComponent<SpriteAnim>().PlayAnimation(spr, str);
		}
		else
		{
			if(GetComponent<Movement>().jumping)
			{
				GetComponent<SpriteAnim>().PlayAnimation(GetComponent<SpriteAnim>().Jump_Sprites, SpriteAnim.State.Jump, false, false, true);
			}
		}

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