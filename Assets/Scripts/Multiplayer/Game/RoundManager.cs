using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RoundManager : NetworkBehaviour
{
	Player[] players;
	bool gameStart;

	bool isHost;

	[SyncVar]public int turnOrder = 0;

	void Start()
    {
		if(!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>().isHost)
		{
			return;
		}

		isHost = true;
		players = GameObject.FindObjectsOfType<Player>();
    }

    void Update()
    {
		if(!isHost)
		{
			return;
		}

		if(turnOrder > 5)
		{
			turnOrder = 0;
		}

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
				//get which type of player we have | 0 = host, 1 = client
				int type = p.GetComponent<PlayerData>().isHost ? 0 : 1;

				//create spawnpoints array
				List<SpawnPoint> points = new List<SpawnPoint>();

				//get all spawnpoints of that type
				foreach(SpawnPoint sp in FindObjectsOfType<SpawnPoint>())
				{
					if((int)sp.GetTeam() == type)
					{
						points.Add(sp);
					}
				}

				//round robin spawn players at the spawn points
				for(int x = 0; x < 3; x++)
				{
					//get random spawnpoint
					int selected = Random.Range(0, points.Count);
					
					//spawn characters for players to control
					GameObject pSpawn = Instantiate(player);
					pSpawn.transform.position = points[selected].transform.position;
					pSpawn.GetComponent<NetworkData>().owner = p.GetComponent<PlayerData>().ID;
					int mod = !p.GetComponent<PlayerData>().isHost ? 3 : 0;
					pSpawn.GetComponent<NetworkData>().ID = x + mod;
					NetworkServer.SpawnWithClientAuthority(pSpawn, p.gameObject);

					//remove index from list
					points[selected].Spawned = true;
					points.Remove(points[selected]);
				}
			}

			gameStart = true;
		}
    }
}