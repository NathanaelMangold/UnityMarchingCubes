using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(WorldDisplay))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WorldDisplay mapGen = (WorldDisplay)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}
