using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void SaveData(DataContainer box)
	{
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
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			DataContainer data = (DataContainer)formatter.Deserialize(stream);
			stream.Close();

			return data;
		}
		else
		{
			DataContainer dataContainer = new DataContainer();

			foreach(Weapon wep in Resources.LoadAll<Weapon>("RiceStuff/Weapons/"))
			{
				bool locked = false;

				if(wep.WeaponName == "" || wep.WeaponName == "" || wep.WeaponName == "")
				{

				}

				dataContainer.unlockedWeapons.Add(new WeaponData(wep.WeaponName, locked, wep.ID));
			}

			foreach(Actor cha in Resources.LoadAll<Actor>("RiceStuff/Actors/"))
			{
				dataContainer.unlockedCharacters.Add(new CharacterData(cha.CharacterName, false, cha.ID));
			}

			return dataContainer;
		}
	}
}