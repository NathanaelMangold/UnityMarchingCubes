using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    void Update()
    {
        RaycastHit hit;
        if(Input.GetMouseButtonDown(0) == true)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (!hit.transform.CompareTag("Terrain"))
                    return;

                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);

                Chunk hitChunk = GameController.Instance.getWorldGenerator().getChunkEstimatePosition(hit.point);
                Debug.Log(hitChunk.ToString());

                hitChunk.changeTerrain(hit.point, 0);

            }
        }
    }
}
