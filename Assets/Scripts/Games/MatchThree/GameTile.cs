using UnityEngine;

public class GameTile : MonoBehaviour
{
	public GameObject refTile;

	public Sprite icon;
	public int type;

	public Vector2 pos;
	public bool active;

	public bool destroyed = false;

	public void Init(Sprite Icon, int Type, GameObject RefTile, Vector2 Position)
	{
		icon = Icon;
		type = Type;
		refTile = RefTile;
		pos = Position;
	}

	private void Update()
	{
		GetComponent<SpriteRenderer>().enabled = !destroyed;

		Vector2 position = transform.position;
		if(position == pos)
		{
			active = true;
		}
		else
		{
			active = false;
		}
	}
}