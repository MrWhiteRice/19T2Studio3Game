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
	private QuadtreeNode<TType> node;
	private int depth;

	public event EventHandler QuadtreeUpdated;

	public Quadtree(Vector2 position, float size, int depth)
	{
		node = new QuadtreeNode<TType>(position, size, depth);
		this.depth = depth;
	}

	public void Insert(TType type, Vector2 pos)
	{
		QuadtreeNode<TType> leafNode = node.Subdivide(type, pos, depth);
		leafNode.Type = type;

		NotifyQuadtreeUpdate();
	}

	public void InsertCircle(TType type, Vector2 pos, float radius)
	{
		//if in node
		//sub divide it
		//check leaf distance < radius
		//delete//add 

		var leafNodes = new LinkedList<QuadtreeNode<TType>>();
		node.SubdivideCircle(leafNodes, type, radius, pos, depth);
		NotifyQuadtreeUpdate();
	}

	private void NotifyQuadtreeUpdate()
	{
		if(QuadtreeUpdated != null)
		{
			QuadtreeUpdated(this, new EventArgs());
		}
	}

	public class QuadtreeNode<TType> where TType : IComparable
	{
		Vector2 position;
		float size;
		QuadtreeNode<TType>[] subNodes;
		TType type;
		int depth;

		public QuadtreeNode(Vector2 pos, float size, int depth, TType type = default(TType))
		{
			position = pos;
			this.size = size;
			this.depth = depth;
			this.type = type;
		}

		public IEnumerable<QuadtreeNode<TType>> Nodes
		{
			get { return subNodes; }
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

		public QuadtreeNode<TType> Subdivide(TType type, Vector2 targetPos, int depth = 0)
		{
			if(depth == 0)
			{
				return this;
			}

			int subDivideIndex = GetIndexOfPosition(targetPos, position);

			if(subNodes == null)
			{
				subNodes = new QuadtreeNode<TType>[4];

				for(int x = 0; x < subNodes.Length; ++x)
				{
					Vector2 newPos = position;

					//& is a bitwise AND check
					//so x = 6 & 4 ---- convert both numbers into binary
					//6 = 1110 || 4 = 0110 ---- the result will be 0110
					//2^(n-1)
					//8421
					//0000 0001 0010 0011 0100
					//   0    1    2    3    4

					//y up
					if((x & 2) == 2)
					{
						newPos.y -= size * 0.25f;
					}
					//y down
					else
					{
						newPos.y += size * 0.25f;
					}

					//x up
					if((x & 1) == 1)
					{
						newPos.x += size * 0.25f;
					}
					//x down
					else
					{
						newPos.x -= size * 0.25f;
					}

					subNodes[x] = new QuadtreeNode<TType>(newPos, size * .5f, depth - 1);
				}
			}

			return subNodes[subDivideIndex].Subdivide(type, targetPos, depth - 1);
		}
		
		public void SubdivideCircle(LinkedList<QuadtreeNode<TType>> selectedNodes, TType type, float radius, Vector2 targetPos, int depth = 0)
		{
			if(depth == 0)
			{
				this.Type = type;
				selectedNodes.AddLast(this);
				return;
			}

			int subDivideIndex = GetIndexOfPosition(targetPos, position);

			if(subNodes == null)
			{
				subNodes = new QuadtreeNode<TType>[4];

				for(int x = 0; x < subNodes.Length; ++x)
				{
					Vector2 newPos = position;

					//& is a bitwise AND check
					//so x = 6 & 4 ---- convert both numbers into binary
					//6 = 1110 || 4 = 0110 ---- the result will be 0110
					//2^(n-1)
					//8421
					//0000 0001 0010 0011 0100
					//   0    1    2    3    4

					//y up
					if((x & 2) == 2)
					{
						newPos.y -= size * 0.25f;
					}
					//y down
					else
					{
						newPos.y += size * 0.25f;
					}

					//x up
					if((x & 1) == 1)
					{
						newPos.x += size * 0.25f;
					}
					//x down
					else
					{
						newPos.x -= size * 0.25f;
					}

					subNodes[x] = new QuadtreeNode<TType>(newPos, size * .5f, depth - 1, Type);
				}
			}

			for(int x = 0; x < subNodes.Length; ++x)
			{
				if(subNodes[x].ContainsCircle(targetPos, radius))
				{
					subNodes[x].SubdivideCircle(selectedNodes ,type, radius, targetPos, depth - 1);
				}
			}

			var shouldReduce = true;
			var initialValue = subNodes[0].Type;

			for(int y = 1; y < subNodes.Length; ++y)
			{
				shouldReduce &= (initialValue.CompareTo(subNodes[y].Type) == 0);
				shouldReduce &= (subNodes[y].IsLeaf());
			}

			if(shouldReduce)
			{
				this.type = initialValue;
				subNodes = null;
			}
		}

		public bool ContainsCircle(Vector2 pos, float radius)
		{
			Vector2 diff = this.position - pos;

			diff.x = Mathf.Max(0, Mathf.Abs(diff.x) - size / 2);
			diff.y = Mathf.Max(0, Mathf.Abs(diff.y) - size / 2);

			return diff.magnitude < radius;
		}

		public bool IsLeaf()
		{
			return Nodes == null;
		}

		public IEnumerable<QuadtreeNode<TType>> GetLeafNodes()
		{
			if(IsLeaf())
			{
				yield return this;
			}
			else
			{
				if(Nodes != null)
				{
					foreach(var node in subNodes)
					{
						foreach(var leaf in node.GetLeafNodes())
						{
							yield return leaf;
						}
					}
				}
			}
		}
	}

	private static int GetIndexOfPosition(Vector2 lookupPosition, Vector2 nodePosition)
	{
		int index = 0;

		//TODO Learn |=

		index |= lookupPosition.y < nodePosition.y ? 2 : 0;
		index |= lookupPosition.x > nodePosition.x ? 1 : 0;

		return index;
	}

	public QuadtreeNode<TType> GetRoot()
	{
		return node;
	}

	public IEnumerable<QuadtreeNode<TType>> GetLeafNodes()
	{
		return node.GetLeafNodes();
	}
}