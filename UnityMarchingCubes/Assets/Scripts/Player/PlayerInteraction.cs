using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    void Update()
    {
        RaycastHit hit;

        // Add Terrain
        if(Input.GetMouseButtonDown(0) == true)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (!hit.transform.CompareTag("Terrain"))
                    return;

                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);

                Chunk hitChunk = GameController.Instance.getWorldGenerator().getChunkAtEstimatePosition(hit.point);
                Debug.Log(hitChunk.ToString());

                hitChunk.changeTerrain(hit.point, 0);

            }
        }

        // Debug Terrain
        if(Input.GetMouseButtonDown(2) == true)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (!hit.transform.CompareTag("Terrain"))
                    return;

                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);

                Chunk hitChunk = GameController.Instance.getWorldGenerator().getChunkAtEstimatePosition(hit.point);
                Vector3Int hitInt = new Vector3Int(Mathf.CeilToInt(hit.point.x), Mathf.CeilToInt(hit.point.y), Mathf.CeilToInt(hit.point.z));
                Debug.Log("Hit Point: " + hit.point);
                Debug.Log("Hit INTPoint: " + hitInt);
                Debug.Log("Value: " + hitChunk.SampleTerrain(hitInt));
       
            }

        }
    }
}
