using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	public bool gameStart;

	void Start()
	{
		DontDestroyOnLoad(this);

		if(!isLocalPlayer)
		{
			return;
		}

		if(isServer)
		{
			GetComponent<PlayerData>().isHost = true;
		}

		tag = "Player";

		CmdUpdateID(Random.Range(0, 99999));
		CmdUpdateName("Player" + Random.Range(0, 99999));
	}

	void Update()
	{
		if(!isLocalPlayer)
		{
			if(gameStart)
			{
				GameObject.Find("Enemy Health").GetComponent<UnityEngine.UI.Slider>().value = GetComponent<PlayerData>().health;
			}
			return;
		}

		if(gameStart)
		{
			GameObject.Find("Player Health").GetComponent<UnityEngine.UI.Slider>().value = GetComponent<PlayerData>().health;
		}
	}

	[Command] public void CmdDealDamage(int damage)
	{
		RpcDealDamage(damage, GetComponent<PlayerData>().ID);
	}
	[ClientRpc] void RpcDealDamage(int damage, int id)
	{
		PlayerData[] players = GameObject.FindObjectsOfType<PlayerData>();
		for(int x = 0; x < players.Length; x++)
		{
			if(players[x].ID != id)
			{
				players[x].health -= damage;
			}
		}
	}

	[Command] public void CmdUpdateName(string inputName)
	{
		RpcUpdateName(inputName);
	}
	[ClientRpc] void RpcUpdateName(string inputName)
	{
		GetComponent<PlayerData>().UpdateName(inputName);
	}

	[Command] void CmdUpdateID(int id)
	{
		RpcUpdateID(id);
	}
	[ClientRpc] void RpcUpdateID(int id)
	{
		GetComponent<PlayerData>().UpdateID(id);
	}

	[Command] public void CmdUpdateReady()
	{
		RpcUpdateReady(!GetComponent<PlayerData>().ready);
	}
	[ClientRpc] void RpcUpdateReady(bool set)
	{
		GetComponent<PlayerData>().UpdateBool(set);
	}

	[Command] public void CmdLoadLevel()
	{
		RpcLoadLevel();
	}
	[ClientRpc]	void RpcLoadLevel()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Match 3");

		foreach(Player p in GameObject.FindObjectsOfType<Player>())
		{
			p.gameStart = true;
		}
	}
}