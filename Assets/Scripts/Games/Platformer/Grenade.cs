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
			FindObjectOfType<QuadtreeComponent>().Quadtree.InsertCircle(false, transform.position, explosionRadius);
			Destroy(gameObject);

			foreach(Collider hit in Physics.OverlapSphere(transform.position, explosionRadius, player))
			{
				Vector3 forceDir = hit.transform.position - transform.position;
				hit.GetComponent<Rigidbody>().velocity = (forceDir + Vector3.up) * 5;
			}
		}
    }
}