using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int Chunks = 5;

    Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

    private void Start()
    {
        Generate();
    }

    void Generate()
    {
        for(int x = 0; x < Chunks; x++)
        {
            for(int z = 0; z < Chunks; z++)
            {
                Vector3Int chunkPos = new Vector3Int(x * GameData.ChunkWidth, 0, z * GameData.ChunkWidth);
                chunks.Add(chunkPos, new Chunk(chunkPos));
            }

        }
    }
}
