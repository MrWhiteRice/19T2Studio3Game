using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void SaveData(DataContainer box)
	{
		if(PlayerPrefs.GetInt("ActivePlayer", -1) == -1)
		{
			return;
		}

		string path = Application.persistentDataPath + "/RiceData" + PlayerPrefs.GetInt("ActivePlayer") + ".Data";
		BinaryFormatter formatter = new BinaryFormatter();

		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, box);
		stream.Close();
	}

	public static DataContainer loadData(int player)
	{
		//version number to check if data needs to be updated
		int VERSION_NUMBER = 1;

		string path = Application.persistentDataPath + "/RiceData" + player + ".Data";
		//Debug.Log(Application.persistentDataPath);
		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			DataContainer data = (DataContainer)formatter.Deserialize(stream);
			stream.Close();

			//check for update
			if(data.version != VERSION_NUMBER)
			{
				Debug.Log("Version update! loading new information!");

				//Weapon Update
				foreach(Weapon wep in Resources.LoadAll<Weapon>("RiceStuff/Weapons/"))
				{
					bool check = true;

					//cycle all weapons we have and check if something is the same
					foreach(WeaponData loadedWep in data.unlockedWeapons)
					{
						//if its the same then dont add
						if(wep.WeaponName == loadedWep.weaponName)
						{
							check = false;
						}
					}

					//check if there is a mismatch, add
					if(check)
					{
						Debug.Log("adding item!" + wep.WeaponName);
						data.unlockedWeapons.Add(new WeaponData(wep.WeaponName, false, wep.ID));
					}
				}

				//Character Update
				foreach(Actor cha in Resources.LoadAll<Actor>("RiceStuff/Actors/"))
				{
					bool check = true;

					//cycle all characters we have and check if something is the same
					foreach(CharacterData loadedChar in data.unlockedCharacters)
					{
						//if its the same then dont add
						if(cha.CharacterName == loadedChar.characterName)
						{
							check = false;
						}
					}

					//check if there is a mismatch, add
					if(check)
					{
						Debug.Log("adding item!" + cha.CharacterName);
						data.unlockedCharacters.Add(new CharacterData(cha.CharacterName, false, cha.ID));
					}
				}

				data.version = VERSION_NUMBER;
			}

			return data;
		}
		else
		{
			DataContainer dataContainer = new DataContainer();

			foreach(Weapon wep in Resources.LoadAll<Weapon>("RiceStuff/Weapons/"))
			{
				bool locked = false;

				if(wep.Starter)
				{
					locked = true;
				}

				dataContainer.unlockedWeapons.Add(new WeaponData(wep.WeaponName, locked, wep.ID));
			}

			foreach(Actor cha in Resources.LoadAll<Actor>("RiceStuff/Actors/"))
			{
				bool locked = false;

				if(cha.CharacterName == "Jacob" || cha.CharacterName == "Jesse N" || cha.CharacterName == "William")
				{
					locked = true;
				}

				dataContainer.unlockedCharacters.Add(new CharacterData(cha.CharacterName, locked, cha.ID));
			}

			return dataContainer;
		}
	}
}