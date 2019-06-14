using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
	public void HostMatch()
	{
		singleton.StartHost();
	}

	public void JoinMatch(string ip)
	{
		singleton.networkAddress = ip;
		singleton.StartClient();
	}

	public void Disconnect()
	{
		singleton.client.Disconnect();
		singleton.StopHost();
	}
}