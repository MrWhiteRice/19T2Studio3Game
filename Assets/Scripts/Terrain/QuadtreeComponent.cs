using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeComponent : MonoBehaviour
{
	public float size = 5;
	public int depth = 1;

	private void OnDrawGizmos()
	{
		var quadtree = new Quadtree<int>(this.transform.position, size, depth);

		DrawNode(quadtree.GetRoot());
	}

	private Color minColor = new Color(1, 1, 1, 1);
	private Color maxColor = new Color(0, 0.5f, 1, 0.25f);

	//TODO: Learn class<>.class<>
	private void DrawNode(Quadtree<int>.QuadTreeNode<int> node, int nodeDepth = 0)
	{
		if(!node.IsLeaf())
		{
			foreach(var subnode in node.Nodes)
			{
				DrawNode(subnode, nodeDepth + 1);
			}
		}

		Gizmos.color = Color.Lerp(minColor, maxColor, nodeDepth / (float)depth);
		Gizmos.DrawWireCube(node.Position, new Vector3(1,1,0) * node.Size);
	}
}
