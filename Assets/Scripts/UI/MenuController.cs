using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public GameObject[] menu;
	int selected;

	void ChangeMenu(int index)
	{
		foreach(GameObject obj in menu)
		{
			obj.SetActive(false);
		}

		menu[index].SetActive(true);
	}
}
