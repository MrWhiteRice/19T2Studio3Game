using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
	Weapon[] weps = new Weapon[0];
	Actor[] chars = new Actor[0];

	public DataContainer data;

	int[] weights = { 1, 4, 25, 70};

    void Start()
    {
		//data = SaveSystem.loadData();

		weps = Resources.LoadAll<Weapon>("RiceStuff/Weapons/");
		chars = Resources.LoadAll<Actor>("RiceStuff/Actors/");
	}

	public void LoadData(int player)
	{
		PlayerPrefs.SetInt("ActivePlay", player);
		data = SaveSystem.loadData(player);
	}

	public void Save()
	{
		SaveSystem.SaveData(data);
	}

	private void OnDisable()
	{
		Save();
	}

	public void MultiRoll()
	{
		CharacterRoll();

		for(int x = 0; x < 9; x++)
		{
			Roll();
		}
	}

	public void Roll()
	{
		int roll = Random.Range(1, 101);
		data.free++;

		int count = weights[0];
		for(int x = 1; x <= weights.Length; x++)
		{
			if(roll <= count)
			{
				//check char roll
				if(x == 1)
				{
					CharacterRoll();
				}
				//weapon roll
				else
				{
					ItemRoll(x);
				}

				return;
			}
			else
			{
				count += weights[x];
			}
		}

		Save();
	}

	void CharacterRoll()
	{
		int rand = Random.Range(0, chars.Length);

		CharacterData cd = data.FindCharacter(chars[rand].ID);

		if(cd.unlocked != true)
		{
			cd.unlocked = true;
			GenerateNotification(chars[rand].Icon, chars[rand].CharacterName);
			//print("[C]" + chars[rand].CharacterName);
		}
		else
		{
			GenerateRefund(chars[rand].Icon, chars[rand].CharacterName);
			//print("[C]" + chars[rand].CharacterName + "refund character credits!");
		}

		Save();
	}

	void ItemRoll(int rarity)
	{
		List<Weapon> star = new List<Weapon>();

		foreach(Weapon a in weps)
		{
			if(a.Rarity == rarity)
			{
				star.Add(a);
			}
		}

		int rand = Random.Range(0, star.Count);

		WeaponData wd = data.FindWeapon(star[rand].ID);

		if(wd.unlocked != true)
		{
			wd.unlocked = true;
			GenerateNotification(star[rand].Icon, star[rand].WeaponName);
			//print("[" + rarity + "]" + wd.weaponName);
		}
		else
		{
			GenerateRefund(star[rand].Icon, star[rand].WeaponName);
			//print("[" + rarity + "]" + wd.weaponName + "refund " + rarity + " star");
		}
	}

	void GenerateNotification(Sprite icon, string text)
	{
		GameObject noti = (GameObject)GameObject.Instantiate(Resources.Load("Notification"), transform);
		noti.GetComponent<NotificationData>().Init(icon, text);
	}

	void GenerateRefund(Sprite icon, string text)
	{
		GameObject noti = (GameObject)GameObject.Instantiate(Resources.Load("Notification_Refund"), transform);
		noti.GetComponent<NotificationData>().Init(icon, text);
	}
}