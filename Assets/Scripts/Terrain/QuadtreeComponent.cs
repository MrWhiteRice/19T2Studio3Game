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

	public UnityEngine.UI.Text text;

	public Quadtree<bool> Quadtree
	{
		get { return quadtree; }
	}

	private void Start()
	{
		text = FindObjectOfType<UnityEngine.UI.Text>();
		quadtree = new Quadtree<bool>(this.transform.position, size, depth);
	}

	private void Update()
	{
		text.text = (int)(1.0f / Time.smoothDeltaTime) + "";
	}

	private void OnDrawGizmos()
	{
		if(quadtree != null)
		{
			foreach(var node in quadtree.Nodes)
			{
				DrawNode(node);
			}
		}
	}

	private Color minColor = new Color(1, 1, 1, 1);
	private Color maxColor = new Color(0, 0.5f, 1, 0.25f);

	private void DrawNode(Quadtree<bool>.QuadtreeNode<bool> node, int nodeDepth = 0)
	{
		Gizmos.color = Color.Lerp(minColor, maxColor, nodeDepth / (float)depth);
		Gizmos.DrawWireCube(node.Position, new Vector3(1, 1, 0) * node.Size);
	}
}