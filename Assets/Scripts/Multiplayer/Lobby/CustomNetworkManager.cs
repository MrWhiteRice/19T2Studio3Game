using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
	public LobbySP lobby;
	public InputField input;
	public GameObject play;

	float check = 1;

	public int connected = 0;

	public Button button;
	public Button button2;

	private void Update()
	{
		bool ready = false;

		foreach(Player p in FindObjectsOfType<Player>())
		{
			if(p.isHost)
			{
				ready = p.gameStart;
			}
		}

		if(!ready)
		{
			button.interactable = !singleton.isNetworkActive;
			button2.interactable = !singleton.isNetworkActive;

			if(play != null)
			{
				if(!play.activeSelf)
				{
					if(singleton.isNetworkActive)
					{
						check -= Time.deltaTime;

						if(check <= 0)
						{
							if(connected == 0)
							{
								print("unloading multiplay module!");
								Disconnect();
							}
						}
					}
					else
					{
						check = 1;
						connected = 0;
					}
				}
				else
				{
					check = 1;
					connected = 0;
				}
			}
		}
	}

	public void HostTheMatch()
	{
		singleton.StartHost();
		lobby.local = false;
		lobby.gameObject.SetActive(true);
	}

	public void JoinTheMatch()
	{
		//create input string
		string useString = input.text;
		//check if null
		if(useString == "")
		{
			//fix
			useString = "localhost";
		}

		print("connecting with: " + useString);

		//connect timer
		check = 10;
		//set ip
		singleton.networkAddress = useString;
		//try connect
		singleton.StartClient();
	}

	public void Disconnect()
	{
		singleton.client.Disconnect();
		singleton.StopHost();
	}
}