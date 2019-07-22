using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void SaveData(DataContainer box)
	{
		string path = Application.dataPath + "/Rice.Data";
		BinaryFormatter formatter = new BinaryFormatter();

		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, box);
		stream.Close();
	}

	public static DataContainer loadData()
	{
		string path = Application.dataPath + "/Rice.Data";

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
			return null;
		}
	}
}