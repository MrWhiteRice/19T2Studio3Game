using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeCircle : MonoBehaviour
{
	public QuadtreeComponent quadtree;

    void Update()
    {
		Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if(Input.GetMouseButtonDown(0))
		{
			quadtree.Quadtree.InsertCircle(1, pos, .5f);
		}
		else if(Input.GetMouseButtonDown(1))
		{
			quadtree.Quadtree.InsertCircle(0, pos, .5f);
		}
    }
}