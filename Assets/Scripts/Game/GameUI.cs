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
	public Text controls;
	public Button nextPhaseButton;

	GameObject win;

	private void Update()
	{
		int red = 0;
		int blue = 0;

		if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.End || FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.Damage)
		{
			nextPhaseButton.interactable = false;
		}
		else
		{
			nextPhaseButton.interactable = true;
		}

		text.text = "Current Phase\n" + FindObjectOfType<GameManager>().phase.ToString();

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

				
				foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
				{
					if(player.health <= 0)
					{
						Destroy(player.gameObject);
					}
					else
					{
						
					}
				}
			}
		}
		else
		{
			if(endtext!= null)
			endtext.text = "";
		}

		red = 0;
		blue = 0;
		foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
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

		if(red == 0)
		{
			if(win == null)
			{
				FindObjectOfType<GameManager>().enabled = false;
				win = (GameObject)Instantiate(Resources.Load("GameOver"));
				win.GetComponentInChildren<Text>().color = Color.blue;
				win.GetComponentInChildren<Text>().text = "Game Over!\nBlue Team Wins!";
			}

			if(timer >= 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}
		else if(blue == 0)
		{
			if(win == null)
			{
				FindObjectOfType<GameManager>().enabled = false;
				win = (GameObject)Instantiate(Resources.Load("GameOver"));
				win.GetComponentInChildren<Text>().color = Color.red;
				win.GetComponentInChildren<Text>().text = "Game Over!\nRed Team Wins!";
			}

			if(timer >= 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}

		ControlsText();
	}

	public void ControlsText()
	{
		switch(FindObjectOfType<GameManager>().phase)
		{
			case GameManager.TurnPhase.Move:
				controls.text = 
					"A: Left\n" +
					"D: Right\n\n" +
					"Jump\n" +
					"W: Up\n" +
					"Space: Forward";
				break;

			case GameManager.TurnPhase.Shoot:
				controls.text =
					"Mouse: Aim\n" +
					"LMB: Shoot";
				break;
		}
	}

	public void NextPhase()
	{
		if(FindObjectOfType<GameManager>().phase != GameManager.TurnPhase.End || FindObjectOfType<GameManager>().phase != GameManager.TurnPhase.Damage)
		{
			FindObjectOfType<GameManager>().NextPhase();
		}
	}

	//public void SkipTurn()
	//{
	//	FindObjectOfType<GameManager>().NextTurn();
	//}
}