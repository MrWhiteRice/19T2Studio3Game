using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkData : NetworkBehaviour
{
	[SyncVar] public int owner;
	[SyncVar] public int health = 100;
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
		text.text = health.ToString();
	}

	[Command] public void CmdDamage(int amount)
	{
		health -= amount;
	}
}