using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    int chunkCount = 5;

    Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

    [SerializeField]
    Transform chunkHolder;

    private void Start()
    {
        Generate();
    }

    public Transform getChunkHolder()
    {
        return chunkHolder;
    }

    public Chunk getChunk(Vector3 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;

        return chunks[new Vector3Int(x, y, z)];
    }

    void Generate()
    {
        for(int x = 0; x < chunkCount; x++)
        {
            for(int z = 0; z < chunkCount; z++)
            {
                Vector3Int chunkPos = new Vector3Int(x * GameData.ChunkWidth, 0, z * GameData.ChunkWidth);
                chunks.Add(chunkPos, new Chunk(chunkPos));
            }

        }
    }
}
