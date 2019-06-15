using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public enum Team
	{
		//host
		Red,
		//client
		Blue
	}

	[SerializeField]
	Team team;

	bool spawned;

	public Team GetTeam()
	{
		return team;
	}

	public bool Spawned
	{
		get{ return spawned; }
		set{ spawned = value; }
	}

	private void OnDrawGizmos()
	{
		switch(team)
		{
			case Team.Blue:
				Gizmos.color = Color.blue;
				break;
			case Team.Red:
				Gizmos.color = Color.red;
				break;
		}

		Gizmos.DrawWireSphere(transform.position, 0.25f);
	}
}