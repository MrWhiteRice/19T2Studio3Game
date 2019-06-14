using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
	Player[] players;
	bool gameStart;

    void Start()
    {
		if(!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().isHost)
		{
			Destroy(gameObject);
		}

		players = GameObject.FindObjectsOfType<Player>();
    }

    void Update()
    {
		int ready = 0;

		foreach(Player p in players)
		{
			if(p.ready)
			{
				ready++;
			}
		}

		GameObject player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().player;

		if(ready == 2 && !gameStart)
		{
			foreach(Player p in players)
			{
				//spawn characters for players to control
				GameObject pSpawn = Instantiate(player);
				pSpawn.GetComponent<NetworkData>().owner = p.GetComponent<PlayerData>().ID;
				UnityEngine.Networking.NetworkServer.SpawnWithClientAuthority(pSpawn, p.gameObject);

				//p.CmdSpawnPlayer();
			}

			gameStart = true;
		}
    }
}