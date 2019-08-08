using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public GameObject[] menu;
	public GameObject[] profiles;

	public Text free;
	public Text paid;

	private void Start()
	{
		//initialise profiles array
		DataContainer[] data = new DataContainer[3];

		//load profile data
		data[0] = SaveSystem.loadData(0);
		data[1] = SaveSystem.loadData(1);
		data[2] = SaveSystem.loadData(2);

		//apply data to text component
		for(int x = 0; x < 3; x++)
		{
			if(data[x].created)
			{
				int unlockedChar = 0;
				int unlockedWeap = 0;

				foreach(CharacterData c in data[x].unlockedCharacters)
				{
					if(c.unlocked)
					{
						unlockedChar++;
					}
				}

				foreach(WeaponData w in data[x].unlockedWeapons)
				{
					if(w.unlocked)
					{
						unlockedWeap++;
					}
				}

				profiles[x].GetComponentInChildren<Text>().text =
					"Free: " + data[x].free + " " +
					"Paid: " + data[x].paid + " " +
					"Char: " + unlockedChar + " " +
					"Weap: " + unlockedWeap + "\n" +
					"Wins: " + data[x].matchesWon + " " +
					"Loss: " + data[x].mathcesLost + "\n" +
					"Tutorial Complete: " + data[x].tutorialComplete;
			}
			else
			{
				profiles[x].GetComponentInChildren<Text>().text = "Create New Profile";
			}
		}
	}

	private void Update()
	{
		if(paid.gameObject.activeSelf)
		{
			paid.text = FindObjectOfType<LootBox>().data.paid.ToString();
			free.text = FindObjectOfType<LootBox>().data.free.ToString();
		}
	}

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
		FindObjectOfType<LootBox>().data.paid += amount;
	}
}