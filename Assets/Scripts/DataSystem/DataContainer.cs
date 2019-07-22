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

	//selected party
	public CharacterParty[] party = new CharacterParty[3];

	////unlocked characters
	//public bool jesseN;     //added
	//public bool jacob;      //added
	//public bool william;    //added
	//public bool jeffry;     //added
	//public bool jesseS;
	//public bool louis;
	//public bool jack;
	//public bool thomasC;    //added
	//public bool thomasL;
	//public bool brandon;
	//public bool lorenzo;
	//public bool steven;

	////unlocked weapons
	//public bool ropehookL;
	//public bool punchM3;
	//public bool grapplinghookL;
	//public bool minigunH1;
	//public bool punchL;
	//public bool punchM;
	//public bool punchH;
	//public bool punchM2;
	//public bool pistolL;
	//public bool pistolL1;
	//public bool burstrifleM3;
	//public bool punchL3;
	//public bool rifleM;
	//public bool burstrifleL2;
	//public bool ropehookM;
	//public bool minigunH3;
	//public bool mac10L2;
	//public bool ropehookH;
	//public bool punchH2;
	//public bool lmgH;

	public List<CharacterData> unlockedCharacters = new List<CharacterData>();
	public List<WeaponData> unlockedWeapons = new List<WeaponData>();

	public CharacterData FindCharacter(string name)
	{
		foreach(CharacterData cd in unlockedCharacters)
		{
			if(cd.characterName == name)
			{
				return cd;
			}
		}

		return null;
	}

	public WeaponData FindWeapon(string name)
	{
		foreach(WeaponData wd in unlockedWeapons)
		{
			if(wd.weaponName == name)
			{
				return wd;
			}
		}

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
}