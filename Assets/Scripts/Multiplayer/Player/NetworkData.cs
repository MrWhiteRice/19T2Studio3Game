using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkData : NetworkBehaviour
{
	[SyncVar] public int owner;
	[SyncVar] public int health = 100;

	private void Start()
	{
		if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().ID == owner)
		{
			GetComponent<Movement>().enabled = true;
			GetComponent<Shoot>().enabled = true;

			GetComponentInChildren<UnityEngine.UI.Text>().text = "Me: " + health;
		}
		else
		{
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
		}
	}
}