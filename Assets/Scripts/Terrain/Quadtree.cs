using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuadtreeIndex
{
	TopLeft = 0,    //00
	TopRight = 1,   //01
	BottomLeft = 2, //10
	BottomRight = 3 //11
}

public class Quadtree<TType> where TType : IComparable
{
	private readonly QuadtreeNode<TType>[] nodes;
	private int depth;

	public event EventHandler QuadtreeUpdated;

	public QuadtreeNode<TType>[] Nodes
	{
		get { return nodes; }
	}

	public Quadtree(Vector2 position, float size, int depth)
	{
		this.depth = depth;
		nodes = BuildQuadtree(position, size, depth);
	}

	QuadtreeNode<TType>[] BuildQuadtree(Vector2 position, float size, int depth)
	{
		int width = 0;
		for(int i = 0; i <= depth; ++i)
		{
			width += (int)Mathf.Pow(4, i);
		}

		var tree = new QuadtreeNode<TType>[width];

		tree[0] = new QuadtreeNode<TType>(position, size, 0);
		BuildQuadtreeRecursive(tree, 0);

		return tree;
	}

	private void BuildQuadtreeRecursive(QuadtreeNode<TType>[] tree, int index)
	{
		if(tree[index].Depth >= this.depth)
		{
			return;
		}

		int nextNode = 4 * index;
		Vector2 deltaX = new Vector2(tree[index].Size / 4, 0);
		Vector2 deltaY = new Vector2(0, tree[index].Size / 4);

		tree[nextNode + 1] = new QuadtreeNode<TType>(tree[index].Position - deltaX + deltaY, tree[index].Size / 2, tree[index].Depth + 1);
		tree[nextNode + 2] = new QuadtreeNode<TType>(tree[index].Position + deltaX + deltaY, tree[index].Size / 2, tree[index].Depth + 1);
		tree[nextNode + 3] = new QuadtreeNode<TType>(tree[index].Position - deltaX - deltaY, tree[index].Size / 2, tree[index].Depth + 1);
		tree[nextNode + 4] = new QuadtreeNode<TType>(tree[index].Position + deltaX - deltaY, tree[index].Size / 2, tree[index].Depth + 1);

		BuildQuadtreeRecursive(tree, nextNode + 1);
		BuildQuadtreeRecursive(tree, nextNode + 2);
		BuildQuadtreeRecursive(tree, nextNode + 3);
		BuildQuadtreeRecursive(tree, nextNode + 4);
	}

	public void InsertCircle(TType type, Vector2 pos, float radius)
	{
		var leafNodes = new LinkedList<QuadtreeNode<TType>>();
		SearchCircle(leafNodes, radius, pos, nodes, 0);

		foreach(var quadtreeNode in leafNodes)
		{
			quadtreeNode.Type = type;
		}

		NotifyQuadtreeUpdate();
	}

	private void NotifyQuadtreeUpdate()
	{
		if(QuadtreeUpdated != null)
		{
			QuadtreeUpdated(this, new EventArgs());
		}
	}

	public void SearchCircle(LinkedList<QuadtreeNode<TType>> selectedNodes, float radius, Vector2 targetPos, QuadtreeNode<TType>[] tree, int index)
	{
		if(tree[index].Depth >= this.depth)
		{
			selectedNodes.AddLast(tree[index]);
			return;
		}

		int nextNode = 4 * index;
		for(int x = 1; x <= 4; ++x)
		{
			if(ContainsCircle(targetPos, radius, tree[nextNode + x]))
			{
				SearchCircle(selectedNodes, radius, targetPos, tree, nextNode + x);
			}
		}
	}

	public bool ContainsCircle(Vector2 pos, float radius, QuadtreeNode<TType> node)
	{
		Vector2 diff = node.Position - pos;

		diff.x = Mathf.Max(0, Mathf.Abs(diff.x) - node.Size / 2);
		diff.y = Mathf.Max(0, Mathf.Abs(diff.y) - node.Size / 2);

		return diff.magnitude < radius;
	}

	public class QuadtreeNode<TType> where TType : IComparable
	{
		Vector2 position;
		float size;
		TType type;
		int depth;

		public QuadtreeNode(Vector2 pos, float size, int depth, TType type = default(TType))
		{
			position = pos;
			this.size = size;
			this.depth = depth;
			this.type = type;
		}

		public Vector2 Position
		{
			get	{ return position; }
		}

		public float Size
		{
			get { return size; }
		}

		public TType Type
		{
			get { return type; }
			internal set { type = value; }
		}

		public int Depth
		{
			get { return depth; }
		}
	}

	//private static int GetIndexOfPosition(Vector2 lookupPosition, Vector2 nodePosition)
	//{
	//	int index = 0;

	//	index |= lookupPosition.y < nodePosition.y ? 2 : 0;
	//	index |= lookupPosition.x > nodePosition.x ? 1 : 0;

	//	return index;
	//}

	public IEnumerable<QuadtreeNode<TType>> GetLeafNodes()
	{
		int leafNodes = (int)Mathf.Pow(4, depth);
		for(int i = nodes.Length - leafNodes; i	< nodes.Length; ++i)
		{
			yield return nodes[i];
		}
	}
}