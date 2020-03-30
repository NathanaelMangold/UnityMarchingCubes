using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	GameObject chunkObject;

	List<Vector3> vertecies = new List<Vector3>();
	List<int> triangles = new List<int>();

	MeshFilter	 meshFilter;
	MeshCollider meshCollider;
	MeshRenderer meshRenderer;

	Vector3Int chunkPosition;
	float[,,]  terrainMap;

	int width  = GameData.ChunkWidth;
	int height = GameData.ChunkHeight;

	public Chunk(Vector3Int position)
	{
		chunkObject = new GameObject();
		chunkObject.name = string.Format("Chunk {0} {1}", position.x, position.z);
		chunkPosition = position;
		chunkObject.transform.position = chunkPosition;

		meshFilter	 = chunkObject.AddComponent<MeshFilter>();
		meshCollider = chunkObject.AddComponent<MeshCollider>();
		meshRenderer = chunkObject.AddComponent<MeshRenderer>();
		meshRenderer.material = Resources.Load<Material>("Materials/TestColor");

		chunkObject.transform.tag = "Terrain";

		terrainMap = new float[width + 1, height + 1, width + 1];

		GenerateTerrainMapData();
		CreateMeshData();
		BuildMesh();
	}

	private void GenerateTerrainMapData()
	{
		for (int x = 0; x <= width; x++)
		{
			for (int y = 0; y <= height; y++)
			{
				for (int z = 0; z <= width; z++)
				{
					float thisHeight = GameData.GetTerrainHeight(x + chunkPosition.x, z + chunkPosition.z);

					terrainMap[x, y, z] = (float)y - thisHeight;
				}
			}

		}
	}

	private void CreateMeshData()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 0; z < width; z++)
				{
					MarchCube(new Vector3Int(x, y, z));
				}
			}
		}
	}

	int GetCubeConfiguration(float[] cube)
	{
		int configurationIndex = 0;
		for (int i = 0; i < 8; i++)
		{
			if (cube[i] > GameData.terrainSurface)
				configurationIndex |= 1 << i;
		}

		return configurationIndex;
	}

	float SampleTerrain(Vector3Int point)
	{
		return terrainMap[point.x, point.y, point.z];
	}

	void MarchCube(Vector3Int position)
	{
		float[] cube = new float[8];
		for (int i = 0; i < 8; i++)
		{
			cube[i] = SampleTerrain(position + GameData.CornerTable[i]);
		}

		int configurationIndex = GetCubeConfiguration(cube);

		// Below Terrain or completly outside (Air)
		if (configurationIndex == 0 || configurationIndex == 255)
			return;

		int edgeIndex = 0;
		for (int i = 0; i < 5; i++)
		{
			for (int p = 0; p < 3; p++)
			{
				int indice = GameData.TriangleTable[configurationIndex, edgeIndex];

				if (indice == -1)
					return;

				Vector3 vert1 = position + GameData.CornerTable[GameData.EdgeTable[indice, 0]];
				Vector3 vert2 = position + GameData.CornerTable[GameData.EdgeTable[indice, 1]];

				Vector3 vertPosition = (vert1 + vert2) / 2f;

				vertecies.Add(vertPosition);
				triangles.Add(vertecies.Count - 1);
				edgeIndex++;
			}
		}
	}

	// Clear current MeshData
	void ClearMeshData()
	{
		vertecies.Clear();
		triangles.Clear();
	}

	// Reassigns the Vertecies and Triangles and rebuilds the mesh
	void BuildMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertecies.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
		meshFilter.mesh = mesh;
		meshCollider.sharedMesh = mesh;
	}

}