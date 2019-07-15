using UnityEngine;

public class Actor : ScriptableObject
{
	int id;

	Sprite icon;
	string characterName;
	WeightClass weight;
	int rarity;
	int initiative;

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

	public string CharacterName
	{
		get { return characterName; }
		set { characterName = value; }
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

	public int Initiative
	{
		get { return initiative; }
		set { initiative = value; }
	}

	public static Actor FindActor(string actorName)
	{
		Object[] loadedAssets = Resources.LoadAll("RiceStuff/Actors/");

		for(int x = 0; x < loadedAssets.Length; x++)
		{
			Actor actor = (Actor)loadedAssets[x];
			
			if(actor.CharacterName == actorName)
			{
				return (Actor)loadedAssets[x];
			}
		}

		return null;
	}
}