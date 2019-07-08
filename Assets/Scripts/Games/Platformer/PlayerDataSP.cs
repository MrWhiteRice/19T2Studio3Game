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

	void Start()
	{
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

		rbc = GetComponent<Rigidbody>().constraints;
	}

	private void Update()
	{
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
				print("asd");
				GetComponent<Movement>().enabled = false;
				GetComponent<Shoot>().enabled = false;
			}

			//check death | skip turn
			if(health <= 0)
			{
				FindObjectOfType<GameManager>().NextTurn();
			}

			healthText.text = "Me: " + health.ToString("f0");
			staminaSlider.enabled = true;
			staminaSlider.value = stamina;
			//GetComponent<Rigidbody>().constraints = rbc;
		}
		else
		{
			healthText.text = "" + health.ToString("f0");
			staminaSlider.enabled = false;
			//GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		}
	}

	void MovePhase()
	{
		if(stamina <= 0)
		{
			FindObjectOfType<GameManager>().phase++;
			stamina = 100;
		}
		GetComponent<Movement>().enabled = true;
		GetComponent<Shoot>().enabled = false;
	}

	void ShootPhase()
	{
		GetComponent<Movement>().enabled = false;
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