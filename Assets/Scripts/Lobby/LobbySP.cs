using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySP : MonoBehaviour
{
	public GameObject[] players;
	public bool noGo;

    void Update()
    {
		noGo = false;

		Weapon[] weapons = Resources.LoadAll<Weapon>("RiceStuff/Weapons");
		Actor[] characters = Resources.LoadAll<Actor>("RiceStuff/Actors");

		for(int x = 0; x < players.Length; x++)
		{
			int playerSelect = x % 3;

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

			foreach(Weapon w in weapons)
			{
				if(w.ID == FindObjectOfType<LootBox>().data.party[playerSelect].weaponID)
				{
					players[x].transform.GetChild(2).GetComponent<Image>().sprite = w.Icon; //special
					break;
				}
				else
				{
					players[x].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Cancel");
				}
			}

			players[x].transform.GetChild(3).GetComponent<Image>().sprite = null; //movement
			players[x].transform.GetChild(4).GetComponent<Image>().sprite = null; //melee
		}
    }
}