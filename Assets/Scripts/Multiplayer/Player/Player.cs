using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
	public bool gameStart;
	public GameObject player;
	public GameObject bullet;
	[SyncVar] public bool ready;

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
				//GameObject.Find("Enemy Health").GetComponent<UnityEngine.UI.Slider>().value = GetComponent<PlayerData>().health;
			}
			return;
		}

		if(gameStart)
		{
			//GameObject.Find("Player Health").GetComponent<UnityEngine.UI.Slider>().value = GetComponent<PlayerData>().health;
		}

		//TODO: Change name to correct scene
		if(SceneManager.GetActiveScene().name == "Platformer")
		{
			CmdReady(true);
		}
	}

	void GameOver(int id)
	{
		GameObject.Find("Canvas Game Over").transform.GetChild(0).gameObject.SetActive(true);
		GameObject.FindObjectOfType<BoardManager>().enabled = false;
		gameStart = false;
	}

	[Command] void CmdReady(bool set)
	{
		RpcReady(set);
	}
	[ClientRpc] void RpcReady(bool set)
	{
		ready = set;
	}

	[Command] public void CmdSpawnPlayer()
	{
		GameObject p = Instantiate(player);
		p.GetComponent<NetworkData>().owner = GetComponent<PlayerData>().ID;
		NetworkServer.SpawnWithClientAuthority(p, gameObject);
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

				if(players[x].health <= 0)
				{
					GameOver(players[x].ID);
				}
			}
		}
	}

	public void Disconnect()
	{
		CmdDisconnect();
	}

	[Command] void CmdDisconnect()
	{
		RpcDisconnect();
	}
	[ClientRpc] void RpcDisconnect()
	{
		FindObjectOfType<CustomNetworkManager>().Disconnect();
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
		SceneManager.LoadScene("Platformer");

		foreach(Player p in GameObject.FindObjectsOfType<Player>())
		{
			p.gameStart = true;
		}
	}

	[Command] public void CmdShoot(int dir, Vector3 pos)
	{
		GameObject b = Instantiate(bullet);

		b.transform.position = pos;
		b.GetComponent<Bullet>().dir = dir;

		NetworkServer.SpawnWithClientAuthority(b, gameObject);
	}
}