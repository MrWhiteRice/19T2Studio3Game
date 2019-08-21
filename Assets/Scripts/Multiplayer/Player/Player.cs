using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	[SyncVar] public int ID = -1;
	[SyncVar] public bool ready;
	[SyncVar] public bool isHost;

	void Start()
	{
		Debug.LogError("bro im broke");
		DontDestroyOnLoad(this);

		if(!isLocalPlayer)
		{
			Debug.LogError("return");
			return;
		}

		ID = isServer ? 0 : 1;
		isHost = isServer ? true : false;

		tag = "Player";
	}

	private void Update()
	{
		if(isHost)
		{
			//get all players
			int ready = 0;
			foreach(Player p in FindObjectsOfType<Player>())
			{
				if(p.ready)
				{
					ready++;
				}
			}

			if(ready >= 2)
			{
				print("all ready!");
			}
		}
	}

	public void UpdateBool(bool set)
	{
		ready = set;
	}
}