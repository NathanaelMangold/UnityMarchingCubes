using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
	public static float terrainSurface = 0.5f;
	public static readonly int ChunkWidth = 16;
	public static readonly int ChunkHeight = 250;

	public static float BaseTerrainHeight = 10f;
	public static float TerrainHeightRange = 5f;

	public static float GetTerrainHeight(int x, int z)
	{
		return (float)TerrainHeightRange * Mathf.PerlinNoise((float)x / 16f * 1.5f + 0.001f, (float)z / 16f * 1.5f + 0.001f) + BaseTerrainHeight;
	}
	
}
