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
		
		//null text
		partyDetails.text = "";

		//Name
		partyDetails.text += player.playerID != -1 ? "Name: " + FindObjectOfType<LootBox>().data.FindCharacter(player.playerID).characterName + "\n" : "Name: Empty\n";

		//class weapon
		partyDetails.text += player.classID	!= -1 ? "Class: " + FindObjectOfType<LootBox>().data.FindWeapon(player.classID).weaponName + "\n" : "Class: Empty\n";

		//special weapon
		partyDetails.text += player.weaponID != -1 ? "Special: " + FindObjectOfType<LootBox>().data.FindWeapon(player.weaponID).weaponName + "\n" : "Special: Empty\n";

		//Traversal - Update
		//partyDetails.text += "Traversal: " + FindObjectOfType<LootBox>().data.FindWeapon(player.traversalID).weaponName;

		//find correct player
		Actor a = null;
		foreach(Actor actor in Resources.LoadAll<Actor>("RiceStuff/Actors"))
		{
			if(actor.id == player.playerID)
			{
				a = actor;
			}
		}

		//find correct weapon
		Weapon classWeapon = null;
		Weapon specialWeapon = null;
		foreach(Weapon weapon in Resources.LoadAll<Weapon>("RiceStuff/Weapons"))
		{
			if(weapon.ID == player.classID)
			{
				classWeapon = weapon;
			}
			else if(weapon.ID == player.weaponID)
			{
				specialWeapon = weapon;
			}
		}

		//set select player sprite
		buttons[0].GetComponent<Image>().sprite = a != null ? a.Icon : Resources.Load<Sprite>("Cancel");

		//set select weapon sprite
		buttons[1].GetComponent<Image>().sprite = classWeapon != null ? classWeapon.Icon : Resources.Load<Sprite>("Cancel");

		//set select special weapon sprite
		buttons[2].GetComponent<Image>().sprite = specialWeapon != null ? specialWeapon.Icon : Resources.Load<Sprite>("Cancel");
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