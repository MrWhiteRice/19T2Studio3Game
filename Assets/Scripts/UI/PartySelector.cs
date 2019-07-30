using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySelector : MonoBehaviour
{
	public Transform panel;

	public PartyMenu.Selector type;

	public int selected;
	public int classID;
	public int traversalID;
	public int weaponID;

	private void Start()
	{
		if(type == 0)
		{
			LoadActors();
		}
		else
		{
			LoadWeapons();
		}
	}

	void LoadActors()
	{
		GameObject button = (GameObject)Resources.Load("PlayerButton");

		Actor[] list = Resources.LoadAll<Actor>("RiceStuff/Actors");

		for(int x = 0; x < FindObjectOfType<LootBox>().data.unlockedCharacters.Count; x++)
		{
			if(FindObjectOfType<LootBox>().data.unlockedCharacters[x].unlocked)
			{
				GameObject b = Instantiate(button, panel);
				b.GetComponent<PlayerButtonData>().id = FindObjectOfType<LootBox>().data.unlockedCharacters[x].ID;
				b.GetComponent<Button>().onClick.AddListener(b.GetComponent<PlayerButtonData>().Click);

				foreach(Actor a in list)
				{
					if(a.ID == FindObjectOfType<LootBox>().data.unlockedCharacters[x].ID)
					{
						b.GetComponent<Image>().sprite = a.Icon;
					}
				}
			}
		}
	}

	void LoadWeapons()
	{
		GameObject button = (GameObject)Resources.Load("PlayerButton");

		Weapon[] list = Resources.LoadAll<Weapon>("RiceStuff/Weapons");

		//cycle through all weapons
		for(int x = 0; x < FindObjectOfType<LootBox>().data.unlockedWeapons.Count; x++)
		{
			//checked if theyre unlocked
			if(FindObjectOfType<LootBox>().data.unlockedWeapons[x].unlocked)
			{
				foreach(Weapon a in list)
				{
					//check correct weapon
					if(a.ID == FindObjectOfType<LootBox>().data.unlockedWeapons[x].ID)
					{
						GameObject b = null;

						//check traversal weapon
						if(a.Traversal && type == PartyMenu.Selector.Traversal)
						{
							b = Instantiate(button, panel);
						}
						else if(a.Special && type == PartyMenu.Selector.Special)
						{
							b = Instantiate(button, panel);
						}
						else if(!a.Traversal && !a.Special && type == PartyMenu.Selector.Class && !a.Melee)
						{
							b = Instantiate(button, panel);
						}

						if(b != null)
						{
							b.GetComponent<PlayerButtonData>().id = FindObjectOfType<LootBox>().data.unlockedWeapons[x].ID;
							b.GetComponent<Button>().onClick.AddListener(b.GetComponent<PlayerButtonData>().ClickWep);

							b.GetComponent<Image>().sprite = a.Icon;
						}
					}
				}
			}
		}
	}

	public void SelectCharacter(int id)
	{
		FindObjectOfType<LootBox>().data.party[selected].playerID = id;
		print("selected: " + id);
		Destroy(gameObject);
	}

	public void SelectWeapon(int id)
	{
		switch(type)
		{
			case PartyMenu.Selector.Class:
				FindObjectOfType<LootBox>().data.party[selected].classID = id;
				break;

			case PartyMenu.Selector.Special:
				FindObjectOfType<LootBox>().data.party[selected].weaponID = id;
				break;

			case PartyMenu.Selector.Traversal:
				FindObjectOfType<LootBox>().data.party[selected].traversalID = id;
				break;
		}

		print("selected: " + id);
		Destroy(gameObject);
	}
}