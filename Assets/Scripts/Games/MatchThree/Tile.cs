using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			active = false;
			transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * 5);

			if(Vector2.Distance(transform.position, pos) > 2 && destroyed)
			{
				transform.position = pos;
			}
		}
		else
		{
			active = true;
		}

		if(destroyed)
		{
			name = "[D]" + type.ToString();
			active = false;
			GetComponent<SpriteRenderer>().sprite = null;
		}
		else
		{
			name = type.ToString();
			GetComponent<SpriteRenderer>().sprite = GetSprite(type.ToString());
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

	//Checks if the given vector is inside this tiles bounds
    public bool Contains(Vector2 pos)
    {
		//TODO: condense to one line
        //left
        if (pos.x < transform.position.x - 0.5f)
        {
            return false;
        }

        //right
        if (pos.x > transform.position.x + 0.5f)
        {
            return false;
        }

        //up
        if (pos.y < transform.position.y - 0.5f)
        {
            return false;
        }

        //down
        if (pos.y > transform.position.y + 0.5f)
        {
            return false;
        }

        return true;
    }

	public static Tile GetTile()
	{
		return null;
	}
}