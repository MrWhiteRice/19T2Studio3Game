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

	public void Payment(int amount)
	{
		print("adding " + amount + " to account!");
		Application.OpenURL("https://cdn.cultofmac.com/wp-content/uploads/2014/12/apple-store-online-paypal-payment-screen-780x502.jpg");
	}
}