using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	public int id = -1;

	void Start()
	{
		if(!isLocalPlayer)
		{
			return;
		}

		if(isServer)
		{
			GetComponent<PlayerData>().isHost = true;
		}

		tag = "Player";

		CmdUpdateName("Player" + Random.Range(0, 99999));
	}

	void Update()
	{
		if(!isLocalPlayer)
		{
			return;
		}
	}

	[Command]
	public void CmdUpdateName(string inputName)
	{
		RpcUpdateName(inputName);
	}

	[ClientRpc]
	void RpcUpdateName(string inputName)
	{
		GetComponent<PlayerData>().UpdateName(inputName);
	}

	[Command]
	public void CmdUpdateReady()
	{
		RpcUpdateReady(!GetComponent<PlayerData>().ready);
	}

	[ClientRpc]
	void RpcUpdateReady(bool set)
	{
		GetComponent<PlayerData>().UpdateBool(set);
	}
}