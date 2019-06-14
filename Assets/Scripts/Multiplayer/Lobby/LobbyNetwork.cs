using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class LobbyNetwork : MonoBehaviour
{
	public InputField ip;
	public Text localIP;

	private void Start()
	{
		localIP.text = LocalIPAddress();
	}

	public void Host()
	{
		GameObject.FindObjectOfType<CustomNetworkManager>().HostMatch();
	}

	public void Join()
	{
		if(ip.text != "")
		{
			GameObject.FindObjectOfType<CustomNetworkManager>().JoinMatch(ip.text);
		}
		else
		{
			GameObject.FindObjectOfType<CustomNetworkManager>().JoinMatch(LocalIPAddress());
		}
	}

	public static string LocalIPAddress()
	{
		IPHostEntry host;
		string localIP = "0.0.0.0";
		host = Dns.GetHostEntry(Dns.GetHostName());

		foreach(IPAddress ip in host.AddressList)
		{
			if(ip.AddressFamily == AddressFamily.InterNetwork)
			{
				localIP = ip.ToString();
				break;
			}
		}

		return localIP;
	}
}