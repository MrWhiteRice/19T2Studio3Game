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

		if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.Death)
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
			}
		}
		else
		{
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