/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
	[SyncVar]public int dir;

    void Start()
    {
		GetComponent<Rigidbody2D>().velocity = Vector2.right * dir;
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name.Contains("Bullet") == false)
		{
			//test type
			if(other.name.Contains("Character"))
			{
				other.GetComponent<NetworkData>().CmdDamage(10);
			}

			NetworkServer.Destroy(gameObject);
		}
	}
}*/