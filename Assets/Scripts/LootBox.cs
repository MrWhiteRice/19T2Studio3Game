using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
	Weapon[] weps = new Weapon[0];
	Actor[] chars = new Actor[0];

	public DataContainer data;

	//              char, 1*, 2*, 3*
	//int[] weights = { 1, 4, 25, 70};
	public float[] normalWeights = { 1, 4, 25, 70};
	public float[] premiumWeights = { 0, 0, 0, 0};
	public float[] eventWeights = { 0, 0, 0, 0};

    void Start()
    {
		PlayerPrefs.DeleteKey("ActivePlayer");

		weps = Resources.LoadAll<Weapon>("RiceStuff/Weapons/");
		chars = Resources.LoadAll<Actor>("RiceStuff/Actors/");
	}

	public void LoadData(int player)
	{
		//set player
		PlayerPrefs.SetInt("ActivePlayer", player);
		//load data
		data = SaveSystem.loadData(player);
		//set created true
		data.created = true;

		SaveSystem.SaveData(data);
	}

	public void Save()
	{
		SaveSystem.SaveData(data);
	}

	private void OnDisable()
	{
		Save();
	}

	private void OnApplicationQuit()
	{
		Save();
	}

	public void MultiRoll(string weightSet)
	{
		//roll 9 weapons
		for(int x = 0; x < 9; x++)
		{
			Roll(weightSet);
		}
	
		//guaranteed character roll
		CharacterRoll();
	}

	public void Roll(string weightSet)
	{
		//initialise weights
		float[] weights = new float[0];

		//find which weight to use
		switch(weightSet)
		{
			case "Normal":
				weights = normalWeights;
				break;

			case "Premium":
				weights = premiumWeights;
				break;

			case "Event":
				weights = eventWeights;
				break;

			default:
				Debug.LogError("RICE_ERROR: No weight set!");
				break;
		}

		//update save file to register actually played
		data.created = true;

		//roll dice to see what item you get
		float roll = Random.Range(0f, 100.0f);

		//initialise weight roll
		float count = weights[0];
		for(int x = 0; x <= weights.Length; x++)
		{
			if(roll <= count)
			{
				if(x == 0) //check char roll
				{
					CharacterRoll();
				}
				else //weapon roll
				{
					ItemRoll(x);
				}

				break;
			}
			else
			{
				//add to weight roll and check the next tier
				count += weights[x+1];
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
		//print("rolling " + rarity + " star item!");
		List<Weapon> star = new List<Weapon>();

		foreach(Weapon a in weps)
		{
			if(a.Rarity == rarity)
			{
				star.Add(a);
			}
		}

		//select an weapon in the star we rolled
		int rand = Random.Range(0, star.Count);

		//print(rand + "," + star.Count);
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