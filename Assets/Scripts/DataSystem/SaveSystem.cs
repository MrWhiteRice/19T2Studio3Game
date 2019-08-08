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
		string path = Application.persistentDataPath + "/RiceData" + player + ".Data";
		Debug.Log(Application.persistentDataPath);
		if(File.Exists(path))
		{
			Debug.Log("found: " + player);
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			DataContainer data = (DataContainer)formatter.Deserialize(stream);
			stream.Close();

			return data;
		}
		else
		{
			Debug.Log("no data found" + player);
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
				bool locked = true;

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