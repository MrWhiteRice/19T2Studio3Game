using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
	[SyncVar] public int ID = -1;
	[SyncVar] public bool ready;
	[SyncVar] public bool isHost;
	[SyncVar] public bool gameStart;
	[SyncVar] public bool lobbyReady;
	bool levelReady;
	string sceneName;
	[SyncVar]public bool spawned;
	bool toldSpawn;

	CharacterParty[] party;
	public List<GameObject> partyChars = new List<GameObject>();

	void Start()
	{
		DontDestroyOnLoad(this);

		if(!isLocalPlayer)
		{
			return;
		}
		party = FindObjectOfType<LootBox>().data.party;

		CmdSetID(isServer ? 0 : 1);
		isHost = isServer ? true : false;

		if(!isHost)
		{
			CustomNetworkManager m = FindObjectOfType<CustomNetworkManager>();
			m.lobby.local = false;
			m.lobby.gameObject.SetActive(true);
		}

		tag = "MyPlayer";
	}

	//CMD RPC
	#region
	[Command]
	public void CmdSetID(int set)
	{
		RpcSetID(set);
	}

	[ClientRpc]
	void RpcSetID(int set)
	{
		ID = set;
	}

	[ClientRpc]
	void RpcStartGame()
	{
		FindObjectOfType<GameManager>().gameStart = true;
	}

	[Command]
	public void CmdSetLobbyReady(bool set)
	{
		RpcLobbyReady(set);
	}

	[ClientRpc]
	void RpcLobbyReady(bool set)
	{
		lobbyReady = set;
	}

	[Command]
	public void CmdSetGameStart(bool set)
	{
		RpcSetGameStart(set);
	}

	[ClientRpc]
	void RpcSetGameStart(bool set)
	{
		foreach(Player p in FindObjectsOfType<Player>())
		{
			p.gameStart = set;
		}
	}

	[Command]
	public void CmdReady(bool set)
	{
		RpcReady(set);
	}

	[ClientRpc]
	void RpcReady(bool set)
	{
		ready = set;
	}

	[Command]
	public void CmdSetSpawn(bool set)
	{
		RpcSetSpawn(set);
	}

	[ClientRpc]
	void RpcSetSpawn(bool set)
	{
		foreach(Player p in FindObjectsOfType<Player>())
		{
			p.spawned = set;
		}
	}

	[Command]
	public void CmdSpawnCharacters()
	{
		RpcSpawnCharacters();
	}

	[ClientRpc]
	void RpcSpawnCharacters()
	{
		//find all spawn points
		SpawnPoint[] points = FindObjectsOfType<SpawnPoint>();

		//init red and blue list
		List<SpawnPoint> r = new List<SpawnPoint>();
		List<SpawnPoint> b = new List<SpawnPoint>();

		//cycle all spawn points
		foreach(SpawnPoint s in points)
		{
			//add depending on team
			if(s.GetTeam() == SpawnPoint.Team.Blue)
			{
				b.Add(s);
			}
			else
			{
				r.Add(s);
			}
		}

		//cycle all players
		foreach(Player p in FindObjectsOfType<Player>())
		{
			for(int x = 0; x < 3; x++)
			{
				GameObject spawn = (GameObject)Instantiate((GameObject)Resources.Load("Character SP"), p.ID == 0 ? r[x].transform.position : b[x].transform.position, Quaternion.identity);
				spawn.GetComponent<PlayerDataSP>().teamInt = p.ID;
				spawn.name = p.ID.ToString();
				p.partyChars.Add(spawn);

				NetworkServer.Spawn(spawn);
			}
		}
	}

	[Command]
	public void CmdInitPlayer(int index, int player, int playerID, int classID, int traversalID, int weaponID)
	{
		RpcInitPlayer(index, player, playerID, classID, traversalID, weaponID);
	}

	[ClientRpc]
	void RpcInitPlayer(int index, int player, int playerID, int classID, int traversalID, int weaponID)
	{
		//cycle both players
		foreach(Player p in FindObjectsOfType<Player>())
		{
			//check if player match
			if(p.ID == player)
			{
				//cycle all character people
				foreach(PlayerDataSP sp in FindObjectsOfType<PlayerDataSP>())
				{
					//check if team match
					if(sp.teamInt == p.ID)
					{
						//check that it hasnt been initialised
						if(sp.character != null)
						{
							//double check if hasnt been initialised
							if(sp.character.playerID == 0)
							{
								sp.character = new CharacterParty(playerID, weaponID, classID, traversalID);
								sp.generate = true;
								return;
							}
						}
					}
				}
			}
		}
	}
#endregion

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		sceneName = scene.name;
		levelReady = true;
	}

	private void Update()
	{
		if(!isLocalPlayer)
		{
			return;
		}

		if(levelReady)
		{
			if(FindObjectsOfType<Player>().Length >= 2)
			{
				if(CompareTag("MyPlayer"))
				{
					if(sceneName.Contains("Terrain"))
					{
						CmdReady(true);
					}
				}
			}
		}

		if(isHost)
		{
			//count all ready players
			int count = 0;
			foreach(Player p in FindObjectsOfType<Player>())
			{
				if(p.ready)
				{
					count++;
				}
			}

			//if all players are ready
			if(count == 2)
			{
				//find all spawn points
				SpawnPoint[] points = FindObjectsOfType<SpawnPoint>();

				//init red and blue list
				List<SpawnPoint> r = new List<SpawnPoint>();
				List<SpawnPoint> b = new List<SpawnPoint>();

				//cycle all spawn points
				foreach(SpawnPoint s in points)
				{
					//add depending on team
					if(s.GetTeam() == SpawnPoint.Team.Blue)
					{
						b.Add(s);
					}
					else
					{
						r.Add(s);
					}
				}

				if(!toldSpawn)
				{
					toldSpawn = true;

					CmdSpawnCharacters();
					CmdSetSpawn(true);
				}
			}

			int createdReadyPlayers = 0;
			foreach(PlayerDataSP p in FindObjectsOfType<PlayerDataSP>())
			{
				if(p.character.playerID != 0)
				{
					createdReadyPlayers++;
				}
			}

			if(createdReadyPlayers == 6)
			{
				if(!FindObjectOfType<GameManager>().gameStart)
				{
					RpcStartGame();
					//FindObjectOfType<GameManager>().BeginGame();
				}
			}
		}

		if(spawned)
		{
			if(CompareTag("MyPlayer"))
			{
				for(int x = 0; x < 3; x++)
				{
					InitPlayer(x, ID, party[x]);
				}
			}
		}
	}

	void InitPlayer(int index, int player, CharacterParty person)
	{
		CmdInitPlayer(index, player, person.playerID, person.classID, person.traversalID, person.weaponID);
	}
}