using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
	float offset = 1;

    void Update()
    {
		if(offset <= 0)
		{

			print("Damage Step!");
			Destroy(gameObject);
		}
		else
		{
			offset -= Time.deltaTime;
		}
    }
}
