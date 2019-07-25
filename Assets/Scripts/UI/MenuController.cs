using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public GameObject[] menu;

	public void DisableAll()
	{
		foreach(GameObject obj in menu)
		{
			obj.SetActive(false);
		}
	}
}
