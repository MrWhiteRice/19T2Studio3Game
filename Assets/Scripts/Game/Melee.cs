using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
	float offset = .15f;
	public LayerMask player;

    void Update()
    {
		if(offset <= 0)
		{
			foreach(Collider hit in Physics.OverlapSphere(transform.position, 0.125f, player))
			{
				if(hit is CapsuleCollider == false)
				{
					continue;
				}

				Vector3 forceDir = hit.transform.position - transform.position;
				hit.GetComponent<Rigidbody>().velocity = (forceDir + Vector3.up) * 5;

				float damageMod = 20;
				hit.GetComponent<PlayerDataSP>().health -= (Mathf.Abs(damageMod));

				hit.GetComponent<PlayerDataSP>().hurt = true;
			}

			Destroy(gameObject);
		}
		else
		{
			offset -= Time.deltaTime;
		}
    }
}
