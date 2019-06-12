using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
	public void Quit()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Disconnect();
	}

	public void Rematch()
	{
		Quit();
	}
}