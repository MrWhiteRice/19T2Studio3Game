using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager1 : MonoBehaviour
{
	public GameObject[,] tiles = new GameObject[8, 12];
	public GameObject grabbed;
	Vector2 grabbedPoint;

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
                if (tiles[x, y].GetComponent<Tile>().Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
					if (Input.GetMouseButtonDown(0))
					{
						grabbed = tiles[x, y];
						grabbedPoint = Input.mousePosition;
					}
				}
			}
		}

		if (grabbed != null)
		{
			if (Input.GetMouseButtonUp(0))
			{
				Vector2 dir = (Vector2)Input.mousePosition - grabbedPoint;

				switch (GetDirection(dir))
				{
					case "Left":
						print("Left");
						MoveTile(Vector2.left);
						break;

					case "Right":
						print("right");
						MoveTile(Vector2.right);
						break;

					case "Up":
						print("up");
						MoveTile(Vector2.up);
						break;

					case "Down":
						print("down");
						MoveTile(Vector2.down);
						break;
				}

				grabbed = null;
			}
		}
	}

	public void MoveTile(Vector2 dir)
	{
		//TODO: check out of bounds
		Vector2 grabbedPos = grabbed.GetComponent<Tile>().pos;

		GameObject oldTile = tiles[(int)grabbedPos.x + (int)dir.x, (int)grabbedPos.y + (int)dir.y];
		Vector2 newPos = oldTile.GetComponent<Tile>().pos;

		grabbed.GetComponent<Tile>().pos = newPos;
		oldTile.GetComponent<Tile>().pos = grabbedPos;

		tiles[(int)grabbedPos.x, (int)grabbedPos.y] = oldTile;
		tiles[(int)grabbedPos.x + (int)dir.x, (int)grabbedPos.y + (int)dir.y] = grabbed;

		grabbed = null;
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
				obj.name = obj.GetComponent<Tile>().type.ToString();
				tiles[x, y] = obj;
			}
		}
	}

	string GetDirection(Vector2 dir)
	{
		if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
		{
			if (dir.x > 0.5f)
			{
				return "Right";
			}
			else if (dir.x < -0.5f)
			{
				return "Left";
			}
		}
		else
		{
			if (dir.y > 0.5f)
			{
				return "Up";
			}
			else if (dir.y < -0.5f)
			{
				return "Down";
			}
		}

		return "";
	}
}