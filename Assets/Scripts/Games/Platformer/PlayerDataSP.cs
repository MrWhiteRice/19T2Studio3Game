using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSP : MonoBehaviour
{
	public int health = 100;
	public int ID = 0;
	public SpawnPoint.Team team;
	UnityEngine.UI.Text healthText;
	RigidbodyConstraints rbc;

	void Start()
	{
		healthText = GetComponentInChildren<UnityEngine.UI.Text>();

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
		if(IsTurn())
		{
			healthText.text = "Me: 100";
			GetComponent<Movement>().enabled = true;
			GetComponent<Shoot>().enabled = true;
			GetComponent<Rigidbody>().constraints = rbc;
		}
		else
		{
			healthText.text = "100";
			GetComponent<Movement>().enabled = false;
			GetComponent<Shoot>().enabled = false;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		}
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
