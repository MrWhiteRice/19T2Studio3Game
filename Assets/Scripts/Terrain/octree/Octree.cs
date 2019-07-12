using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OctreeIndex
{
	UpperLeftFront = 4, //000
	UpperRightFront = 6, //010
	UpperLeftBack = 5, //011
	UpperRightBack = 7, //001

	LowerLeftFront = 0, //100
	LowerRightFront = 2, //110
	LowerLeftBack = 1, //111
	LowerRightBack = 3 //101
}

public class Octree<TType>
{
	OctreeNode<TType> node;
	int depth;

	public Octree(Vector3 position, float size, int depth)
	{
		node = new OctreeNode<TType>(position, size);
		node.Subdivide(--depth);
	}

	public class OctreeNode<TType>
	{
		OctreeNode<TType>[] subNodes;
		IList<TType> value;

		Vector3 position;
		float size;

		public OctreeNode(Vector3 pos, float size)
		{
			position = pos;
			this.size = size;
		}

		public IEnumerable<OctreeNode<TType>> Node
		{
			get { return subNodes; }
		}

		public float Size
		{
			get { return size; }
		}

		public Vector3 Position
		{
			get { return position; }
		}

		public void Subdivide(int depth)
		{
			subNodes = new OctreeNode<TType>[8];

			for(int i = 0; i < subNodes.Length; ++i)
			{
				Vector3 newPos = position;

				//up down
				if((i & 4) == 4)
				{
					newPos.y += size * 0.25f;
				}
				else
				{
					newPos.y -= size * 0.25f;
				}

				//left right
				if((i & 2) == 2)
				{
					newPos.x += size * 0.25f;
				}
				else
				{
					newPos.x -= size * 0.25f;
				}

				//front back
				if((i & 1) == 1)
				{
					newPos.z += size * 0.25f;
				}
				else
				{
					newPos.z -= size * 0.25f;
				}

				subNodes[i] = new OctreeNode<TType>(newPos, size * 0.5f);

				if(depth > 0)
				{
					subNodes[i].Subdivide(depth - 1);
				}
			}
		}

		public bool IsLeaf()
		{
			return subNodes == null;
		}
	}

	private int GetIndexOfPosition(Vector3 lookupPosition, Vector3 nodePosition)
	{
		int index = 0;

		index |= lookupPosition.y > nodePosition.y ? 0 : 4;
		index |= lookupPosition.x > nodePosition.x ? 0 : 2;
		index |= lookupPosition.z > nodePosition.z ? 0 : 1;

		return index;
	}

	public OctreeNode<TType> GetRoot()
	{
		return node;
	}
}