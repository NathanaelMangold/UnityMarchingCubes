using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVisualizer : MonoBehaviour
{
	public WorldGenerator wg;

	void OnDrawGizmos()
	{
		if (Application.isPlaying == false)
		{
			return;
		}

		Chunk chunk = wg.getChunk(Vector3Int.zero);
		float[,,] terrainMap = chunk.getTerrainMap();

		if (chunk == null)
		{
			Debug.LogWarning("Chunk is null!");
			return;
		}

		for (int x = 0; x < 17; x++)
		{
			for(int y = 0; y < 17; y++)
			{
				for(int z = 0; z < 17; z++)
				{
					//if (terrainMap[x, y, z] > GameData.terrainSurface)
					//{
						//Gizmos.DrawSphere(chunk.getChunkPosition() + new Vector3(x, y, z), 0.25f);
					//}
				}
			}
		}
	}
}
