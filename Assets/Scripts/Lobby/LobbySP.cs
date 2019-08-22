using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySP : MonoBehaviour
{
	public GameObject[] players;
	public bool noGo;
	public bool local;
	public bool host;

	public GameObject panel;
	bool playerFound;

	Weapon[] weapons;
	Actor[] characters;

	private void Start()
	{
		weapons = Resources.LoadAll<Weapon>("RiceStuff/Weapons");
		characters = Resources.LoadAll<Actor>("RiceStuff/Actors");
	}

	void Update()
    {
		noGo = false;

		//cycle through the first players party
		for(int x = 0; x < 3; x++)
		{
			PlayerSetUp(x, false);
		}

		if(!local)
		{
			//find players
			playerFound = FindObjectsOfType<Player>().Length >= 2 ? true : false;
		}

		if(!playerFound)
		{
			//if we're local
			if(local)
			{
				//Check if keyboard is to be used
				if(Input.GetKeyDown(KeyCode.Space))
				{
					PlayerPrefs.SetInt("Player2Controller", 0);
					playerFound = true;
				}
				else
				{
					//check which controller is being used
					for(int x = 1; x <= 4; x++)
					{
						if(Input.GetKeyDown("joystick " + x + " button 0"))
						{
							print("selected joystick" + x);
							PlayerPrefs.SetInt("Player2Controller", x);
							playerFound = true;
							break;
						}
					}
				}
			}
		}
		else //found a player
		{
			if(local) //local
			{
				for(int x = 3; x < 6; x++)
				{
					PlayerSetUp(x, true);
					panel.SetActive(false);
				}
			}
			else //online
			{
				LoadPlayerData();
				panel.SetActive(false);

				int ready = 0;
				foreach(Player p in FindObjectsOfType<Player>())
				{
					if(p.lobbyReady)
					{
						ready++;
					}
				}

				if(ready == 2)
				{
					foreach(Player p in FindObjectsOfType<Player>())
					{
						if(p.CompareTag("MyPlayer") && p.isHost)
						{
							p.CmdSetGameStart(true);
						}
					}

					FindObjectOfType<CustomNetworkManager>().ServerChangeScene("Terrain 2");
					enabled = false;
				}
			}
		}
    }

	public void PlayGame()
	{
		//if lan
		if(local)
		{
			if(noGo != true)
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("Terrain 2");
			}
		}
		else//if online
		{
			foreach(Player p in FindObjectsOfType<Player>())
			{
				if(p.CompareTag("MyPlayer"))
				{
					p.CmdSetLobbyReady(true);
				}
			}
		}
	}

	//Load online player into lobby
	void LoadPlayerData()
	{
		for(int x = 3; x < 6; x++)
		{
			int playerSelect = x % 3;

			//setting up player portrait
			foreach(Actor a in characters)
			{
				if(a.ID == FindObjectOfType<LootBox>().data.party[playerSelect].playerID)
				{
					players[x].transform.GetChild(0).GetComponent<Image>().sprite = a.Icon; //player
					break;
				}
				else
				{
					if(FindObjectOfType<LootBox>().data.party[playerSelect].playerID == -1)
					{
						players[x].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Cancel");
						noGo = true;
					}
				}
			}

			//setting up class weapon
			foreach(Weapon w in weapons)
			{
				if(w.ID == FindObjectOfType<LootBox>().data.party[playerSelect].classID)
				{
					players[x].transform.GetChild(1).GetComponent<Image>().sprite = w.Icon; //gun
					break;
				}
				else
				{
					players[x].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Cancel");
				}
			}

			//setting up special weapon
			foreach(Weapon w in weapons)
			{
				if(w.ID == FindObjectOfType<LootBox>().data.party[playerSelect].weaponID)
				{
					players[x].transform.GetChild(2).GetComponent<Image>().sprite = w.Icon; //special
					break;
				}
				else
				{
					//use empty texture
					players[x].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Cancel");
				}
			}

			players[x].transform.GetChild(3).GetComponent<Image>().sprite = null; //movement
			players[x].transform.GetChild(4).GetComponent<Image>().sprite = null; //melee
		}
	}

	void PlayerSetUp(int x, bool p2)
	{
		int playerSelect = p2 ? x % 3 : x;

		//setting up player portrait
		foreach(Actor a in characters)
		{
			if(a.ID == FindObjectOfType<LootBox>().data.party[playerSelect].playerID)
			{
				players[x].transform.GetChild(0).GetComponent<Image>().sprite = a.Icon; //player
				break;
			}
			else
			{
				if(FindObjectOfType<LootBox>().data.party[playerSelect].playerID == -1)
				{
					players[x].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Cancel");
					noGo = true;
				}
			}
		}

		//setting up class weapon
		foreach(Weapon w in weapons)
		{
			if(w.ID == FindObjectOfType<LootBox>().data.party[playerSelect].classID)
			{
				players[x].transform.GetChild(1).GetComponent<Image>().sprite = w.Icon; //gun
				break;
			}
			else
			{
				players[x].transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Cancel");
			}
		}

		//setting up special weapon
		foreach(Weapon w in weapons)
		{
			if(w.ID == FindObjectOfType<LootBox>().data.party[playerSelect].weaponID)
			{
				players[x].transform.GetChild(2).GetComponent<Image>().sprite = w.Icon; //special
				break;
			}
			else
			{
				//use empty texture
				players[x].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Cancel");
			}
		}

		players[x].transform.GetChild(3).GetComponent<Image>().sprite = null; //movement
		players[x].transform.GetChild(4).GetComponent<Image>().sprite = null; //melee
	}
}