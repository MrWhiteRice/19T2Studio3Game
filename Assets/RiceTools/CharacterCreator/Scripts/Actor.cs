using UnityEngine;

public class Actor : ScriptableObject
{
	int id;

	Sprite icon;
	new string name;
	WeightClass weight;
	int rarity;

	public enum WeightClass
	{
		Light,
		Medium,
		Heavy
	}

	public int ID
	{
		get { return id; }
		set { id = value; }
	}

	public Sprite Icon
	{
		get { return icon; }
		set { icon = value; }
	}

	public string Name
	{
		get { return name; }
		set { name = value; }
	}

	public WeightClass Weight
	{
		get { return weight; }
		set { weight = value; }
	}

	public int Rarity
	{
		get { return rarity; }
		set { rarity = value; }
	}

	public static Actor FindActor(string actorName)
	{
		Object[] loadedAssets = Resources.LoadAll("Actors/");

		for(int x = 0; x < loadedAssets.Length; x++)
		{
			Actor actor = (Actor)loadedAssets[x];

			if(actor.name == actorName)
			{
				Debug.Log("found");
				return (Actor)loadedAssets[x];
			}
		}

		Debug.Log("no one ofund" + actorName);
		return null;
	}
}