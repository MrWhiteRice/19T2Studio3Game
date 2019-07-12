using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeComponent : MonoBehaviour
{
	public float size;
	public int depth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDrawGizmos()
	{
		var octree = new Octree<int>(transform.position, size, depth);

		DrawNode(octree.GetRoot());
	}

	public void DrawNode(Octree<int>.OctreeNode<int> node)
	{
		if(node.IsLeaf())
		{
			Gizmos.color = Color.green;
		}
		else
		{
			Gizmos.color = Color.blue;
			foreach(var subNode in node.Node)
			{
				DrawNode(subNode);
			}
		}

		Gizmos.DrawWireCube(node.Position, Vector3.one * node.Size);
	}
}
