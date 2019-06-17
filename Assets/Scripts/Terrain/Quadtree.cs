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

public class Quadtree<TType>
{
	private QuadTreeNode<TType> node;
	private int depth;

	public Quadtree(Vector2 position, float size, int depth)
	{
		node = new QuadTreeNode<TType>(position, size);
		node.Subdivide(depth);
	}

	public class QuadTreeNode<TType>
	{
		Vector2 position;
		float size;
		QuadTreeNode<TType>[] subNodes;
		List<TType> value;

		public QuadTreeNode(Vector2 pos, float size)
		{
			position = pos;
			this.size = size;
		}

		public IEnumerable<QuadTreeNode<TType>> Nodes
		{
			get
			{
				return subNodes;
			}
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}
		}

		public float Size
		{
			get
			{
				return size;
			}
		}

		public void Subdivide(int depth)
		{
			subNodes = new QuadTreeNode<TType>[4];

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
					var binaryString = System.Convert.ToString(x, 2);
					Debug.Log("Down" + binaryString);
					newPos.y -= size * 0.25f;
				}
				//y down
				else
				{
					var binaryString = System.Convert.ToString(x, 2);
					Debug.Log("Down" + binaryString);
					newPos.y += size * 0.25f;
				}

				//x up
				if((x & 1) == 1)
				{
					var binaryString = System.Convert.ToString(x, 2);
					Debug.Log("Down" + binaryString);
					newPos.x += size * 0.25f;
				}
				//x down
				else
				{
					var binaryString = System.Convert.ToString(x, 2);
					Debug.Log("Down" + binaryString);
					newPos.x -= size * 0.25f;
				}

				subNodes[x] = new QuadTreeNode<TType>(newPos, size * .5f);

				if(depth > 0)
				{
					subNodes[x].Subdivide(depth - 1);
				}
			}
		}

		public bool IsLeaf()
		{
			return subNodes == null;
		}
	}

	private int GetIndexOfPosition(Vector2 lookupPosition, Vector2 nodePosition)
	{
		int index = 0;

		//TODO Learn |=

		index |= lookupPosition.y > nodePosition.y ? 2 : 0;
		index |= lookupPosition.x > nodePosition.y ? 1 : 0;

		return index;
	}

	public QuadTreeNode<TType> GetRoot()
	{
		return node;
	}
}