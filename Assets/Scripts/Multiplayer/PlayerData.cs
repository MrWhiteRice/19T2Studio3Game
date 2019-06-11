using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : NetworkBehaviour
{
	[SyncVar] public int ID = -1;
	[SyncVar] public new string name;
	[SyncVar] public bool ready;
	[SyncVar] public bool isHost;

	[SyncVar] public int health = 100;

	public void UpdateName(string inputName)
	{
		name = inputName;
	}

	public void UpdateBool(bool set)
	{
		ready = set;
	}

	public void UpdateID(int id)
	{
		ID = id;
	}
}