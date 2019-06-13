using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
	public Transform gun;

    void Update()
    {
		Vector2 me = gun.transform.position;
		Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		gun.transform.rotation = Quaternion.LookRotation(mouse - me);
    }
}