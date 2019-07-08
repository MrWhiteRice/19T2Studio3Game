using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	//public void Skip()
	//{
	//	GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdEndTurn();
	//}

	public Text text;

	private void Update()
	{
		text.text = "Current Phase: " + FindObjectOfType<GameManager>().phase.ToString();
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