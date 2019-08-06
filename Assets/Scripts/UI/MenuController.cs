using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public GameObject[] menu;

	public void DisableAll()
	{
		foreach(GameObject obj in menu)
		{
			if(obj != null)
			obj.SetActive(false);
		}
	}

	public void PlayGame()
	{
		SceneManager.LoadScene("Terrain 2");
	}

	public void Quit()
	{
		Application.Quit();
	}
}