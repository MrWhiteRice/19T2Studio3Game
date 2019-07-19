using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	float timer = 3;
	//public void Skip()
	//{
	//	GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdEndTurn();
	//}

	public Text text;
	public Text endtext;

	private void Update()
	{
		text.text = "Current Phase: " + FindObjectOfType<GameManager>().phase.ToString();

		//TODO: move to game manager
		if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.End)
		{
			endtext.text = "Next Turn: " + timer.ToString("f0");

			if(timer >= 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				NextPhase();
				timer = 3;

				int red = 0;
				int blue = 0;
				foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
				{
					if(player.health <= 0)
					{
						Destroy(player.gameObject);
					}
					else
					{
						switch(player.team)
						{
							case SpawnPoint.Team.Blue:
								blue++;
								break;
							case SpawnPoint.Team.Red:
								red++;
								break;
						}
					}
				}

				if(red == 0)
				{
					FindObjectOfType<GameManager>().enabled = false;
					GameObject win = (GameObject)Instantiate(Resources.Load("GameOver"));
					win.GetComponentInChildren<Text>().color = Color.blue;
					win.GetComponentInChildren<Text>().text = "Game Over!\nBlue Team Wins!";
				}
				else if(blue == 0)
				{
					FindObjectOfType<GameManager>().enabled = false;
					GameObject win = (GameObject)Instantiate(Resources.Load("GameOver"));
					win.GetComponentInChildren<Text>().color = Color.red;
					win.GetComponentInChildren<Text>().text = "Game Over!\nRed Team Wins!";
				}
			}
		}
		else
		{
			if(endtext!= null)
			endtext.text = "";
		}
	}

	public void NextPhase()
	{
		FindObjectOfType<GameManager>().NextPhase();
	}

	public void SkipTurn()
	{
		FindObjectOfType<GameManager>().NextTurn();
	}
}