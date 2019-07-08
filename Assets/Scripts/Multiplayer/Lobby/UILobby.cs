//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class UILobby : MonoBehaviour
//{
//	public InputField text;

//	public Text player1;
//	public Text player2;

//	public Toggle p1;
//	public Toggle p2;

//	public Button start;

//	void Update()
//	{
//		PlayerData[] players = GameObject.FindObjectsOfType<PlayerData>();

//		int ready = 0;
//		bool isHost = false;

//		for(int x = 0; x < players.Length; x++)
//		{
//			if(players[x].ready)
//			{
//				ready++;
//			}

//			if(players[x].CompareTag("Player"))
//			{
//				player1.text = players[x].name;
//				p1.isOn = players[x].ready;

//				isHost = players[x].isHost;
//			}
//			else
//			{
//				player2.text = players[x].name;
//				p2.isOn = players[x].ready;
//			}
//		}

//		if(isHost)
//		{
//			start.interactable = ready == 2 ? true : false;
//		}
//	}

//	public void UpdateName()
//	{
//		GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdUpdateName(text.text);
//	}

//	public void UpdateReady()
//	{
//		GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdUpdateReady();
//	}

//	public void StartGame()
//	{
//		GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdLoadLevel();
//	}
//}