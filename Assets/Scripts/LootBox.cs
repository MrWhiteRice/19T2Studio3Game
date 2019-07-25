using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootBox : MonoBehaviour
{
	Weapon[] weps = new Weapon[0];
	Actor[] chars = new Actor[0];

	//public Text actor;
	//public Text weapon;

	public DataContainer data;

    void Start()
    {
		data = SaveSystem.loadData();

		weps = Resources.LoadAll<Weapon>("RiceStuff/Weapons/");
		chars = Resources.LoadAll<Actor>("RiceStuff/Actors/");
	}

	private void Update()
	{
		//weapon.text = "";

		//foreach(Weapon w in weps)
		//{
		//	WeaponData wd = data.FindWeapon(w.ID);

		//	if(wd.unlocked)
		//	{
		//		weapon.text += "<color=green>[Y] ";
		//	}
		//	else
		//	{
		//		weapon.text += "<color=red>[N] ";
		//	}

		//	weapon.text += wd.weaponName + "\n</color>";
		//}

		//actor.text = "";

		//foreach(Actor w in chars)
		//{
		//	CharacterData wd = data.FindCharacter(w.ID);

		//	if(wd.unlocked)
		//	{
		//		actor.text += "<color=green>[Y] ";
		//	}
		//	else
		//	{
		//		actor.text += "<color=red>[N] ";
		//	}

		//	actor.text += wd.characterName + "\n</color>";
		//}
	}

	public void MultiRoll()
	{
		CharacterRoll();
		for(int x = 0; x < 9; x++)
		{
			Roll();
		}
	}

	void CharacterRoll()
	{
		int rand = Random.Range(0, chars.Length);

		CharacterData cd = data.FindCharacter(chars[rand].ID);

		if(cd.unlocked != true)
		{
			cd.unlocked = true;
			print("[C]" + chars[rand].CharacterName);
		}
		else
		{
			print("[C]" + chars[rand].CharacterName + "refund character credits!");
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

			WeaponData wd = data.FindWeapon(oneStar[rand].ID);

			if(wd.unlocked != true)
			{
				wd.unlocked = true;
				print("[1]" + wd.weaponName);
				GenerateNotification(oneStar[rand].Icon, oneStar[rand].WeaponName);
			}
			else
			{
				print("[1]" + wd.weaponName + "refund 1 star");
			}
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

			WeaponData wd = data.FindWeapon(twoStar[rand].ID);

			if(wd.unlocked != true)
			{
				wd.unlocked = true;
				print("[2]" + wd.weaponName);
				GenerateNotification(twoStar[rand].Icon, twoStar[rand].WeaponName);
			}
			else
			{
				print("[2]" + wd.weaponName + "refund 2 star");
			}

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

			WeaponData wd = data.FindWeapon(threeStar[rand].ID);

			if(wd.unlocked != true)
			{
				wd.unlocked = true;
				GenerateNotification(threeStar[rand].Icon, threeStar[rand].WeaponName);
				print("[3]" + wd.weaponName);
			}
			else
			{
				print("[3]" + wd.weaponName + "refund 3 star");
			}

		}

		//character
		if(roll == 1)
		{
			int rand = Random.Range(0, chars.Length);

			CharacterData cd = data.FindCharacter(chars[rand].ID);

			if(cd.unlocked != true)
			{
				cd.unlocked = true;
				GenerateNotification(chars[rand].Icon, chars[rand].CharacterName);
				print("[C]" + chars[rand].CharacterName);
			}
			else
			{
				print("[C]" + chars[rand].CharacterName + "refund character credits!");
			}
		}

		SaveSystem.SaveData(data);
	}

	public void GenerateNotification(Sprite icon, string text)
	{
		GameObject noti = (GameObject)GameObject.Instantiate(Resources.Load("Notification"), transform.parent);
		noti.GetComponent<NotificationData>().Init(icon, text);
	}

	public void GenerateRefund(Sprite icon, string text)
	{
		GameObject noti = (GameObject)GameObject.Instantiate(Resources.Load("Notification_Refund"), transform.parent);
		noti.GetComponent<NotificationData>().Init(icon, text);
	}
}