using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadMeshGenerator : MonoBehaviour
{
	public bool generate = false;
	public QuadtreeComponent quadtree;
	public Material mat;
	public Material mat2;

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

		//Set up collider
		GameObject terrain = new GameObject("Terrain");
		terrain.layer = gameObject.layer;
		terrain.transform.parent = quadtree.transform;

		Mesh mesh = new Mesh();
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();

		//set up grass
		GameObject up = new GameObject("Grass");
		up.transform.SetParent(terrain.transform);

		Mesh grassMesh = new Mesh();
		List<Vector3> grassVerts = new List<Vector3>();
		List<int> grassTris = new List<int>();

		//set up terrain
		GameObject stone = new GameObject("Stone");
		stone.transform.SetParent(terrain.transform);

		Mesh stoneMesh = new Mesh();
		List<Vector3> stoneVerts = new List<Vector3>();
		List<int> stoneTris = new List<int>();

		//UVs
		List<Vector2> uvsStone = new List<Vector2>();
		List<Vector2> uvsGrass = new List<Vector2>();

		foreach(var leaf in quadtree.Quadtree.GetLeafNodes())
		{
			if(leaf.Type != 0)
			{
				Vector3 ul = new Vector3(leaf.Position.x - leaf.Size * 0.5f, leaf.Position.y + leaf.Size * 0.5f);
				var initialIndex = verts.Count;
				var initialIndexGrass = grassVerts.Count;
				var initialIndexStone= stoneVerts.Count;

				//locations of each corner point of the leaf
				verts.Add(ul);
				verts.Add(ul + Vector3.right * leaf.Size);
				verts.Add(ul + Vector3.down * leaf.Size);
				verts.Add(ul + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

				//tri 1
				tris.Add(initialIndex);
				tris.Add(initialIndex + 1);
				tris.Add(initialIndex + 2);

				//tri 2
				tris.Add(initialIndex + 1);
				tris.Add(initialIndex + 3);
				tris.Add(initialIndex + 2);

				if(leaf.Type == 1)
				{
					//setting uvs - these are the same as the verts
					stoneVerts.Add(ul);
					stoneVerts.Add(ul + Vector3.right * leaf.Size);
					stoneVerts.Add(ul + Vector3.down * leaf.Size);
					stoneVerts.Add(ul + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

					//setting uvs - these are the same as the verts
					uvsStone.Add(ul);
					uvsStone.Add(ul + Vector3.right * leaf.Size);
					uvsStone.Add(ul + Vector3.down * leaf.Size);
					uvsStone.Add(ul + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

					//tri 1
					stoneTris.Add(initialIndexStone);
					stoneTris.Add(initialIndexStone + 1);
					stoneTris.Add(initialIndexStone + 2);

					//tri 2
					stoneTris.Add(initialIndexStone + 1);
					stoneTris.Add(initialIndexStone + 3);
					stoneTris.Add(initialIndexStone + 2);
				}
				else if(leaf.Type == 2)
				{
					grassVerts.Add(ul);
					grassVerts.Add(ul + Vector3.right * leaf.Size);
					grassVerts.Add(ul + Vector3.down * leaf.Size);
					grassVerts.Add(ul + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

					//setting uvs - these are the same as the verts
					uvsGrass.Add(ul);
					uvsGrass.Add(ul + Vector3.right * leaf.Size);
					uvsGrass.Add(ul + Vector3.down * leaf.Size);
					uvsGrass.Add(ul + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

					//tri 1
					grassTris.Add(initialIndexGrass);
					grassTris.Add(initialIndexGrass + 1);
					grassTris.Add(initialIndexGrass + 2);

					//tri 2
					grassTris.Add(initialIndexGrass + 1);
					grassTris.Add(initialIndexGrass + 3);
					grassTris.Add(initialIndexGrass + 2);
				}
			}
		}

		mesh.SetVertices(verts);
		mesh.SetTriangles(tris, 0);

		//mesh.SetUVs(0, uvs);
		//mesh.SetUVs(1, uvsGrass);

		//MeshFilter filter = terrain.AddComponent<MeshFilter>();
		//MeshRenderer renderer = terrain.AddComponent<MeshRenderer>();
		MeshFilter Colfilter = terrain.AddComponent<MeshFilter>();
		Colfilter.mesh = mesh;
		MeshCollider col = terrain.AddComponent<MeshCollider>();
		col.sharedMesh = mesh;

		//renderer.materials = new Material[] {mat, mat2};

		//filter.mesh = mesh;

		////////// //grass Mesh stuff
		grassMesh.SetVertices(grassVerts);
		grassMesh.SetTriangles(grassTris, 0);
		grassMesh.SetUVs(0, uvsGrass);

		MeshFilter filter;
		MeshRenderer renderer;

		filter = up.AddComponent<MeshFilter>();
		renderer = up.AddComponent<MeshRenderer>();
		renderer.material = mat;
		filter.mesh = grassMesh;

		////////// //stone Mesh stuff
		stoneMesh.SetVertices(stoneVerts);
		stoneMesh.SetTriangles(stoneTris, 0);
		stoneMesh.SetUVs(0, uvsStone);

		filter = stone.AddComponent<MeshFilter>();
		renderer = stone.AddComponent<MeshRenderer>();
		renderer.material = mat2;
		filter.mesh = stoneMesh;

		//assign the terrain so that we can destroy it on regenerate
		lastMesh = terrain;
	}
}
