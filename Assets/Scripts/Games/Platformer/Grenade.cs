using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
	public float timer = 3;
	public float explosionRadius = 1;

    void Update()
    {
		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			FindObjectOfType<QuadtreeComponent>().Quadtree.InsertCircle(false, transform.position, explosionRadius);
			Destroy(gameObject);
		}
    }
}