using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE: scrip order execution: -90
//if its not set then generators will conflict
public class QuadtreeComponent : MonoBehaviour
{
	public float size = 5;
	public int depth = 1;

	Quadtree<bool> quadtree;

	public Quadtree<bool> Quadtree
	{
		get { return quadtree; }
	}

	private void Start()
	{
		quadtree = new Quadtree<bool>(this.transform.position, size, depth);
	}

	private void OnDrawGizmos()
	{
		if(quadtree != null)
		DrawNode(quadtree.GetRoot());
	}

	private Color minColor = new Color(1, 1, 1, 1);
	private Color maxColor = new Color(0, 0.5f, 1, 0.25f);

	private void DrawNode(Quadtree<bool>.QuadtreeNode<bool> node, int nodeDepth = 0)
	{
		if(!node.IsLeaf())
		{
			if(node.Nodes != null)
			{
				foreach(var subnode in node.Nodes)
				{
					if(subnode != null)
					{
						DrawNode(subnode, nodeDepth + 1);
					}
				}
			}
		}

		Gizmos.color = Color.Lerp(minColor, maxColor, nodeDepth / (float)depth);
		Gizmos.DrawWireCube(node.Position, new Vector3(1,1,0) * node.Size);
	}
}