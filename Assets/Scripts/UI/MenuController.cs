using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public bool selectedController = false;

	public GameObject[] menu;
	public GameObject[] profiles;

	public Text free;
	public Text paid;

	private void Start()
	{
		PlayerPrefs.DeleteKey("Player1Controller");
		PlayerPrefs.DeleteKey("Player2Controller");
	}

	private void Update()
	{
		//Beginning of the game, check what controller type player 1 is using
		BeginMenu(false);

		//set currency amounts
		if(paid.gameObject.activeSelf)
		{
			paid.text = FindObjectOfType<LootBox>().data.paid.ToString();
			free.text = FindObjectOfType<LootBox>().data.free.ToString();
		}

		if(menu[1].activeSelf)
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

					if(profiles.Length > 0)
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
					if(profiles.Length > 0)
						profiles[x].GetComponentInChildren<Text>().text = "Create New Profile";
				}
			}
		}
	}

	public void BeginMenu(bool mobile)
	{
		//Beginning of the game, check what controller type player 1 is using
		if(!selectedController)
		{
			//Check if mobile
			if(mobile)
			{
				PlayerPrefs.SetInt("Player1Controller", -1);
				selectedController = true;
			}
			//Check if keyboard is to be used
			else if(Input.GetKeyDown(KeyCode.Space))
			{
				PlayerPrefs.SetInt("Player1Controller", 0);
				selectedController = true;
			}
			else
			{
				//check which controller is being used
				for(int x = 1; x <= 4; x++)
				{
					if(Input.GetKeyDown("joystick " + x + " button 0"))
					{
						print("selected joystick" + x);
						PlayerPrefs.SetInt("Player1Controller", x);
						selectedController = true;
						break;
					}
				}
			}

			if(selectedController)
			{
				DisableAll();
				menu[1].SetActive(true);
			}
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