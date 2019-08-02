using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
	public float timer = 3;
	public float explosionRadius = 1;
	public LayerMask player;

    void Update()
    {
		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			FindObjectOfType<QuadtreeComponent>().Quadtree.InsertCircle(0, transform.position, explosionRadius);
			Destroy(gameObject);

			foreach(Collider hit in Physics.OverlapSphere(transform.position, explosionRadius, player))
			{
				if(hit is CapsuleCollider == false)
				{
					continue;
				}

				Vector3 forceDir = hit.transform.position - transform.position;
				hit.GetComponent<Rigidbody>().velocity = (forceDir + Vector3.up) * 5;

				float damageMod = Mathf.Clamp((1 - Vector3.Distance(transform.position, hit.transform.position)) + 0.5f, 0.5f, 100);
				hit.GetComponent<PlayerDataSP>().health -= (Mathf.Abs(damageMod) * 50);
			}
		}
    }
}