using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	GameObject chunkObject;
	WorldGenerator worldGenerator;

	List<Vector3> vertecies = new List<Vector3>();
	List<int> triangles = new List<int>();

	MeshFilter	 meshFilter;
	MeshCollider meshCollider;
	MeshRenderer meshRenderer;

	Vector3Int chunkPosition;
	float[,,]  terrainMap;

	int width = GameData.ChunkWidth;
	int height = GameData.ChunkHeight;

	public Chunk(Vector3Int position)
	{
		worldGenerator = GameController.Instance.getWorldGenerator();

		chunkObject = new GameObject();
		chunkObject.name = string.Format("Chunk {0} {1}", position.x, position.z);
		chunkPosition = position;
		chunkObject.transform.position = chunkPosition;
		chunkObject.transform.parent = worldGenerator.getChunkHolder();

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

	public Vector3Int getChunkPosition() {
		return chunkPosition;
	}

	public float[,,] getTerrainMap()
	{
		return terrainMap;
	}

	public void changeTerrain(Vector3 pos, float amount)
	{
		pos = pos - chunkPosition;
		Vector3Int posInt= new Vector3Int(Mathf.CeilToInt(pos.x), Mathf.CeilToInt(pos.y), Mathf.CeilToInt(pos.z));

		Debug.Log(pos);
		terrainMap[posInt.x, posInt.y, posInt.z] = amount;
		UpdateChunk();
	}

	public void UpdateChunk()
	{
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

	// Wandelt aus den 8 Werten für einen Würfel in den Konfigurationsindex um
	int GetCubeConfiguration(float[] cube)
	{
		// Gehe alle 8 Punkte durch um zu schauen welche über bzw unter dem Terrain sind
		int configurationIndex = 0;
		for (int i = 0; i < 8; i++)
		{
			// Wenn ein entsprechender Punkt drüber ist dann setzte die entsprechende Position des Bits auf 1
			if (cube[i] > GameData.terrainSurface)
				// Bit Shifting Magie
				configurationIndex |= 1 << i;
		}

		return configurationIndex;
	}

	// Gibt Wert an der der Position zurück
	float SampleTerrain(Vector3Int point)
	{
		return terrainMap[point.x, point.y, point.z];
	}

	// Gehe die Position durch und erstelle die ensprechenden Dreiecke
	void MarchCube(Vector3Int position)
	{
		// Eckpunkte erstellen
		float[] cube = new float[8];
		for (int i = 0; i < 8; i++)
		{
			// Position + "offset" für die tatsächlichen Eckpunkte
			cube[i] = SampleTerrain(position + MarchingCubesData.CornerTable[i]);
		}

		// Konfigurationsindex anhand von den Eckwerten berechnen
		int configurationIndex = GetCubeConfiguration(cube);

		// Below Terrain or completly outside (Air)
		if (configurationIndex == 0 || configurationIndex == 255)
			return;

		// edgeIndex = Momentane Kante die abgearbeitet wird
		int edgeIndex = 0;
		// Maximal 5 Dreiecke für jede Kombination in Marching Cube
		for (int i = 0; i < 5; i++)
		{
			// Immer 3 Punkte pro Dreieck
			for (int p = 0; p < 3; p++)
			{
				// Gibt zurück welcher der Kanten verbunden werden muss
				int indice = MarchingCubesData.TriangleTable[configurationIndex, edgeIndex];

				// -1 Ende der Konfiguration, keine weiteren Punkte
				if (indice == -1)
					return;

				// Berechnen der tatsächlichen Positionen 
				Vector3 vert1 = position + MarchingCubesData.CornerTable[MarchingCubesData.EdgeTable[indice, 0]];
				Vector3 vert2 = position + MarchingCubesData.CornerTable[MarchingCubesData.EdgeTable[indice, 1]];

				// Tatsächliche Position der "Ecke" ist die Mitte von beiden Punkte
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

	override
	public string ToString()
	{
		return chunkObject.transform.name + " " + chunkPosition;
	}

}