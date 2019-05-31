using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public GameObject[,] tile = new GameObject[8, 12];
	GameObject grabbed;

	Vector2 grabPos;

	void Start()
	{
		InitTiles();
	}

	private void Update()
	{
		DrawTiles();

		FillSpace();
		CheckMatch();

		CheckInput();
	}

	void FillSpace()
	{
		for(int x = 0; x < tile.GetLength(0); x++)
		{
			for(int y = tile.GetLength(1); y > -1; y--)
			{
				if(y == 12)
				{
					if (tile[x, y - 1].GetComponent<GameTile>().destroyed)
					{
						Vector2 pos = tile[x, y - 1].transform.position;
						if (pos == new Vector2(x, y-1))
						{
							int selected = Random.Range(0, 6);

							Color c = tile[x, y - 1].GetComponentInChildren<Renderer>().material.color;

							switch (selected)
							{
								case 0:
									c = Color.red;
									break;

								case 1:
									c = Color.green;
									break;

								case 2:
									c = Color.blue;
									break;

								case 3:
									c = Color.cyan;
									break;

								case 4:
									c = Color.yellow;
									break;

								case 5:
									c = Color.grey;
									break;
							}

							tile[x, y - 1].transform.position += Vector3.up;
							tile[x, y - 1].GetComponent<GameTile>().destroyed = false;
							tile[x, y - 1].GetComponent<GameTile>().type = selected;
							tile[x, y - 1].GetComponentInChildren<Renderer>().material.color = c;
							tile[x, y - 1].GetComponentInChildren<Renderer>().material.SetColor("_TINT", Color.white);
						}
					}

					continue;
				}

				if (y - 1 > 0)
				{
					if(tile[x, y - 1].GetComponent<GameTile>().destroyed)
					{
						GameObject current = tile[x, y];
						GameObject nullTile = tile[x, y - 1];

						Vector2 pos = current.GetComponent<GameTile>().pos;
						current.GetComponent<GameTile>().pos = nullTile.GetComponent<GameTile>().pos;
						nullTile.GetComponent<GameTile>().pos = pos;

						tile[x, y] = nullTile;
						tile[x, y - 1] = current;
					}
				}

				
			}
		}
	}

	void CheckMatch()
	{
		int count = 0;
		for (int xx = 0; xx < tile.GetLength(0); xx++)
		{
			for (int yy = 0; yy < tile.GetLength(1); yy++)
			{
				if (tile[xx, yy].GetComponent<GameTile>().active)
				{
					count++;
				}
			}
		}

		if (count != tile.GetLength(0) * tile.GetLength(1))
		{
			return;
		}

		//hor check
		for(int y = 0; y < tile.GetLength(1); y++)
		{
			int type = -1;
			int match = 0;

			for(int x = 0; x < tile.GetLength(0); x++)
			{
				if(type == -1)
				{
					type = tile[x, y].GetComponent<GameTile>().type;
					match++;
					continue;
				}
				else
				{
					if(tile[x, y].GetComponent<GameTile>().type == type)
					{
						match++;

						if(match == 3)
						{
							//TODO: Count more than 3

							int check = 0;

							for(int c = 0; c < match; c++)
							{
								if(tile[x - c, y].GetComponent<GameTile>().active)
								{
									check++;
								}
							}

							if(check == 3)
							{
								DisableTile(tile[x, y]);
								DisableTile(tile[x-1, y]);
								DisableTile(tile[x-2, y]);

								match = 0;
								type = -1;
							}
						}
					}
					else
					{
						match = 1;
						type = tile[x, y].GetComponent<GameTile>().type;
					}
				}
			}
		}

		//vert check
		for(int x = 0; x < tile.GetLength(0); x++)
		{
			for(int y = 0; y < tile.GetLength(1); y++)
			{

			}
		}
	}

	void DisableTile(GameObject tile)
	{
		tile.GetComponent<GameTile>().destroyed = true;
		tile.GetComponent<GameTile>().active = false;
		tile.GetComponent<GameTile>().type = -100;
	}

	void CheckInput()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if(Input.GetMouseButtonDown(0))
		{
			for(int x = 0; x < tile.GetLength(0); x++)
			{
				for(int y = 0; y < tile.GetLength(1); y++)
				{
					if(Contains(tile[x, y].transform.position, mousePos))
					{
						grabbed = tile[x, y];
						grabPos = mousePos;
					}
				}
			}
		}

		if(Input.GetMouseButtonUp(0))
		{
			if(grabbed != null)
			{
				Vector2 dir = mousePos - grabPos;

				if(dir != Vector2.zero)
				{
					if(dir.x > .5f)
					{
						MoveTile(1, 0);
					}
					else if(dir.x < -.5f)
					{
						MoveTile(-1, 0);
					}

					if(dir.y > .5f)
					{
						MoveTile(0, 1);

					}
					else if(dir.y < -.5f)
					{
						MoveTile(0, -1);
					}

					CheckMatch();
					return;
				}
			}

			grabbed = null;
		}
	}

	void MoveTile(int x, int y)
	{
		Vector2 pos = grabbed.GetComponent<GameTile>().pos;

		if((int)pos.y + y < 0)
		{
			return;
		}

		if((int)pos.y + y > tile.GetLength(1)-1)
		{
			return;
		}

		if((int)pos.x + x < 0)
		{
			return;
		}

		if((int)pos.x + x > tile.GetLength(0) - 1)
		{
			return;
		}

		GameObject oldTile = tile[(int)pos.x+x, (int)pos.y+y];

		grabbed.GetComponent<GameTile>().pos = oldTile.GetComponent<GameTile>().pos;
		grabbed.GetComponent<GameTile>().active = false;
		tile[(int)grabbed.GetComponent<GameTile>().pos.x, (int)grabbed.GetComponent<GameTile>().pos.y] = grabbed;

		oldTile.GetComponent<GameTile>().pos = pos;
		oldTile.GetComponent<GameTile>().active = false;
		tile[(int)pos.x, (int)pos.y] = oldTile;

		grabbed = null;
	}

	bool Contains(Vector2 a, Vector2 b)
	{
		//left side
		if(b.x < a.x - 0.5f)
		{
			return false;
		}

		//right side
		if(b.x > a.x + 0.5f)
		{
			return false;
		}

		//up side
		if(b.y > a.y + 0.5f)
		{
			return false;
		}

		//down side
		if(b.y < a.y - 0.5f)
		{
			return false;
		}
		
		return true;
	}

	void DrawTiles()
	{
		for(int x = 0; x < tile.GetLength(0); x++)
		{
			for(int y = 0; y < tile.GetLength(1); y++)
			{
				if (Vector3.Distance(tile[x, y].transform.position, tile[x, y].GetComponent<GameTile>().pos) > 2)
				{
					print("teleporting");
					tile[x, y].transform.position = tile[x, y].GetComponent<GameTile>().pos;
				}
				else
				{
					tile[x, y].transform.position = Vector2.MoveTowards(tile[x, y].transform.position, new Vector2(x, y), Time.deltaTime * 2);
				}
			}
		}
	}

	void InitTiles()
	{
		for(int x = 0; x < tile.GetLength(0); x++)
		{
			for(int y = 0; y < tile.GetLength(1); y++)
			{
				AddRandomTile(x, y);
			}
		}
	}
	
	void AddRandomTile(int x, int y)
	{
		int selected = Random.Range(0, 6);

		GameObject spawn = (GameObject)Instantiate(Resources.Load("GameTile"));
		Color c = spawn.GetComponent<SpriteRenderer>().color;

		switch(selected)
		{
			case 0:
				c = Color.red;
				break;

			case 1:
				c = Color.green;
				break;

			case 2:
				c = Color.blue;
				break;

			case 3:
				c = Color.cyan;
				break;

			case 4:
				c = Color.yellow;
				break;

			case 5:
				c = Color.grey;
				break;
		}

		spawn.GetComponent<SpriteRenderer>().color = c;

		spawn.GetComponent<GameTile>().Init(null, selected, spawn, new Vector2(x, y));
		spawn.transform.position = new Vector2(x, y);

		tile[x, y] = spawn;
	}
}