using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMaker : MonoBehaviour
{
    public MeshFilter meshFilter;
    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1)
        };

        mesh.triangles = new int[] {
            0, 1, 2,
            3, 2, 1
        };

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        
    }

    // Corner of the cube
    Vector3Int[] CornerTable = new Vector3Int[8] {
        new Vector3Int(0, 0, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(1, 1, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(1, 0, 1),
        new Vector3Int(1, 1, 1),
        new Vector3Int(0, 1, 1)
    };

    // Edges of the cube
    int[,] EdgeTable = new int[12, 2] {

        {0, 1}, {1, 2}, {3, 2}, {0, 3}, {4, 5}, {5, 6}, {7, 6}, {4, 7}, {0, 4}, {1, 5}, {2, 6}, {3, 7}

    };
}
