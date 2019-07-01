using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadMeshGenerator : MonoBehaviour
{
	public bool generate = false;
	public QuadtreeComponent quadtree;
	public Material mat;

	private GameObject lastMesh;

	private void Start()
	{
		quadtree.Quadtree.QuadtreeUpdated += (obj, args) => { generate = true; };
	}

	void Update()
    {
		if(generate)
		{
			GenerateMesh();
			generate = false;
		}
    }

	void GenerateMesh()
	{
		if(lastMesh)
		{
			Destroy(lastMesh);
		}

		GameObject terrain = new GameObject("Terrain");
		terrain.layer = gameObject.layer;
		terrain.transform.parent = quadtree.transform;

		Mesh mesh = new Mesh();
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();

		foreach(var leaf in quadtree.Quadtree.GetLeafNodes())
		{
			if(leaf.Type)
			{
				Vector3 ul = new Vector3(leaf.Position.x - leaf.Size * 0.5f, leaf.Position.y + leaf.Size * 0.5f);
				var initialIndex = verts.Count;

				//locations of each corner point of the leaf
				verts.Add(ul);
				verts.Add(ul + Vector3.right * leaf.Size);
				verts.Add(ul + Vector3.down * leaf.Size);
				verts.Add(ul + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

				//setting uvs - these are the same as the verts
				uvs.Add(ul);
				uvs.Add(ul + Vector3.right * leaf.Size);
				uvs.Add(ul + Vector3.down * leaf.Size);
				uvs.Add(ul + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

				//tri 1
				tris.Add(initialIndex);
				tris.Add(initialIndex + 1);
				tris.Add(initialIndex + 2);

				//tri 2
				tris.Add(initialIndex + 1);
				tris.Add(initialIndex + 3);
				tris.Add(initialIndex + 2);
			}
		}

		mesh.SetVertices(verts);
		mesh.SetTriangles(tris, 0);
		mesh.SetUVs(0, uvs);

		MeshFilter filter = terrain.AddComponent<MeshFilter>();
		MeshRenderer renderer = terrain.AddComponent<MeshRenderer>();
		MeshCollider col = terrain.AddComponent<MeshCollider>();
		col.sharedMesh = mesh;

		renderer.material = mat;
		filter.mesh = mesh;

		lastMesh = terrain;
	}
}
