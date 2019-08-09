using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject player;
	public int turn = 6;
	public List<GameObject> redPlayers = new List<GameObject>();
	public List<GameObject> bluePlayers = new List<GameObject>();
	bool gameStart = false;
	int selectedLevel = 0;

	public DataContainer data;

	public enum TurnPhase
	{
		Move = 0,
		Shoot = 1,
		Damage = 2,
		End = 3
	}
	public TurnPhase phase = TurnPhase.End;

    void Start()
    {
		data = SaveSystem.loadData(PlayerPrefs.GetInt("ActivePlayer", -1));

		turn = Random.Range(0, 6);
		SpawnPlayers();
		NumberPlayers();
		Invoke("BeginGame", 3f);
    }

	void BeginGame()
	{
		gameStart = true;
	}

    void Update()
    {
		if(phase == TurnPhase.Damage)
		{
			if(GameObject.FindGameObjectsWithTag("Weapon").Length == 0)
			{
				NextPhase();
			}
		}

		if(gameStart)
		{
			foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
			{
				if(player.ID == turn)
				{
					return;
				}
			}

			print("yuh");
			NextTurn();
		}
	}

	public void NextTurn()
	{
		turn++;

		turn %= 6;

		phase = TurnPhase.Move;

		foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
		{
			player.stamina = 100;
			if(player.ID == turn)
			{
				return;
			}
		}

		NextTurn();
	}

	public void NextPhase()
	{
		FindObjectOfType<GameManager>().phase++;

		if((int)FindObjectOfType<GameManager>().phase == 4)
		{
			NextTurn();
			FindObjectOfType<GameManager>().phase = GameManager.TurnPhase.Move;

			foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
			{
				player.hurt = false;
			}
		}
	}

	void SpawnPlayers()
	{
		foreach(SpawnPoint point in FindObjectsOfType<SpawnPoint>())
		{
			GameObject p = Instantiate(player);
			p.GetComponent<PlayerDataSP>().SetTeam(point.GetTeam());
			p.transform.position = point.transform.position;

			if(point.GetTeam() == SpawnPoint.Team.Red)
			{
				redPlayers.Add(p);
			}
			else
			{
				bluePlayers.Add(p);
			}
		}
	}

	void NumberPlayers()
	{
		int number = 0;
		//red - first - 0, 2, 4
		foreach(GameObject p in redPlayers)
		{
			p.GetComponent<PlayerDataSP>().ID = number;
			number += 2;
		}

		number = 1;
		//blue - second - 1, 3, 5
		foreach(GameObject p in bluePlayers)
		{
			p.GetComponent<PlayerDataSP>().ID = number;
			number += 2;
		}
	}
}