using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager1 : MonoBehaviour
{
	public GameObject[,] tiles = new GameObject[8, 12];

	void Start()
	{
		InitTiles();
	}

	private void Update()
	{
		CheckInput();
	}

	void CheckInput()
	{
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				print(x + "," + y);
				if (tiles[x, y].GetComponent<Tile>().Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
				{
					print("yes");
				}
			}
		}
	}

	void InitTiles()
	{
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				GameObject obj = new GameObject();
				obj.AddComponent<Tile>();
				obj.GetComponent<Tile>().type = (Tile.TileType)Random.Range(0, 6);
				obj.GetComponent<Tile>().pos = new Vector2(x, y);
			}
		}
	}
}

public class Tile : MonoBehaviour
{
	public enum TileType
	{
		Green,
		Blue,
		Orange,
		Yellow,
		Red,
		Purple
	}

	public TileType type;

	public bool active;
	public bool destroyed;

	public Vector2 pos;

	private void Start()
	{
		gameObject.AddComponent<SpriteRenderer>().sprite = GetSprite(type.ToString());
	}

	private void Update()
	{
		if (pos != (Vector2)transform.position)
		{
			transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime);
		}
	}

	public Sprite GetSprite(string spr)
	{
		Sprite[] sprites = Resources.LoadAll<Sprite>("Gems");

		for (int x = 0; x < sprites.Length; x++)
		{
			if (sprites[x].name == spr)
			{
				return sprites[x];
			}
		}

		return null;
	}

	public bool Contains(Vector2 pos)
	{
		//left
		if (pos.x < transform.position.x-0.5f)
		{
			return false;
		}

		//right
		if (pos.x > transform.position.x+0.5f)
		{
			return false;
		}

		//up
		if (pos.y < transform.position.y-0.5f)
		{
			return false;
		}

		//down
		if (pos.y > transform.position.y+0.5f)
		{
			return false;
		}

		return true;
	}
}