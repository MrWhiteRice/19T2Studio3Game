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

	[Space]
	public GameObject player;

	private void Update()
	{
		button.interactable = !singleton.isNetworkActive;
		button2.interactable = !singleton.isNetworkActive;

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

	//public override void OnClientConnect(NetworkConnection conn)
	//{
	//	connected++;
	//	lobby.local = false;
	//	lobby.gameObject.SetActive(true);
	//}

	public void HostTheMatch()
	{
		if(singleton.isNetworkActive == false)
		{
			singleton.StartHost();
			lobby.local = false;
			lobby.gameObject.SetActive(true);
		}
	}

	public void JoinTheMatch()
	{
		print("connecting with: " + input.text);

		string useString = input.text;
		if(useString == "")
		{
			useString = "localhost";
		}

		check = 10;
		singleton.networkAddress = useString;
		singleton.StartClient();
	}

	public void Disconnect()
	{
		singleton.client.Disconnect();
		singleton.StopHost();
	}
}