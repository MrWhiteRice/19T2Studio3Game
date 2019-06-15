using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkData : NetworkBehaviour
{
	[SyncVar] public int owner;
	[SyncVar] public int health = 100;
	[SyncVar] public int ID;
	Text text;

	private void Start()
	{
		text = GetComponentInChildren<Text>();

		if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().ID == owner)
		{
			GetComponent<Movement>().enabled = true;
			GetComponent<Shoot>().enabled = true;
		}
		else
		{
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
		}
	}

	private void Update()
	{
		string a = IsMyTurn() ? "Me: " : "";
		text.text = a + health.ToString();

		if(Input.GetMouseButtonDown(2))
		{
			if(IsMyTurn())
			{
				GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdEndTurn();
			}
		}
	}

	bool IsMyTurn()
	{
		int mod = 0;

		if(!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().isHost)
		{
			mod = 3;
		}

		if(ID == FindObjectOfType<RoundManager>().turnOrder)
		{
			return true;
		}

		return false;
	}

	[Command] public void CmdDamage(int amount)
	{
		health -= amount;
	}
}