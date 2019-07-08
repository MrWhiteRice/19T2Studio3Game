using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject player;
	public int turn = 0;
	public List<GameObject> redPlayers = new List<GameObject>();
	public List<GameObject> bluePlayers = new List<GameObject>();

	public enum TurnPhase
	{
		Move = 0,
		Shoot = 1,
		Damage = 2,
		Death = 3
	}
	public TurnPhase phase = TurnPhase.Move;

    void Start()
    {
		SpawnPlayers();
		NumberPlayers();
    }

    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Return))
		{
			NextTurn();
		}
    }

	public void NextTurn()
	{
		turn++;

		turn %= 6;

		phase = TurnPhase.Move;
	}

	public void NextPhase()
	{
		FindObjectOfType<GameManager>().phase++;

		if((int)FindObjectOfType<GameManager>().phase == 4)
		{
			NextTurn();
			FindObjectOfType<GameManager>().phase = GameManager.TurnPhase.Move;
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