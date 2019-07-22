using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootBox : MonoBehaviour
{
	Weapon[] weps = new Weapon[0];
	Actor[] chars = new Actor[0];

	public Text actor;
	public Text weapon;

	public DataContainer data;

    void Start()
    {
		data = SaveSystem.loadData();

		weps = Resources.LoadAll<Weapon>("RiceStuff/Weapons/");
		chars = Resources.LoadAll<Actor>("RiceStuff/Actors/");
	}

	private void Update()
	{
		weapon.text = "";

		foreach(Weapon w in weps)
		{
			WeaponData wd = data.FindWeapon(w.WeaponName);

			if(wd.unlocked)
			{
				weapon.text += "<color=green>[Y] ";
			}
			else
			{
				weapon.text += "<color=red>[N] ";
			}

			weapon.text += wd.weaponName + "\n</color>";
		}

		actor.text = "";

		foreach(Actor w in chars)
		{
			CharacterData wd = data.FindCharacter(w.CharacterName);

			if(wd.unlocked)
			{
				actor.text += "<color=green>[Y] ";
			}
			else
			{
				actor.text += "<color=red>[N] ";
			}

			actor.text += wd.characterName + "\n</color>";
		}
	}

	public void Roll()
	{
		int roll = Random.Range(1, 101);

		data.free++;

		//1 Star weapon
		if(roll > 30 && roll <= 100)
		{
			List<Weapon> oneStar = new List<Weapon>();

			foreach(Weapon a in weps)
			{
				if(a.Rarity == 1)
				{
					oneStar.Add(a);
				}
			}

			int rand = Random.Range(0, oneStar.Count);

			WeaponData wd = data.FindWeapon(weps[rand].WeaponName);

			if(wd.unlocked != true)
			{
				wd.unlocked = true;
			}
			else
			{
				print("refund 1 star");
			}

			print("[1]" + oneStar[rand].WeaponName);
		}

		//2 Star weapon
		if(roll > 5 && roll <= 25)
		{
			List<Weapon> twoStar = new List<Weapon>();

			foreach(Weapon a in weps)
			{
				if(a.Rarity == 2)
				{
					twoStar.Add(a);
				}
			}

			int rand = Random.Range(0, twoStar.Count);

			WeaponData wd = data.FindWeapon(weps[rand].WeaponName);

			if(wd.unlocked != true)
			{
				wd.unlocked = true;
			}
			else
			{
				print("refund 2 star");
			}

			print("[2]" + twoStar[rand].WeaponName);
		}

		//3 star weapon
		if(roll > 1 && roll <= 4)
		{
			List<Weapon> threeStar = new List<Weapon>();

			foreach(Weapon a in weps)
			{
				if(a.Rarity == 3)
				{
					threeStar.Add(a);
				}
			}

			int rand = Random.Range(0, threeStar.Count);

			WeaponData wd = data.FindWeapon(weps[rand].WeaponName);

			if(wd.unlocked != true)
			{
				wd.unlocked = true;
			}
			else
			{
				print("refund 3 star");
			}

			print("[3]" + threeStar[rand].WeaponName);
		}

		//character
		if(roll == 1)
		{
			int rand = Random.Range(0, chars.Length);

			CharacterData cd = data.FindCharacter(chars[rand].CharacterName);

			if(cd.unlocked != true)
			{
				cd.unlocked = true;
			}
			else
			{
				print("refund character credits!");
			}

			print("[C]" + chars[rand].CharacterName);
		}

		SaveSystem.SaveData(data);
	}
}