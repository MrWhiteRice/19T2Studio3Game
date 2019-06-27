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

	public RigidbodyConstraints2D rbc;

	private void Start()
	{
		text = GetComponentInChildren<Text>();

		//rbc = GetComponent<Rigidbody2D>().constraints;

		if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().ID == owner)
		{
			GetComponent<Movement>().enabled = true;
			GetComponent<Shoot>().enabled = true;
		}
	}

	private void Update()
	{
		string a = IsMyTurn() ? "Me: " : "";
		text.text = a + health.ToString();


		if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().ID == owner)
		{
			if(IsMyTurn())
			{
				//GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			}
			else
			{
				//GetComponent<Rigidbody2D>().constraints = rbc;
			}
		}
	}

	public bool IsMyTurn()
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