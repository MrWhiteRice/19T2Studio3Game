using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
	public void Skip()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdEndTurn();
	}
}