using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageToVoxel : MonoBehaviour
{
	public Texture2D image;
	public QuadtreeComponent quadtree;
	public float threshold = 0.5f;

    void Start()
    {
		Generate();
    }

	void Generate()
	{
		int cells = (int)Mathf.Pow(2, quadtree.depth);

		for(int x = 0; x <= cells; ++x)
		{
			for(int y = 0; y <= cells; ++y)
			{
				Vector2 pos = quadtree.transform.position;
				pos.x += (x - cells / 2) / (float)cells * quadtree.size;
				pos.y += (y - cells / 2) / (float)cells * quadtree.size;

				Color pixel = image.GetPixelBilinear(x / (float)cells, y / (float)cells);
				if(pixel.r > threshold)
				{
					quadtree.Quadtree.InsertCircle(true, pos, 0.0001f);
				}
			}
		}

		//Reduce after generate
	}
}