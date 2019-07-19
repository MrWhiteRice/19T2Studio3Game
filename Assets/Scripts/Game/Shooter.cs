using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
	int shots;
	float timeBetweenShots;
	int damage;
	int knockback;
	int accuracy;

	public void Init(int Shots, float TimeBetweenShots, int Damage, int Knockback, int Accuracy)
	{
		shots = Shots;
		timeBetweenShots = TimeBetweenShots;
		damage = Damage;
		knockback = Knockback;
		accuracy = Accuracy;

		InvokeRepeating("Shoot", timeBetweenShots, timeBetweenShots);
	}

	void Shoot()
	{
		Vector3[] pos = new Vector3[2];
		pos[0] = transform.position;

		GameObject bang = new GameObject("Bang");
		Destroy(bang, timeBetweenShots/2);

		//if hit
		if(Physics.SphereCast(new Ray(transform.position, transform.right), 0.1f, out RaycastHit hit))
		{
			pos[1] = hit.point;

			//if hit player
			if(hit.transform.CompareTag("Player"))
			{
				Vector3 forceDir = hit.transform.position - transform.position;
				hit.transform.GetComponent<Rigidbody>().velocity = (forceDir.normalized + Vector3.up) * knockback;

				hit.transform.GetComponent<PlayerDataSP>().health -= damage;
			}
		}
		else
		{
			pos[1] = transform.right * 100;
		}

		bang.AddComponent<LineRenderer>().SetPositions(pos);
		LineRenderer lr = bang.GetComponent<LineRenderer>();
		lr.startWidth = 0.1f;
		lr.endWidth = 0.1f;

		lr.material = Resources.Load<Material>("Line");

		//check delete
		if(--shots <= 0)
		{
			Destroy(gameObject);
		}

		//apply recoil
		Vector3 rot = transform.eulerAngles;
		rot.z += Random.Range(-accuracy, accuracy);
		transform.eulerAngles = rot;
	}
}
