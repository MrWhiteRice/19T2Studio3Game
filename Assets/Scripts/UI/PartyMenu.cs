using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMenu : MonoBehaviour
{
	public Text partyDetails;
	int memberSelected = 0;

	public Button[] buttons;
	CharacterParty player;

	void Update()
	{
		player = FindObjectOfType<LootBox>().data.party[memberSelected];
		
		//name
		partyDetails.text = "";
		partyDetails.text += "Name: " + FindObjectOfType<LootBox>().data.FindCharacter(player.playerID).characterName + "\n";

		//class weapon
		if(player.classID != -1)
		{
			partyDetails.text += "Class: " + FindObjectOfType<LootBox>().data.FindWeapon(player.classID).weaponName + "\n";
		}
		else
		{
			partyDetails.text += "Class: Empty\n";
		}

		//special weapon
		if(player.weaponID != -1)
		{
			partyDetails.text += "Special: " + FindObjectOfType<LootBox>().data.FindWeapon(player.weaponID).weaponName + "\n";
		}
		else
		{
			partyDetails.text += "Special: Empty\n";
		}
		//partyDetails.text += "Traversal: " + FindObjectOfType<LootBox>().data.FindWeapon(player.traversalID).weaponName;

		Actor a = null;
		foreach(Actor actor in Resources.LoadAll<Actor>("RiceStuff/Actors"))
		{
			if(actor.id == player.playerID)
			{
				a = actor;
			}
		}

		buttons[0].GetComponent<Image>().sprite = a.Icon;
	}

	public void ChangeMember(int id)
	{
		memberSelected = id;

		FindObjectOfType<LootBox>().Save();
	}

	public void PartySelector(int type)
	{
		GameObject p = (GameObject)Instantiate(Resources.Load("PartySelector"), FindObjectOfType<MenuController>().menu[3].transform);

		p.GetComponent<PartySelector>().type = (Selector)type;

		p.GetComponent<PartySelector>().selected = memberSelected;
		p.GetComponent<PartySelector>().classID = player.classID;
		p.GetComponent<PartySelector>().traversalID = player.traversalID;
		p.GetComponent<PartySelector>().weaponID = player.weaponID;
	}

	public enum Selector
	{
		Character = 0,
		Class = 1,
		Special = 2,
		Traversal = 3
	}
}