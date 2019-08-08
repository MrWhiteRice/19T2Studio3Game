using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataContainer
{
	//stats
	public int free;
	public int paid;

	public int matchesWon;
	public int mathcesLost;

	public bool tutorialComplete;

	public bool created;

	//selected party
	public CharacterParty[] party = new CharacterParty[3] { new CharacterParty(46929, 1468, 1468, 1468), new CharacterParty(49880, 1468, 1468, 1468), new CharacterParty(25073, 1468, 1468, 1468) };

	//weapon and character data
	public List<CharacterData> unlockedCharacters = new List<CharacterData>();
	public List<WeaponData> unlockedWeapons = new List<WeaponData>();

	public CharacterData FindCharacter(int id)
	{
		foreach(CharacterData cd in unlockedCharacters)
		{
			if(cd.ID == id)
			{
				return cd;
			}
		}

		Debug.LogError("oit waadsasdasdasdasdhsd");
		return null;
	}

	public WeaponData FindWeapon(int id)
	{
		foreach(WeaponData wd in unlockedWeapons)
		{
			if(wd.ID == id)
			{
				return wd;
			}
		}

		Debug.LogError("Weapon Not Found: " + id);
		return null;
	}
}

[System.Serializable]
public class WeaponData
{
	public string weaponName;
	public bool unlocked;
	public int ID;

	public WeaponData(string Name, bool unlock, int id)
	{
		weaponName = Name;
		unlocked = unlock;
		ID = id;
	}
}

[System.Serializable]
public class CharacterData
{
	public string characterName;
	public bool unlocked;
	public int ID;

	public CharacterData(string Name, bool unlock, int id)
	{
		characterName = Name;
		unlocked = unlock;
		ID = id;
	}
}

[System.Serializable]
public class CharacterParty
{
	public int playerID;
	public int weaponID;
	public int classID;
	public int traversalID;

	public CharacterParty(int id, int wep, int clas, int trav)
	{
		playerID = id;
		weaponID = wep;
		classID = clas;
		traversalID = trav;
	}

	public CharacterParty(int id)
	{
		playerID = id;
	}
}