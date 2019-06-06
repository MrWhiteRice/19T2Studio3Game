using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

public class ClientUI : MonoBehaviour
{
	//HOW IT WORKS - Jacob Edition:
	/*
	 * on start, initialise everything to zero and clear the serverpoint;
	 * 
	 * on gui show the ping details, this is updated in the "ClienBehaviour"
	 * on gui show either 2 buttons, start or stop pinging
	 * start pinging will set up a loopback unless an ip is specified in the box
	 * stop pinging will clear the server endpoint so that the connection is dead
	 * 
	 * the static function UpdateStats is called by "ClienBehaviour" with the updated stats from pinging the server
	*/

	//the endpoint the ping client should ping, will be a non-created end point when ping should not run
	public static NetworkEndPoint ServerEndPoint { get; private set; }

	//ping statistics
	static int s_PingTime;
	static int s_PingCounter;

	string m_CustomIp = "";

    void Start()
    {
		s_PingTime = 0;
		s_PingCounter = 0;
		ServerEndPoint = default(NetworkEndPoint);
	}

	void OnGUI()
	{
		UpdatePingClientUI();
	}

	// Update the ping statistics displayed in the ui. Should be called from the ping client every time a new ping is complete
	public static void UpdateStats(int count, int time)
	{
		s_PingCounter = count;
		s_PingTime = time;
	}

	void UpdatePingClientUI()
	{
		GUILayout.Label("PING " + s_PingCounter + ": " + s_PingTime + "ms");

		if(GUILayout.Button("startServer"))
		{
			GetComponent<ServerBehaviour>().enabled = true;
		}

		if(GUILayout.Button("startClient"))
		{
			GetComponent<ClientBehaviour>().enabled = true;
		}

		if(!ServerEndPoint.IsValid)
		{
			// Ping is not currently running, display ui for starting a ping
			if(GUILayout.Button("Start ping"))
			{
				ushort port = 9000;
				if(string.IsNullOrEmpty(m_CustomIp))
				{
					var endpoint = NetworkEndPoint.LoopbackIpv4;
					endpoint.Port = port;
					ServerEndPoint = endpoint;
				}
				else
				{
					string[] endpoint = m_CustomIp.Split(':');
					ushort newPort = 0;

					if(endpoint.Length > 1 && ushort.TryParse(endpoint[1], out newPort))
					{
						port = newPort;
					}

					Debug.Log($"Connecting to PingServer at {endpoint[0]}:{port}.");
					ServerEndPoint = NetworkEndPoint.Parse(endpoint[0], port);
				}
			}

			m_CustomIp = GUILayout.TextField(m_CustomIp);
		}
		else
		{
			// Ping is running, display ui for stopping it
			if(GUILayout.Button("Stop ping"))
			{
				ServerEndPoint = default(NetworkEndPoint);
			}
		}
	}
}