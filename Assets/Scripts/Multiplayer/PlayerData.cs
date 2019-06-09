using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : NetworkBehaviour
{
	[SyncVar]
	public new string name;

	[SyncVar]
	public bool ready;

	[SyncVar]
	public bool isHost;

	public void UpdateName(string inputName)
	{
		name = inputName;
	}

	public void UpdateBool(bool set)
	{
		ready = set;
	}
}