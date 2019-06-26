using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGenerator : MonoBehaviour
{
	public bool generate = false;
	public QuadtreeComponent quadtree;
	public Material mat;
	public Texture2D mesh;

	private GameObject lastMesh;

	private void Start()
	{
		quadtree.Quadtree.QuadtreeUpdated += (obj, args) => { generate = true; };
	}

	void Update()
	{
		if(generate)
		{
			GenerateMesh();
			generate = false;
		}
	}

	void GenerateMesh()
	{
		if(lastMesh)
		{
			Destroy(lastMesh);
		}

		GameObject terrain = new GameObject("Terrain");
		terrain.transform.parent = quadtree.transform;

		mesh = new Texture2D(100, 100);
		mesh.filterMode = FilterMode.Point;

		for(int x = 0; x < mesh.width; x++)
		{
			for(int y = 0; y < mesh.height; y++)
			{
				mesh.SetPixel(x, y, Color.clear);
			}
		}
		mesh.Apply();

		foreach(var leaf in quadtree.Quadtree.GetLeafNodes())
		{
			Vector2 pos = Camera.main.WorldToScreenPoint(leaf.Position);

			if(leaf.Type)
			{
				mesh.SetPixel((int)(leaf.Position.x * 10), (int)(leaf.Position.y * 10), Color.red);
			}
		}

		mesh.Apply();

		SpriteRenderer spr = terrain.AddComponent<SpriteRenderer>();
		spr.sprite = Sprite.Create(mesh, new Rect(0, 0, mesh.width, mesh.height), Vector2.one/2);

		terrain.AddComponent<PolygonCollider2D>();

		//spr.material = mat;

		lastMesh = terrain;
	}
}
