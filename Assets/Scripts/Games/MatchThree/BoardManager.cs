using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	GameObject[,] tiles = new GameObject[8, 12];
	GameObject grabbed;
	Vector2 grabbedPoint;

	void Start()
	{
		InitTiles();
	}

	private void Update()
	{
		if(CanMove())
		{
			//Check horizontal Matches
			CheckMatch(tiles.GetLength(0), tiles.GetLength(1), false);

			//Check vertical Matches
			CheckMatch(tiles.GetLength(1), tiles.GetLength(0), true);

			//Check player move
			CheckInput();
		}
		else
		{
			FloatTiles();
			RefilTiles();
		}
	}

	void FloatTiles()
	{
		for(int y = 0; y < tiles.GetLength(1); y++)
		{
			for(int x = 0; x < tiles.GetLength(0); x++)
			{
				if(GetTile(x, y))
				{
					if(GetTile(x, y).destroyed)
					{
						if(GetTile(x, y + 1))
						{
							if(!GetTile(x, y + 1).destroyed)
							{
								grabbed = GetTile(x, y).gameObject;
								MoveTile(Vector2.up);
							}
						}
					}
				}
			}
		}
	}

	void RefilTiles()
	{
		for(int y = 0; y < tiles.GetLength(1); y++)
		{
			for(int x = 0; x < tiles.GetLength(0); x++)
			{
				if(GetTile(x, y))
				{
					if(GetTile(x, y).destroyed)
					{
						if(y == 11)
						{
							GetTile(x, y).gameObject.transform.localPosition = new Vector2(x, y + 1);
							GetTile(x, y).type = (Tile.TileType)Random.Range(0, 6);
							GetTile(x, y).destroyed = false;
						}
					}
				}
			}
		}
	}

	bool CanMove()
	{
		int total = tiles.GetLength(0) * tiles.GetLength(1);
		int count = 0;

		for(int x = 0; x < tiles.GetLength(0); x++)
		{
			for(int y = 0; y < tiles.GetLength(1); y++)
			{
				if(tiles[x, y].GetComponent<Tile>().active)
				{
					count++;
				}
			}
		}

		if(count == total)
		{
			return true;
		}

		return false;
	}

	void CheckInput()
	{
		for(int x = 0; x < tiles.GetLength(0); x++)
		{
			for(int y = 0; y < tiles.GetLength(1); y++)
			{
				if(tiles[x, y].GetComponent<Tile>().Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
				{
					if(Input.GetMouseButtonDown(0))
					{
						grabbed = tiles[x, y];
						grabbedPoint = Input.mousePosition;
					}
				}
			}
		}

		if(grabbed != null)
		{
			if(Input.GetMouseButtonUp(0))
			{
				//direction we moved the mouse
				Vector2 dir = (Vector2)Input.mousePosition - grabbedPoint;

				//check specified direction then move tile that way
				switch(GetDirection(dir))
				{
					case "Left":
						MoveTile(Vector2.left);
						break;

					case "Right":
						MoveTile(Vector2.right);
						break;

					case "Up":
						MoveTile(Vector2.up);
						break;

					case "Down":
						MoveTile(Vector2.down);
						break;
				}

				grabbed = null;
			}
		}
	}

	void CheckMatch(int xCheck, int yCheck, bool axisSwap)
	{
		for(int y = 0; y < yCheck; y++)
		{
			int type = -1;
			int count = 0;

			for(int x = 0; x < xCheck; x++)
			{
				//if first tile begin match count
				if(type == -1)
				{
					type = (int)GetTile(x, y, axisSwap).type;
					count = 1;
					continue;
				}

				// if type match then add
				if((int)GetTile(x, y, axisSwap).type == type)
				{
					count++;
				}

				//Match Check Precondiditions:
				//if not the same
				//if at the end of the horizontal

				//Then Reset
				if((int)GetTile(x, y, axisSwap).type != type || xCheck - 1 == x)
				{
					//Check match
					if(count >= 3)
					{
						//iterate backward and 'destroy' the tile
						for(int c = 0; c < count; c++)
						{
							if((int)GetTile(x, y, axisSwap).type != type)
							{
								GetTile(x - c - 1, y, axisSwap).destroyed = true;
							}

							else if(xCheck - 1 == x)
							{
								GetTile(x - c, y, axisSwap).destroyed = true;
							}
						}

						//Deal damage
						int damage = count - 3 + 1;

						GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdDealDamage(damage);
					}

					//reset
					type = -1;
					count = 0;
					x--;
					continue;
				}
			}
		}
	}

	Tile GetTile(int x, int y)
	{
		if(y <= tiles.GetLength(1) - 1)
		{
			return GetTile(x, y, false);
		}

		return null;
	}

	Tile GetTile(int x, int y, bool swap)
	{
		if(swap)
		{
			return tiles[y, x].GetComponent<Tile>();
		}
		else
		{
			return tiles[x, y].GetComponent<Tile>();
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
		for (int y = 0; y < tiles.GetLength(1); y++)
		{
			for (int x = 0; x < tiles.GetLength(0); x++)
			{
				GameObject obj = new GameObject();
				obj.transform.SetParent(transform);
				obj.AddComponent<Tile>();
				obj.GetComponent<Tile>().type = (Tile.TileType)Random.Range(0, 6);
				obj.GetComponent<Tile>().pos = new Vector2(x, y);
				obj.transform.position = new Vector3(tiles.GetLength(0) / 2 - 0.5f, tiles.GetLength(1) / 2 - 0.5f, 0);
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