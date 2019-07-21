using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	static string path = Application.persistentDataPath + "/RiceData/Rice.Data";

	public static void SaveData(LootBox box)
	{
		BinaryFormatter formatter = new BinaryFormatter();

		FileStream stream = new FileStream(path, FileMode.Create);

		PlayerData data = new DataContainer(player);

		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static PlayerData loadPlayer()
	{
		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			PlayerData data = (PlayerData)formatter.Deserialize(stream);
			stream.Close();

			return data;
		}
		else
		{
			return null;
			//nothing found error!
		}
	}
}