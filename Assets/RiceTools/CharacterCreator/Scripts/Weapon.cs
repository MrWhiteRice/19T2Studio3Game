using UnityEngine;

public class Weapon : ScriptableObject
{
	[SerializeField]public int id;

	[SerializeField] Sprite icon;
	[SerializeField] string weaponName;
	[SerializeField] WeightClass weight;
	[SerializeField] int rarity;
	[SerializeField] int damage;
	[SerializeField] int shots;
	[SerializeField] int accuracy;
	[SerializeField] int knockback;
	[SerializeField] bool traversal;
	[SerializeField] bool special;
	[SerializeField] bool usesTurn;
	[SerializeField] bool passive;

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

	public string WeaponName
	{
		get { return weaponName; }
		set { weaponName = value; }
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

	public int Damage
	{
		get { return damage; }
		set { damage = value; }
	}

	public int Shots
	{
		get { return shots; }
		set { shots = value; }
	}

	public int Accuracy
	{
		get { return accuracy; }
		set { accuracy = value; }
	}

	public int Knockback
	{
		get { return knockback; }
		set { knockback = value; }
	}

	public bool Traversal
	{
		get { return traversal; }
		set { traversal = value; }
	}

	public bool Special
	{
		get { return special; }
		set { special = value; }
	}

	public bool UsesTurn
	{
		get { return usesTurn; }
		set { usesTurn = value; }
	}

	public bool Passive
	{
		get { return passive; }
		set { passive = value; }
	}

	public static Weapon FindActor(string actorName)
	{
		Object[] loadedAssets = Resources.LoadAll("RiceStuff/Weapons/");

		for(int x = 0; x < loadedAssets.Length; x++)
		{
			Weapon actor = (Weapon)loadedAssets[x];
			
			if(actor.WeaponName == actorName)
			{
				return (Weapon)loadedAssets[x];
			}
		}

		return null;
	}
}